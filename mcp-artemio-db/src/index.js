#!/usr/bin/env node
import { McpServer } from "@modelcontextprotocol/sdk/server/mcp.js";
import { StdioServerTransport } from "@modelcontextprotocol/sdk/server/stdio.js";
import { StreamableHTTPServerTransport } from "@modelcontextprotocol/sdk/server/streamableHttp.js";
import { z } from "zod";
import mysql from "mysql2/promise";
import dotenv from "dotenv";
import http from "node:http";
import crypto from "node:crypto";
import path from "node:path";
import { fileURLToPath } from "node:url";

// Carga el .env que esté junto a este archivo, sin importar desde dónde se invoque el proceso.
const __dirname = path.dirname(fileURLToPath(import.meta.url));
dotenv.config({ path: path.join(__dirname, "..", ".env") });

const ALLOW_WRITE = (process.env.ALLOW_WRITE || "false").toLowerCase() === "true";
const MAX_ROWS = parseInt(process.env.MAX_ROWS || "500", 10);

// "stdio" (por defecto, para Claude Desktop/Code local) o "http" (para correrlo
// como servidor remoto, ej. en un hosting con cPanel + Setup Node.js App).
const MCP_TRANSPORT = (process.env.MCP_TRANSPORT || "stdio").toLowerCase();
const PORT = parseInt(process.env.PORT || "3000", 10);
const AUTH_TOKEN = process.env.MCP_AUTH_TOKEN;

const REQUIRED_ENV = ["DB_HOST", "DB_PORT", "DB_NAME", "DB_USER", "DB_PASSWORD"];
for (const key of REQUIRED_ENV) {
  if (!process.env[key]) {
    console.error(`[mcp-artemio-db] Falta la variable de entorno ${key}. Revisa tu archivo .env.`);
  }
}

let pool;
function getPool() {
  if (!pool) {
    console.error(`[mcp-artemio-db] Creando pool de conexión a ${process.env.DB_HOST}:${process.env.DB_PORT}/${process.env.DB_NAME}...`);
    pool = mysql.createPool({
      host: process.env.DB_HOST,
      port: parseInt(process.env.DB_PORT || "3306", 10),
      database: process.env.DB_NAME,
      user: process.env.DB_USER,
      password: process.env.DB_PASSWORD,
      ssl: (process.env.DB_SSL || "true").toLowerCase() === "true" ? { rejectUnauthorized: false } : undefined,
      // Equivalente a "Convert Zero Datetime=True": evita que fechas 0000-00-00 rompan la consulta.
      dateStrings: true,
      waitForConnections: true,
      connectionLimit: 5,
      queueLimit: 0,
    });
  }
  return pool;
}

async function testConnection() {
  try {
    const conn = await getPool().getConnection();
    console.error("[mcp-artemio-db] Conexión a la base de datos OK.");
    conn.release();
  } catch (err) {
    console.error("[mcp-artemio-db] No se pudo conectar a la base de datos:", err.message);
  }
}

const WRITE_KEYWORDS = [
  "INSERT",
  "UPDATE",
  "DELETE",
  "DROP",
  "ALTER",
  "CREATE",
  "TRUNCATE",
  "REPLACE",
  "GRANT",
  "REVOKE",
  "RENAME",
  "LOCK",
];

