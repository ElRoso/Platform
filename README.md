# Platform

Plataforma gratuita y open source que conecta personas que saben hacer con personas que necesitan: profesionales y oficios con quienes buscan sus servicios. Mercado inicial: Argentina.

## Estado

**Etapa 1 — Fundaciones (en curso).** Base backend compilable del #2: solución .NET 10, Host, cuatro BuildingBlocks, esqueleto del módulo Identity y tests de arquitectura. Sin persistencia, sin endpoints de negocio, sin frontend todavía. Ver [docs/ROADMAP.md](docs/ROADMAP.md).

### Backend

```bash
dotnet build backend/Platform.sln -c Release
dotnet test backend/tests/ArchitectureTests/Platform.ArchitectureTests/Platform.ArchitectureTests.csproj
dotnet run --project backend/src/Host/Platform.Host/Platform.Host.csproj
```

El Host responde en `http://localhost:5211/` (info) y `http://localhost:5211/health`. Comandos completos en la sección "Comandos" de [CLAUDE.md](CLAUDE.md).

### Entorno local: PostgreSQL + Mailpit

Requiere Docker y Docker Compose. Solo desarrollo: no usar este compose en producción.

```bash
cp .env.example .env     # credenciales y puertos locales; .env no se versiona
docker compose up -d     # postgres + mailpit
docker compose ps        # ambos servicios deben quedar "healthy"
docker compose down      # baja los contenedores; los datos quedan en volúmenes
docker compose down -v   # destructivo: borra volúmenes, base y correos
```

| Servicio | Dirección | Uso |
|---|---|---|
| PostgreSQL 17 + PostGIS 3.7 | `127.0.0.1:5432` | BD `platform`, usuario `platform`, contraseña `change-me-local` |
| Mailpit SMTP | `127.0.0.1:1025` | SMTP local, sin TLS ni autenticación real |
| Mailpit UI | `http://127.0.0.1:8025` | Bandeja de correos |

Puertos y credenciales salen de `.env` (`POSTGRES_*`, `MAILPIT_*`). Si un puerto está ocupado, cambiarlo ahí y volver a levantar.

Las extensiones `postgis`, `unaccent`, `pg_trgm` y `citext` se crean al inicializar la base:

```bash
docker compose exec -T postgres psql -U platform -d platform -c "SELECT postgis_version();"
docker compose exec -T postgres psql -U platform -d platform -c "SELECT extname FROM pg_extension WHERE extname IN ('citext','pg_trgm','postgis','unaccent') ORDER BY extname;"
```

El script de inicialización corre solo con el volumen vacío: si se agrega una extensión o cambia el script, hay que recrear el volumen con `docker compose down -v` (borra los datos).

Correo de prueba por SMTP y lectura por API:

```bash
python3 - <<'PY'
import smtplib
from email.message import EmailMessage

msg = EmailMessage()
msg["From"] = "dev@platform.local"
msg["To"] = "dev@platform.local"
msg["Subject"] = "Prueba"
msg.set_content("Correo de prueba.")
with smtplib.SMTP("127.0.0.1", 1025, timeout=10) as s:
    s.send_message(msg)
PY

curl -fsS http://127.0.0.1:8025/api/v1/messages
```

Conexión desde .NET (Npgsql): `Host=localhost;Port=5432;Database=platform;Username=platform;Password=change-me-local`. SMTP de desarrollo: `Host=localhost;Port=1025`, sin TLS.

`.env` lo lee Compose, no `dotnet run`: para la aplicación exportar `ConnectionStrings__Default` en la shell o usar user-secrets.

## Documentación

| Documento | Contenido |
|---|---|
| [AGENTS.md](AGENTS.md) | Documento maestro: producto, arquitectura, reglas |
| [docs/DISCOVERY.md](docs/DISCOVERY.md) | Diagnóstico, visión, actores, casos de uso |
| [docs/EVENT-STORMING.md](docs/EVENT-STORMING.md) | Eventos, comandos y políticas por flujo |
| [docs/BOUNDED-CONTEXTS.md](docs/BOUNDED-CONTEXTS.md) | Mapa de contextos y contratos entre módulos |
| [docs/DOMAIN.md](docs/DOMAIN.md) | Agregados, value objects, estados, reglas de negocio |
| [docs/ARCHITECTURE.md](docs/ARCHITECTURE.md) | Arquitectura y estructura de proyectos |
| [docs/DATABASE.md](docs/DATABASE.md) | Modelo de datos, índices, migraciones |
| [docs/API.md](docs/API.md) | Contratos REST iniciales |
| [docs/TESTING.md](docs/TESTING.md) | Estrategia TDD |
| [docs/SECURITY.md](docs/SECURITY.md) | Seguridad y privacidad |
| [docs/UX.md](docs/UX.md) | Diseño mobile first y SEO |
| [docs/ROADMAP.md](docs/ROADMAP.md) | Etapas, riesgos, decisiones |
| [docs/decisions/](docs/decisions/) | ADRs |

## Stack

ASP.NET Core (.NET 10) · Next.js + TypeScript + Tailwind · PostgreSQL + PostGIS · Docker.

## Licencia

[AGPL-3.0-only](LICENSE).
