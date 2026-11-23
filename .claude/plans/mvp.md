# Plan MVP — Platform

Estado: **borrador, esperando aprobación del dueño**.

## Lo que entiendo que buscás

Un plan único que lleve el repo vacío hasta el MVP de AGENTS.md §4, respetando el orden del roadmap (§26). Etapa 0 (Discovery) va detallada y se ejecuta primero: solo docs, sin código. Las etapas 1–7 quedan como backlog de vertical slices: cada una se vuelve a planificar en detalle con el agente `planner` cuando Discovery esté aprobado, porque dependen de lo que salga de ahí.

## Resumen

| Etapa | Entrega | Gate para pasar |
|---|---|---|
| 0 Discovery | `docs/` completo (§28, 18 puntos) + ADR iniciales | Aprobación explícita del dueño |
| 1 Fundaciones | Solución .NET, Next.js, Postgres+PostGIS en compose, CI, logging, health checks, tests de arquitectura | CI verde en `mainline` |
| 2 Identidad | Registro, login, verificación, recuperación, sesiones, roles | Suite Identity + seguridad verde |
| 3 Perfiles | UserProfile, ProfessionalProfile, Catalog, publicación, portfolio | Reglas de publicación §7.3 cubiertas |
| 4 Discovery | Geography, búsqueda, filtros, ranking, SEO | p95 búsqueda medido |
| 5 Contacto | WhatsApp, solicitudes, historial, favoritos | Antiabuso cubierto |
| 6 Confianza | Reportes, moderación, auditoría, (valoraciones: ver D13) | Flujos de moderador E2E |
| 7 Lanzamiento | Seguridad, performance, accesibilidad, E2E, deploy, monitoreo | Checklist §25 completo |

## Qué existe / qué falta

- Existe: `AGENTS.md`, `CLAUDE.md`, `.claude/` (rules, agents, lessons), árbol de carpetas vacío, git en `mainline` con remoto `ElRoso/Platform`, sin commits.
- Falta: todo lo demás. No hay README, ni código, ni CI, ni issues.

---

## Paso 0 — Commit inicial

`mainline` no tiene commits; sin eso no hay `origin/mainline` desde donde ramificar.

