# Roadmap MVP

Etapa 0 (AGENTS.md §28, puntos 17 y 18). Base: AGENTS.md §26. Plan de trabajo: `.claude/plans/mvp.md`.

## Etapas

| Etapa | Objetivo | Issues | Gate |
|---|---|---|---|
| 0 Discovery | Este `docs/` | #1 | Aprobación del dueño |
| 1 Fundaciones | Esqueleto que compila, testea y despliega local | #2 – #8 | CI verde en `mainline` |
| 2 Identidad | UC-01..05, roles | se crean al planificar | Suite Identity verde, controles de `SECURITY.md` de auth |
| 3 Perfiles | UC-06..12 | ídem | BR-020..042 cubiertas |
| 4 Discovery | UC-13..16, SEO | ídem | Set de consultas naturales con ≥ 80 % interpretado correcto; p95 búsqueda medido |
| 5 Contacto | UC-17..21 | ídem | BR-060..070 cubiertas, antiabuso probado |
| 6 Confianza | UC-22..24 | ídem | Flujo de moderación E2E |
| 7 Lanzamiento | ASVS L2, carga, WCAG, deploy, monitoreo | ídem | Definition of Done §25 completo |

Detalle de slices por etapa en `.claude/plans/mvp.md`. Cada slice: domain → application → infrastructure → API → frontend → tests.

## Orden dentro de las etapas

Profiles antes que Discovery porque la búsqueda necesita perfiles publicados. Geography y Catalog se cargan con seed al inicio de Etapa 3/4. Notifications se implementa al primer uso (verificación de email en Etapa 2).

## Riesgos

| # | Riesgo | Impacto | Mitigación |
|---|---|---|---|
| R1 | Interpretación de consulta natural sin IA insuficiente | Búsquedas sin resultados, abandono | Set de frases reales como test, diccionario de sinónimos editable por admin, FTS como fallback (BR-051) |
| R2 | Datos geográficos (localidades homónimas, centroides) | Resultados lejanos o erróneos | Ids oficiales Georef, desambiguar por provincia en la UI |
| R3 | Spam y abuso en plataforma gratuita | Profesionales se van | Verificación, límites, reportes, moderación humana |
| R4 | Dependencias que pasan a pago | Bloqueo legal (AGPL + OSS) | Revisar licencia por versión exacta, preferir fundaciones (.NET Foundation, Apache, MIT) |
| R5 | Sobrediseño del monolito modular | Lentitud para el MVP | Proyectos mínimos por módulo chico; vertical slices |
| R6 | Privacidad (Ley 25.326) | Legal y reputacional | Minimización por diseño, sin domicilio, retención definida, evaluación AAIP (D20) |
| R7 | Arranque en frío (pocos perfiles) | Búsquedas vacías al lanzar | Fuera de alcance técnico; señalar al dueño del producto. UI de "sin resultados" con sugerencias |
| R8 | Costos de infraestructura (email, storage, hosting) | Contradice gratuidad si no se sostiene | Decidir proveedores con tier gratuito o self-hosted (D9–D11) |
| R9 | Estado en memoria de proceso | Rompe al escalar a >1 réplica | Listado en `ARCHITECTURE.md` §Escalado |
| R10 | Etapa 1 depende de cambios en ElRoso.Commons (feed público, LICENSE, dependencias separadas) | #2–#5 bloqueados o con dependencias que Platform no usa | Requisitos C1–C6 de ADR-0010 resueltos antes de referenciar el paquete |

## Decisiones

### Tomadas (aprobadas con el plan)

| # | Decisión | ADR |
|---|---|---|
| D1 | .NET 10 LTS | — (se fija en Etapa 1) |
| D2 | Next.js estable vigente al iniciar Etapa 1 | — |
| D3 | Handlers propios, sin mediator | 0004 |
| D4 | AwesomeAssertions | 0007 |
| D5 | NSubstitute | 0007 |
| D6 | Schema por módulo, un `DbContext` por módulo | 0003 |
| D7 | Eventos in-process + outbox en Postgres, sin broker | 0005 |
| D8 | Sesión en cookie httpOnly vía BFF Next.js | 0006 |
| D12 | Georef importado como seed | — |
| D13 | Valoraciones fuera del MVP | — |
| D15 | Visitante usa canales públicos; solicitud interna requiere cuenta verificada | — |
| D17 | Issues de Etapas 0 y 1 creados; el resto por etapa | — |
| D18 | AGPL-3.0-only, solo dependencias compatibles | 0009 |
| D23 | Backend usa ElRoso.Commons; lo que falte se cambia en Commons | 0010 |

### Pendientes

| # | Decisión | Opciones | Recomendación | Antes de |
|---|---|---|---|---|
| D9 | Proveedor de email en producción | SMTP self-hosted / proveedor con tier gratuito | Proveedor con tier gratuito suficiente para transaccionales | Etapa 2 |
| D10 | Storage de imágenes en producción | Volumen en el VPS / S3 compatible (MinIO self-hosted o proveedor) | MinIO self-hosted si el hosting es VPS | Etapa 3 |
| D11 | Hosting | VPS con compose / nube | VPS con compose para el MVP | Etapa 7 |
| D14 | Interacción elegible para valorar | Solicitud `Responded`/`Closed` | Esa | Cuando entre Reputation |
| D16 | Nombre del producto y dominio | — | — (del dueño) | Etapa 4 |
| D19 | Un perfil profesional por usuario o varios | Uno con varias especialidades / varios | Uno (BR-021): más simple y cubre multi-oficio con especialidades | Etapa 3 |
| D20 | Inscripción de la base ante la AAIP | Inscribir / evaluar con asesoría | Evaluar con asesoría legal | Etapa 7 |
| D21 | Hasher de contraseñas | Argon2id (librería OSS) / PBKDF2 de ASP.NET Identity | Argon2id | Etapa 2 |
| D22 | Generador de tipos TS desde OpenAPI | `openapi-typescript` (MIT) / otro | `openapi-typescript` | Etapa 1 |

### Reglas agregadas al modelar (aprobadas por el dueño)

No están en AGENTS.md; viven en `DOMAIN.md` y `API.md`:

| Regla | Qué agrega |
|---|---|
| BR-004, BR-024, BR-061 | Email verificado para publicar y para enviar solicitudes internas |
| BR-025 | Publicar exige al menos un canal de contacto |
| BR-027 | Perfil pausado muestra "no disponible temporalmente" en su URL |
| BR-063 | Una solicitud abierta por par solicitante–profesional |
| BR-064 | Límite diario de solicitudes (inicial 10) |
| BR-068 | Solicitudes sin actividad 30 días se cierran |
| BR-029 | Portfolio: máx 20 imágenes de 5 MB |
| `ProfessionalDescription` | 30–2000 caracteres, sin URLs ni teléfonos embebidos |
| API | `PUT /favorites/{id}` en vez de `POST /favorites` (idempotencia) |
