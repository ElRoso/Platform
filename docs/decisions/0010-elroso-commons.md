# ADR-0010: Usar ElRoso.Commons como base transversal del backend

Estado: aceptada · 2026-11-23

**Contexto.** ElRoso.Commons (`github.com/ElRoso/Commons`, net10.0, MIT) es la librería propia del dueño con piezas transversales ya usadas en Facturador: Problem Details, Serilog, health checks, forwarded headers, validación de JWT, usuario actual, `CustomRandom`. Se publica en nuget.org en paquetes por área (`ElRoso.Commons.Core`, `.AspNetCore`, `.Serilog`, `.EntityFrameworkCore`, etc.).

**Problema.** Etapa 1 (#2–#5) necesita esas mismas piezas. Escribirlas de cero en Platform duplica código que el dueño ya mantiene.

**Alternativas.** Dependencia a Commons · copiar ideas sin dependencia · partir Commons en paquetes chicos.

**Decisión.** Platform consume ElRoso.Commons. Lo que falte o no encaje se cambia en Commons, no se reescribe en Platform. Commons evoluciona para cumplir los requisitos de abajo.

**Requisitos sobre Commons para que Platform lo consuma.**

| # | Requisito | Por qué | Estado |
|---|---|---|---|
| C1 | Paquete en un feed público (nuget.org) | AGPL y open source: cualquiera compila sin credenciales; CI de GitHub sin secreto | Cumplido |
| C2 | Archivo `LICENSE` MIT en el repo | Compatibilidad con AGPL verificable (ADR-0009) | Cumplido |
| C3 | Dependencias con licencia OSI, sin pago | AGENTS.md §1.3 | Cumplido en Core, AspNetCore, Serilog y EntityFrameworkCore: MIT o Apache-2.0 |
| C4 | Paquetes por área: no arrastrar SQL Server, MongoDB, Azure, AWS, Telegram ni Windows Services | Platform usa PostgreSQL y no usa esos servicios | Cumplido |
| C5 | Bootstrap componible, sin pipeline fijo | Cada módulo registra sus servicios y endpoints (ADR-0001) | Cumplido: extensiones `AddX`/`UseX`/`MapX`; `UseElRoso` queda solo en el metapaquete |
| C6 | Persistencia sin atarse a SQL Server | ADR-0002 | Cumplido: `ElRoso.Commons.EntityFrameworkCore` sin proveedor |

Platform referencia los paquetes por área, nunca el metapaquete `ElRoso.Commons`.

**Choques a resolver al planificar cada issue.**

- Errores: Commons usa excepciones tipadas; `ARCHITECTURE.md` define handler → `Result` → Problem Details.
- Hasher: Commons trae BCrypt; D21 recomienda Argon2id.
- Serilog: `MinimumLevel.Debug` fijo en código.
- Health: `MapHealthChecksCustom` acepta la ruta; `/health/live` y `/health/ready` se mapean desde Platform.

**Consecuencias.** Menos código en BuildingBlocks y Host. Platform queda atado al ritmo de Commons: un cambio de Platform puede requerir antes una versión nueva de Commons. Toda dependencia que se agrega a Commons la hereda Platform con su licencia. Trampas conocidas de Commons en `.claude/rules/backend.md` cuando se integre.
