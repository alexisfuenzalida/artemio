# Artemio

Sistema de gestión de reservas, contratos e itinerarios. Tiene tres partes:

| Carpeta | Qué es |
| --- | --- |
| `MinimalAPIArtemio` | API en ASP.NET Core 8. Habla con MySQL. |
| `ArtemioBlazorWA` | Cliente Blazor WebAssembly. |
| `mcp-artemio-db` | Servidor MCP opcional para consultar la base desde un cliente MCP. |
| `Artemio.Contracts` | Modelos compartidos entre la API y la web. |

## Requisitos

- [.NET SDK 8](https://dotnet.microsoft.com/download/dotnet/8.0)
- MySQL accesible con el esquema que ya usa la aplicación (incluye la tabla `Users`)
- Node.js 18 o superior, solo si vas a usar `mcp-artemio-db`

## 1. Configurar la API

La API no arranca sin la cadena `ConnectionStrings:Default`. Esos archivos no están en git porque traen credenciales.

Desde la raíz del repositorio:

```powershell
Copy-Item MinimalAPIArtemio\MinimalAPIArtemio\appsettings.example.json MinimalAPIArtemio\MinimalAPIArtemio\appsettings.json
Copy-Item MinimalAPIArtemio\MinimalAPIArtemio\appsettings.Development.example.json MinimalAPIArtemio\MinimalAPIArtemio\appsettings.Development.json
```

Edita ambos y reemplaza `CHANGE_ME` por el servidor, usuario y contraseña de MySQL. `appsettings.Development.json` se aplica al correr en el perfil de desarrollo y pisa los valores del archivo base.

`Jwt:Key` debe tener al menos 32 caracteres. `Graph` solo hace falta para el envío de correos; el resto de la API funciona sin ese secreto.

El login lee `Users` y comprueba la contraseña con BCrypt. No hay un usuario de prueba en el repositorio: usa una cuenta que ya exista en esa base.

## 2. Levantar la API

La web llama a `https://localhost:7210/api/`. Hay que usar el perfil `https`, que publica ese puerto.

```powershell
dotnet run --project MinimalAPIArtemio\MinimalAPIArtemio\MinimalAPIArtemio.csproj --launch-profile https
```

Queda disponible en:

- API: `https://localhost:7210`
- Swagger: `https://localhost:7210/swagger`
- Salud: `https://localhost:7210/health`

En desarrollo, CORS acepta cualquier origen de `localhost`.

## 3. Levantar la web

En otra terminal:

```powershell
dotnet run --project ArtemioBlazorWA\ArtemioBlazorWA\ArtemioBlazorWA.csproj --launch-profile https
```

Abre `https://localhost:7005`.

`Program.cs` de la web fija la base de la API en `https://localhost:7210/api/`. El valor `ApiBaseUrl` de `wwwroot/appsettings.json` no se usa mientras esa línea siga así. Si la API no está en ese puerto, el login y el resto de las pantallas no van a responder.

## Pruebas

```powershell
dotnet test MinimalAPIArtemio\MinimalAPIArtemio.sln
dotnet test ArtemioBlazorWA\ArtemioBlazorWA.sln
```

## MCP (opcional)

Sirve para consultar MySQL desde un cliente MCP. No hace falta para usar la web.

```powershell
cd mcp-artemio-db
Copy-Item .env.example .env
npm install
node src/index.js
```

Completa `DB_PASSWORD` en `.env` antes de ejecutarlo. El detalle de herramientas, Claude y el modo HTTP está en [mcp-artemio-db/README.md](mcp-artemio-db/README.md).

No subas `appsettings.json`, `appsettings.Development.json`, `appsettings.Production.json` ni `.env`. Esos archivos están en `.gitignore`.
