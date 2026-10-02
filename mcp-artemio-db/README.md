# mcp-artemio-db

Servidor MCP (Model Context Protocol) que conecta Claude a la base de datos
de producción MySQL de Artemio Navieras (`afuenzal_artemio`), para poder
consultar datos reales mientras se prueba el flujo de reserva en las
páginas de las navieras.

## ⚠️ Antes de usarlo — seguridad

Esta contraseña quedó escrita en texto plano en un chat. Aunque el archivo
`.env` no se sube a ningún repositorio, **se recomienda rotar la contraseña
del usuario `afuenzal_ti`** en cuanto sea posible, y actualizar el `.env`
con la nueva.

Otras recomendaciones:

- **Usa un usuario de solo lectura si puedes.** Pide que se cree un usuario
  MySQL aparte con permisos `SELECT` únicamente sobre `afuenzal_artemio`, y
  úsalo aquí en vez del usuario `afuenzal_ti` (que probablemente tiene más
  permisos de los necesarios).
- El servidor bloquea por defecto cualquier consulta de escritura
  (`INSERT`, `UPDATE`, `DELETE`, `DROP`, `ALTER`, `CREATE`, `TRUNCATE`,
  `REPLACE`, `GRANT`, `REVOKE`, `RENAME`, `LOCK`). Esto protege la base de
  producción de cambios accidentales hechos por Claude. Solo cambia
  `ALLOW_WRITE=true` en el `.env` si tienes una razón concreta para
  permitir escritura, y hazlo con cuidado.
- Nunca subas el archivo `.env` (con la contraseña real) a git. Ya está
  listado en `.gitignore`.

## Qué incluye

- `query`: ejecuta SQL contra la base (por defecto solo lectura), limitado
  a `MAX_ROWS` filas por respuesta.
- `list_tables`: lista las tablas de la base.
- `describe_table`: muestra columnas, tipos y claves de una tabla.

## Instalación

Requiere Node.js 18 o superior.

```bash
cd mcp-artemio-db
npm install
```

El archivo `.env` ya viene con las credenciales que diste. Revísalo y
ajústalo si es necesario (por ejemplo, para apuntar a un usuario de solo
lectura, o si falla la conexión SSL).

## Probarlo manualmente

```bash
node src/index.js
```

Si no muestra errores y queda "colgado" esperando entrada, es porque el
servidor MCP habla por stdio y está esperando que un cliente (como Claude)
se conecte — eso es normal. Ctrl+C para salir.

## Conectarlo a Claude Desktop

1. Abre el archivo de configuración de Claude Desktop:
   - macOS: `~/Library/Application Support/Claude/claude_desktop_config.json`
   - Windows: `%APPDATA%\Claude\claude_desktop_config.json`
2. Agrega (o crea) la sección `mcpServers` con esta entrada, usando la ruta
   absoluta real donde dejaste esta carpeta:

```json
{
  "mcpServers": {
    "artemio-mysql": {
      "command": "node",
      "args": ["/ruta/completa/a/mcp-artemio-db/src/index.js"]
    }
  }
}
```

3. Reinicia Claude Desktop. Deberías ver las herramientas `query`,
   `list_tables` y `describe_table` disponibles para el modelo.

## Conectarlo a Claude Code

Desde la carpeta del proyecto:

```bash
claude mcp add artemio-mysql -- node /ruta/completa/a/mcp-artemio-db/src/index.js
```

## Subirlo a un hosting con cPanel (para usarlo desde varios dispositivos)

Por defecto este servidor habla por **stdio**: solo sirve si Claude lo ejecuta
como proceso local en tu propia máquina. Para tenerlo corriendo 24/7 en tu
hosting y conectarte desde cualquier equipo, el servidor puede levantar un
modo **HTTP** (`MCP_TRANSPORT=http`), protegido con un token secreto
(`MCP_AUTH_TOKEN`).

### 1. Requisitos

- Tu cPanel debe tener la opción **"Setup Node.js App"** (Node.js Selector).
- Node.js 18 o superior disponible ahí.

### 2. Subir el código

Sube la carpeta `mcp-artemio-db` por Git o por el File Manager de cPanel
(comprimido en `.zip` y luego "Extract"). **No subas `node_modules`** (se
instala en el servidor) **ni el `.env`** con la contraseña real por git —
créalo directamente en el servidor con el File Manager o por SSH.

### 3. Crear la app en "Setup Node.js App"

