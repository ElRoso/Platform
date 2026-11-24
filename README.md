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