1. `.gitignore` (dotnet + node + `.env`), `.editorconfig` (C#: nullable, file-scoped namespaces; TS: 2 espacios) y `LICENSE` según D18.
2. Commit `chore: estructura base del repositorio y reglas de agentes` directo en `mainline`.
3. Push a `origin/mainline` (**requiere tu OK**: es el primer push al remoto).

Se demuestra con: `git log --oneline origin/mainline` muestra el commit.

## Etapa 0 — Discovery (detallada)

Rama `docs/<issue>-discovery`. Solo `.md` y diagramas Mermaid. Un PR al final.

Orden por dependencia: cada doc se arma sobre el anterior.

| # | Archivo | Cubre (§28) | Contenido clave |
|---|---|---|---|
| 1 | `docs/DISCOVERY.md` | 1, 2, 3, 4 | Diagnóstico del repo, visión funcional, actores con permisos (matriz actor × acción), casos de uso del MVP con actor, precondición, flujo, reglas |
| 2 | `docs/EVENT-STORMING.md` | 5 | Eventos de dominio por flujo (registro, publicación, búsqueda, contacto, moderación), comandos, políticas, read models. Mermaid |
| 3 | `docs/BOUNDED-CONTEXTS.md` | 6 | Context map de los 10 módulos: upstream/downstream, contratos públicos, eventos que publica/consume cada uno, qué datos NO comparte |
| 4 | `docs/DOMAIN.md` | 7, 8, 9 | Agregados y raíces por módulo, value objects (solo los que validan algo), máquinas de estado (`ProfessionalProfile`, `ContactRequest`, `ModerationCase`) con transiciones permitidas, reglas de negocio numeradas (`BR-xxx`) referenciables desde tests |
| 5 | `docs/ARCHITECTURE.md` | 10, 11 | Modular monolith: capas, regla de dependencias, comunicación entre módulos, estructura de proyectos `.csproj` por módulo, Host, BuildingBlocks, frontend por features. Diagrama C4 nivel 1 y 2 |
| 6 | `docs/DATABASE.md` | 12 | Un schema de Postgres por módulo, ERD por schema, índices (GIN FTS, GiST geo, trigram, únicos), checks, estrategia de migraciones por `DbContext` |
| 7 | `docs/API.md` | 13 | Endpoints `/api/v1` del MVP: request/response, códigos, Problem Details, paginación, autorización por endpoint, idempotencia |
| 8 | `docs/TESTING.md` | 14 | Pirámide, qué se testea en cada capa, Testcontainers Postgres, tests de arquitectura, casos prioritarios §17.4 mapeados a `BR-xxx`, gate de cobertura |
| 9 | `docs/SECURITY.md` | 15 | ASVS nivel objetivo, auth y sesiones, autorización por recurso, rate limiting, archivos, privacidad Ley 25.326, matriz de amenazas por flujo |
| 10 | `docs/UX.md` | 16 | Principios mobile first, tokens de diseño, navegación, wireframes en texto de las pantallas MVP (home, búsqueda, resultados, perfil, publicar, mis solicitudes, admin), estados loading/vacío/error, accesibilidad |
| 11 | `docs/ROADMAP.md` | 17, 18 | Etapas 1–7 con issues propuestos, riesgos, decisiones pendientes con recomendación |
| 12 | `docs/decisions/0001…` | — | ADR iniciales: modular monolith, Postgres+PostGIS, schema por módulo, sin mediator comercial, librerías de test, auth/sesión, búsqueda con FTS, formato de eventos |
| 13 | `README.md` | — | Qué es, estado (Discovery), dónde está cada doc. Sin comandos hasta Etapa 1 |

Por cada doc:
- Se demuestra con: cada punto de §28 que cubre tiene su sección; toda regla de negocio citada existe en AGENTS.md o está marcada como decisión pendiente; nada contradice AGENTS.md.
- Agente `docs` no aplica (es todo docs); `comments` no aplica. `tester`: verifica la checklist §28 completa y que los Mermaid parseen.

Cierre de Etapa 0: resumen ejecutivo en el PR y **espera de aprobación** antes de Etapa 1.

---

## Etapas 1–7 — backlog de slices

Cada slice = un issue = domain → application → infrastructure → API → frontend → tests. Se planifica en detalle con `planner` al llegar.

### Etapa 1 — Fundaciones
1. Solución .NET con Host, BuildingBlocks y un módulo vacío de referencia. Test de arquitectura (NetArchTest) rojo → verde con las reglas §18.
2. `docker-compose.yml`: Postgres + PostGIS, Mailpit (email local), volúmenes, health checks.
3. Persistencia base: `DbContext` por módulo con schema propio, `NoTracking` global, convención UTC, migración inicial.
4. Pipeline HTTP: Problem Details, Serilog estructurado, correlation id, health checks, OpenTelemetry, CORS, headers de seguridad, rate limiter base.
5. Next.js App Router + TS strict + Tailwind + shadcn/ui, tokens de diseño, layout mobile con navegación inferior, Vitest + RTL + Playwright con un test de humo.
6. GitHub Actions: formato, análisis, tests back/front, integración con Testcontainers, build, verificación de migraciones, gate de cobertura.
7. `CLAUDE.md` sección "Comandos" y README con setup.

### Etapa 2 — Identidad
1. Registro con email (VO `EmailAddress`, duplicado, política de contraseña, sin enumeración).
2. Verificación de email.
3. Login, sesiones, refresh rotation con detección de reuso, logout, revocación.
4. Recuperación de contraseña.
5. Roles y policies (usuario, moderador, admin), ownership validation base.
6. Frontend: registro, login, recuperación, sesión en cookie httpOnly.

### Etapa 3 — Perfiles
1. Catalog: categorías jerárquicas, profesiones, especialidades, seed inicial, ABM admin.
2. UserProfile: crear y editar.
3. ProfessionalProfile: crear borrador, editar, especialidades, modalidad, zona de cobertura.
4. Publicar / pausar / reactivar con reglas §7.3 y estado `Suspended`.
5. Portfolio e imagen de perfil: subida segura (§15.4), almacenamiento.
6. Preferencias de contacto público (qué datos se muestran).
7. Frontend: flujo "Publicar" progresivo con borrador, perfil público.

### Etapa 4 — Discovery
1. Geography: provincias y localidades de Argentina con centroides, `ServiceArea`.
2. Read model de búsqueda alimentado por eventos de Profiles/Catalog/Geography.
3. Búsqueda textual (FTS español + unaccent + trigram), por categoría, por localidad, por radio.
4. Parser de consulta natural sin IA: diccionario de sinónimos → servicio, categoría, ubicación, modalidad.
5. Filtros, ordenamiento, paginación, ranking orgánico documentado.
6. SEO: rutas de categoría/localidad/perfil, metadata, OG, sitemap, robots, canonical, JSON-LD.
7. Frontend: home, búsqueda, resultados, detalle.

### Etapa 5 — Contacto
1. Botón WhatsApp (`wa.me`) y teléfono/email según preferencias del profesional.
2. `ContactRequest` con estados §13.3, idempotencia, duplicados, perfil pausado.
3. Mis solicitudes (usuario y profesional), historial.
4. Antiabuso: rate limit por usuario, límites diarios, bloqueo.
5. Favoritos (único por usuario-profesional).
6. Notificación por email de solicitud nueva.

### Etapa 6 — Confianza
1. Reportes de perfil, imagen y contenido.
2. Casos de moderación, acciones, ocultar/suspender perfil, motivos.
3. Auditoría de acciones sensibles.
4. Panel admin: usuarios, categorías, moderadores, parámetros.
5. Valoraciones (si D13 las deja en MVP): elegibilidad, sin autovaloración, sin duplicados, "sin valoraciones" ≠ 0.

### Etapa 7 — Lanzamiento
1. Revisión ASVS y OWASP Top 10, privacidad (exportación y borrado de cuenta).
2. Prueba de carga contra objetivos §20 y Lighthouse móvil.
3. Auditoría WCAG 2.2 AA.
4. E2E de flujos principales.
5. Dockerfiles de producción, deploy, secretos, backups.
6. Monitoreo y alertas.

---

## Riesgos

- **Búsqueda natural sin IA** (§12.1 vs IA fuera de MVP): un parser por diccionario puede quedarse corto. Mitigación: medir con un set de consultas reales antes de decidir algo más.
- **Datos geográficos**: calidad de centroides y nombres duplicados de localidades entre provincias. Mitigación: clave por id oficial, no por nombre.
- **Licencias**: el proyecto es open source. Toda dependencia nueva se revisa: licencia OSI compatible con la del proyecto (D18) y sin pasar a pago en versiones nuevas (ya pasó con MediatR y FluentAssertions 8).
- **Spam y abuso** en contacto gratuito sin barrera económica. Mitigación: rate limit y verificación de email antes de contactar.
- **Privacidad**: zona de cobertura sin exponer domicilio; la ubicación exacta no se guarda si no hace falta.
- **Estructura de carpetas ya creada** antes de Discovery: puede moverse según `ARCHITECTURE.md`.

## Decisiones pendientes

Cada una va como ADR o como sección del doc que corresponda. Recomendación entre corchetes.

| # | Decisión | Opciones | Recomendación |
|---|---|---|---|
| D1 | Versión .NET | .NET 10 LTS / otra | [.NET 10 LTS] |
| D2 | Versión Next.js | estable vigente / otra | [estable vigente al iniciar Etapa 1, verificada] |
| D3 | Mediator | **Decidido:** handlers propios con interfaz mínima. Nada de pago | — |
| D4 | Assertions | **Decidido:** AwesomeAssertions (fork Apache 2.0 de FluentAssertions). Nada de pago | — |
| D5 | Mocks | NSubstitute / Moq | [NSubstitute] |
| D6 | Persistencia por módulo | schema por módulo en una BD / tablas con prefijo | [schema por módulo, un `DbContext` por módulo] |
| D7 | Eventos entre módulos | dispatcher in-process / outbox en Postgres / broker | [in-process + outbox en Postgres; sin broker en MVP] |
| D8 | Sesión web | JWT bearer desde el browser / cookie httpOnly vía Next.js (BFF) | [cookie httpOnly; nada en localStorage] |
| D9 | Proveedor de email | a definir (costo) | [Mailpit en dev; prod decidir en Etapa 2] |
| D10 | Storage de imágenes | volumen local / S3 compatible (costo) | [volumen en dev; S3 compatible en prod, decidir en Etapa 3] |
| D11 | Hosting | VPS con compose / AWS / otro (costo) | [decidir en Etapa 7; diseño compatible con ambos] |
| D12 | Fuente geográfica AR | API Georef (datos.gob.ar) importada como seed / otra | [Georef importado, sin dependencia en runtime] |
| D13 | Valoraciones en MVP | MVP §4 no las lista, roadmap §26 sí (Etapa 6) | [dejarlas fuera del MVP; preparar el modelo] |
| D14 | Interacción elegible para valorar | `ContactRequest` respondida / otra | [ContactRequest en `Responded` o `Closed`] |
| D15 | Contacto de visitantes | solo WhatsApp/teléfono públicos / también solicitud interna anónima | [visitante: solo canales públicos; solicitud interna requiere cuenta verificada] |
| D16 | Nombre del producto y dominio | — | [necesario antes de Etapa 4 por SEO] |
| D18 | Licencia del proyecto | **Decidido:** AGPL-3.0-only. Dependencias: solo licencias compatibles (MIT, BSD, Apache 2.0, LGPL, MPL 2.0, GPL/AGPL 3) | — |
| D17 | Issues en GitHub | crear uno por slice ahora / crear por etapa al llegar | [crear los de Etapa 0 y 1 ahora; el resto al planificar cada etapa] |

## Ejecución

- Paso 0 y Etapa 0: en esta sesión, en orden, un doc por vez. Sin subagentes en paralelo: los docs dependen uno del otro.
- Etapa 1 en adelante: `planner` → aprobación → `executor` → `comments` → `docs` → `tester` → `pr`, una etapa por vez, backend antes que frontend dentro de cada slice.

## Rama y PR

- Paso 0: commit directo en `mainline` (repo sin commits).
- Etapa 0: `docs/<issue>-discovery` desde `origin/mainline` · un PR, un commit por doc o grupo de docs.
