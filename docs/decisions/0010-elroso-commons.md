# ADR-0010: Usar ElRoso.Commons como base transversal del backend

Estado: aceptada · 2026-11-23

**Contexto.** ElRoso.Commons (`Commons/Backend`, net10.0, MIT en el csproj) es la librería propia del dueño que ya resuelve piezas transversales en Facturador: Problem Details, Serilog, health checks, forwarded headers, validación de JWT, usuario actual, BCrypt, `CustomRandom`. Hoy está pensada para SQL Server y MongoDB, usa capas clásicas con controllers y se publica en un feed privado de Azure Artifacts.

**Problema.** Etapa 1 (#2–#5) necesita esas mismas piezas. Escribirlas de cero en Platform duplica código que el dueño ya mantiene.

**Alternativas.** Dependencia a Commons · copiar ideas sin dependencia · partir Commons en paquetes chicos.

**Decisión.** Platform consume ElRoso.Commons. Lo que falte o no encaje se cambia en Commons, no se reescribe en Platform. Commons evoluciona para cumplir los requisitos de abajo.

**Requisitos sobre Commons para que Platform lo consuma.**

| # | Requisito | Por qué | Estado hoy |
|---|---|---|---|
| C1 | Paquete en un feed público (nuget.org) | AGPL y open source: cualquiera compila sin credenciales; CI de GitHub sin secreto | Feed privado con PAT |
| C2 | Archivo `LICENSE` MIT en el repo | Compatibilidad con AGPL verificable (ADR-0009) | Solo `PackageLicenseExpression` |
| C3 | Dependencias con licencia OSI, sin pago | AGENTS.md §1.3 | A verificar por versión exacta |
| C4 | Sin arrastrar SQL Server, MongoDB, Azure AD, Graph, SharePoint, S3, Telegram ni Windows Services a quien no los usa | Platform usa PostgreSQL y no usa esos servicios | Una sola dll con todo |
| C5 | Bootstrap componible, sin pipeline fijo | Cada módulo registra sus servicios y endpoints (ADR-0001) | `UseElRoso` con orden fijo |
| C6 | Soporte PostgreSQL o persistencia fuera de Commons | ADR-0002 | Repositorio base solo SQL Server |

**Choques a resolver al planificar cada issue.**

- Errores: Commons usa excepciones tipadas; `ARCHITECTURE.md` define handler → `Result` → Problem Details.
- Hasher: Commons trae BCrypt; D21 recomienda Argon2id.
- Health: Commons expone `/healthz`; `ARCHITECTURE.md` define `/health/live` y `/health/ready`.
- Serilog: Commons fija `MinimumLevel.Debug` sin `ReadFrom.Configuration`.
- JSON y OpenAPI: Commons usa Newtonsoft y Swashbuckle; D22 genera tipos TS desde OpenAPI.

**Consecuencias.** Menos código en BuildingBlocks y Host. Platform queda atado al ritmo de Commons: un cambio de Platform puede requerir antes una versión nueva de Commons. Toda dependencia que se agrega a Commons la hereda Platform con su licencia. Trampas conocidas de Commons en `.claude/rules/backend.md` cuando se integre.