function isWriteQuery(sql) {
  const normalized = sql.trim().replace(/^\(+/, "").toUpperCase();
  return WRITE_KEYWORDS.some((k) => normalized.startsWith(k));
}

function errorText(err) {
  return `Error: ${err.message}`;
}

const server = new McpServer({
  name: "artemio-mysql",
  version: "1.0.0",
});

server.tool(
  "query",
  "Ejecuta una consulta SQL contra la base de datos de producción afuenzal_artemio (MySQL). " +
    "Por defecto solo se permiten consultas de lectura (SELECT, SHOW, DESCRIBE, EXPLAIN). " +
    "El resultado se limita a MAX_ROWS filas.",
  {
    sql: z.string().describe("Sentencia SQL a ejecutar"),
    params: z
      .array(z.union([z.string(), z.number(), z.null()]))
      .optional()
      .describe("Parámetros opcionales para una consulta preparada (usa '?' en el SQL)"),
  },
  async ({ sql, params }) => {
    if (!ALLOW_WRITE && isWriteQuery(sql)) {
      return {
        isError: true,
        content: [
          {
            type: "text",
            text:
              "Bloqueado: esta consulta modifica datos y ALLOW_WRITE no está habilitado. " +
              "Si realmente necesitas escritura en producción, cambia ALLOW_WRITE=true en el .env (no recomendado).",
          },
        ],
      };
    }
    try {
      const conn = getPool();
      const [rows] = await conn.query(sql, params || []);
      const limited = Array.isArray(rows) ? rows.slice(0, MAX_ROWS) : rows;
      const truncated = Array.isArray(rows) && rows.length > MAX_ROWS;
      return {
        content: [
          {
            type: "text",
            text:
              JSON.stringify(limited, null, 2) +
              (truncated ? `\n\n[Resultado truncado a ${MAX_ROWS} filas de ${rows.length}]` : ""),
          },
        ],
      };
    } catch (err) {
      return { isError: true, content: [{ type: "text", text: errorText(err) }] };
    }
  }
);

server.tool(
  "list_tables",
  "Lista todas las tablas de la base de datos afuenzal_artemio",
  {},
  async () => {
    try {
      const conn = getPool();
      const [rows] = await conn.query("SHOW TABLES");
      return { content: [{ type: "text", text: JSON.stringify(rows, null, 2) }] };
    } catch (err) {
      return { isError: true, content: [{ type: "text", text: errorText(err) }] };
    }
  }
);

server.tool(
  "describe_table",
  "Muestra la estructura de una tabla (columnas, tipos, claves, nulabilidad, default)",
  {
    table: z.string().describe("Nombre exacto de la tabla"),
  },
  async ({ table }) => {
    try {
      const conn = getPool();
      const [tables] = await conn.query("SHOW TABLES");
      const tableNames = tables.map((r) => Object.values(r)[0]);
      if (!tableNames.includes(table)) {
        return {
          isError: true,
          content: [{ type: "text", text: `La tabla "${table}" no existe en ${process.env.DB_NAME}.` }],
        };
      }
      const [rows] = await conn.query(`DESCRIBE \`${table}\``);
      return { content: [{ type: "text", text: JSON.stringify(rows, null, 2) }] };
    } catch (err) {
      return { isError: true, content: [{ type: "text", text: errorText(err) }] };
    }
  }
);

async function runStdio() {
  const transport = new StdioServerTransport();
  await server.connect(transport);
}

async function runHttp() {
  if (!AUTH_TOKEN) {
    console.error(
      "[mcp-artemio-db] MCP_TRANSPORT=http requiere definir MCP_AUTH_TOKEN en el .env (un token largo y secreto)."
    );
    process.exit(1);
  }

  const transport = new StreamableHTTPServerTransport({
    sessionIdGenerator: undefined, // modo stateless: no requiere sesión pegajosa entre requests
  });
  await server.connect(transport);

  const httpServer = http.createServer(async (req, res) => {
    if (req.url === "/health") {
      res.writeHead(200, { "content-type": "text/plain" });
      res.end("ok");
      return;
    }

    if (req.url !== "/mcp") {
      res.writeHead(404).end();
      return;
    }

    const authHeader = req.headers["authorization"] || "";
    const expected = `Bearer ${AUTH_TOKEN}`;
    const ok =
      authHeader.length === expected.length &&
      crypto.timingSafeEqual(Buffer.from(authHeader), Buffer.from(expected));
    if (!ok) {
      res.writeHead(401, { "content-type": "application/json" }).end(
        JSON.stringify({ error: "unauthorized" })
      );
      return;
    }

    try {
      await transport.handleRequest(req, res);
    } catch (err) {
      console.error("[mcp-artemio-db] Error manejando request HTTP:", err);
      if (!res.headersSent) {
        res.writeHead(500).end();
      }
    }
  });

  httpServer.listen(PORT, () => {
    console.error(`[mcp-artemio-db] Escuchando HTTP en el puerto ${PORT} (endpoint: /mcp)`);
  });
}

async function main() {
  await testConnection();
  if (MCP_TRANSPORT === "http") {
    await runHttp();
  } else {
    await runStdio();
  }
}

main().catch((err) => {
  console.error("[mcp-artemio-db] Error fatal al iniciar el servidor:", err);
  process.exit(1);
});
