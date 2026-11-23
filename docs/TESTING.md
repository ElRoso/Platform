# Estrategia de testing

Etapa 0 (AGENTS.md §28, punto 14). Base: AGENTS.md §17, §18; `.claude/rules/testing.md`. ADR-0007.

## Ciclo

TDD obligatorio: RED (test que falla por el motivo esperado, salida mostrada) → GREEN (mínimo) → REFACTOR. Bug = test que lo reproduce primero.

## Herramientas (todas OSS, sin pago)

| Capa | Herramienta | Licencia |
|---|---|---|
| Runner .NET | xUnit v3 | Apache 2.0 |
| Assertions | AwesomeAssertions | Apache 2.0 |
| Dobles | NSubstitute | BSD-3 |
| BD real | Testcontainers (`postgis/postgis`) | MIT |
| HTTP | `WebApplicationFactory` | MIT |
| Arquitectura | NetArchTest.eNhancedEdition | MIT |
| Cobertura | coverlet + ReportGenerator | MIT / Apache 2.0 |
| Front unit/componentes | Vitest + React Testing Library | MIT |
| E2E | Playwright | Apache 2.0 |
| Accesibilidad | `@axe-core/playwright` | MPL 2.0 |

Versión exacta y licencia se revisan al agregar cada paquete.

## Pirámide

| Tipo | Qué cubre | Dónde | Dobles |
|---|---|---|---|
| Unit de dominio | Invariantes, VOs, máquinas de estado, `BR-xxx` | `UnitTests/<Module>` | Ninguno |
| Unit de aplicación | Orquestación de handlers, validación, autorización por recurso | `UnitTests/<Module>` | Puertos con NSubstitute |
| Integración | Repositorios, consultas, índices, migraciones, outbox, búsqueda FTS/geo | `IntegrationTests/<Module>` | Postgres real |
| Funcional / API | Endpoint completo: auth, status, Problem Details, paginación | `FunctionalTests` | Postgres real, email fake |
| Arquitectura | Capas y límites de módulo | `ArchitectureTests` | — |
| Front unit/componente | Hooks, schemas Zod, componentes con estados loading/vacío/error | `frontend/tests/unit`, `integration` | MSW para API |
| E2E | Flujos principales en viewport móvil y desktop | `frontend/tests/e2e` | Stack completo en compose |

## Tests de arquitectura

- `*.Domain` no depende de `Infrastructure`, `Api`, EF Core ni ASP.NET.
- `*.Application` no depende de `Api` ni de `Infrastructure`.
- `*.Api` no depende de `Infrastructure` ni usa `DbContext`.
- Ningún módulo referencia ensamblados internos de otro (solo `*.Contracts`).
- Entidades de dominio sin setters públicos.
- Todo `AbstractValidator<T>` apunta a un tipo de request usado.
- Endpoints no contienen lógica de negocio: solo llaman a un handler (regla por convención: un endpoint = un handler).

## Casos prioritarios (AGENTS.md §17.4 → reglas)

| Área | Caso | Regla | Tipo |
|---|---|---|---|
| Registro | Email válido / inválido / duplicado sin enumeración / contraseña inválida | BR-001..003 | Unit + Funcional |
| Sesión | Rotación, reuso detectado revoca familia, sesión revocada rechazada | BR-007, BR-008 | Integración |
| Perfil | Crear, publicar válido, publicar incompleto (un test por requisito), pausar, reactivar | BR-023..027 | Unit |
| Perfil | Edición por no dueño → 403; contraparte positiva del dueño | — | Funcional |
| Perfil | Suspendido no publica | BR-028 | Unit |
| Búsqueda | Por categoría, por localidad, por radio, sin resultados con sugerencias, filtros combinados, paginación, solo `Published` | BR-050..056 | Integración |
| Búsqueda | Consulta natural: set fijo de frases → interpretación esperada | BR-050 | Unit |
| Contacto | Válida, destinatario inexistente, pausado, a uno mismo, duplicado idempotente, límite diario | BR-061..065 | Unit + Funcional |
| Favoritos | Duplicado idempotente | BR-070 | Integración |
| Moderación | Agrupa reportes, suspende y saca de búsqueda, moderador no modera lo propio | BR-080..084 | Unit + Integración |

## Datos de test

- Builders por agregado (`ProfessionalProfileBuilder.Published().InLocality(...)`); sin setup copiado.
- Fechas fijas con `FakeTimeProvider`; nunca `UtcNow`.
- Integración: una BD por corrida (`platform_it_<utc>_<guid8>`), borrada al final con guard de nombre; cada test siembra sus datos con ids nuevos.
- Datos geográficos de test: subconjunto fijo de provincias y localidades con coordenadas reales.

## Cobertura

- Gate sobre `*.Domain` y `*.Application` medido en CI con `line-rate` de `coverage.cobertura.xml`.
- Umbral inicial: el valor medido al cerrar Etapa 1; sube por etapa, nunca baja (AGENTS.md §17.5). Prioridad: invariantes, autorización, persistencia.
- Front: gate sobre `features/` y `lib/`.

## Ejecución

- Local: unit por módulo filtrado mientras se desarrolla; integración de a una suite (Testcontainers consume RAM).
- CI: todas las suites, en el orden de AGENTS.md §21.3.
- Comandos exactos: sección "Comandos" de `CLAUDE.md` (Etapa 1).