1. cPanel → **Setup Node.js App** → **Create Application**.
2. Node.js version: 18 o superior.
3. Application mode: `Production`.
4. Application root: la ruta donde dejaste la carpeta, ej. `mcp-artemio-db`.
5. Application URL: un subdominio o path, ej. `mcp.artemio.cl` o
   `artemio.cl/mcp-artemio-db` (con SSL vía AutoSSL, recomendado).
6. Application startup file: `src/index.js`.
7. En **Environment variables**, agrega (o pon los mismos valores en el
   `.env` del servidor):
   - `MCP_TRANSPORT=http`
   - `MCP_AUTH_TOKEN=<token largo generado con crypto.randomBytes>`
   - `DB_HOST`, `DB_PORT`, `DB_NAME`, `DB_USER`, `DB_PASSWORD`, `DB_SSL`,
     `ALLOW_WRITE`, `MAX_ROWS` (los mismos que en `.env.example`).

   > No definas `PORT` a mano: cPanel/Passenger inyecta su propio puerto y
   > el servidor lo respeta automáticamente.

8. Guarda y pulsa **Run NPM Install** (esto instala las dependencias usando
   el entorno virtual de Node que crea cPanel).
9. Pulsa **Restart**.

### 4. Probarlo

```bash
curl https://mcp.artemio.cl/health
# -> ok
```

### 5. Conectar Claude a la URL remota

En vez de `command`/`args`, usa el transporte HTTP apuntando a
`https://mcp.artemio.cl/mcp` con el header de autorización.

**Claude Code:**

```bash
claude mcp add --transport http artemio-mysql https://mcp.artemio.cl/mcp \
  --header "Authorization: Bearer <tu-token>"
```

**Claude Desktop** (`claude_desktop_config.json`):

```json
{
  "mcpServers": {
    "artemio-mysql": {
      "type": "http",
      "url": "https://mcp.artemio.cl/mcp",
      "headers": { "Authorization": "Bearer <tu-token>" }
    }
  }
}
```

Repite esto en cada dispositivo desde el que quieras usarlo — todos pegan
al mismo servidor y a la misma base de datos.

### Notas de seguridad para el modo HTTP

- El endpoint `/mcp` exige el header `Authorization: Bearer <MCP_AUTH_TOKEN>`;
  sin él responde `401`. Guarda ese token como cualquier contraseña.
- Las reglas que bloquean escritura (`ALLOW_WRITE=false`) siguen aplicando
  igual en HTTP que en stdio.
- Usa siempre HTTPS (AutoSSL de cPanel es gratis) — nunca mandes el token
  por HTTP plano.
- Si en algún momento sospechas que el token se filtró, genera uno nuevo
  (mismo comando de `crypto.randomBytes`) y actualízalo en el `.env` del
  servidor y en cada cliente.

## Variables de entorno (`.env`)

| Variable      | Descripción                                                        |
|---------------|---------------------------------------------------------------------|
| `DB_HOST`     | Host de la base de datos (`artemio.cl`)                            |
| `DB_PORT`     | Puerto (`3306`)                                                    |
| `DB_NAME`     | Nombre de la base (`afuenzal_artemio`)                             |
| `DB_USER`     | Usuario de MySQL                                                    |
| `DB_PASSWORD` | Contraseña del usuario                                              |
| `DB_SSL`      | `true`/`false` — intenta SSL (equivalente a `SslMode=Preferred`)    |
| `ALLOW_WRITE` | `true`/`false` — permite consultas de escritura (por defecto `false`) |
| `MAX_ROWS`    | Máximo de filas devueltas por consulta (por defecto `500`)          |
| `MCP_TRANSPORT` | `stdio` (por defecto) o `http` — ver [sección de cPanel](#subirlo-a-un-hosting-con-cpanel-para-usarlo-desde-varios-dispositivos) |
| `PORT`        | Puerto HTTP cuando `MCP_TRANSPORT=http` (cPanel suele inyectar el suyo) |
| `MCP_AUTH_TOKEN` | Token requerido como `Authorization: Bearer <token>` en modo `http` |

## Notas técnicas

- La cadena original incluía `SslMode=Preferred` y
  `Convert Zero Datetime=True`, típico de conectores .NET/ODBC. Este
  servidor usa `mysql2`, así que se aproximan así:
  - `Preferred` → se intenta SSL con verificación laxa (`DB_SSL=true`). Si
    la conexión falla, prueba con `DB_SSL=false`.
  - `Convert Zero Datetime=True` → se usa `dateStrings: true`, que evita
    que fechas `0000-00-00` rompan la consulta (se devuelven como texto en
    vez de intentar convertirlas a `Date`).
