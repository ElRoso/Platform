# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Fuente de verdad

`AGENTS.md` es el documento maestro (producto, arquitectura, dominio, TDD, seguridad, roadmap). Compartido con otros agentes (Codex). Cambios de reglas van ahí, no acá.

@AGENTS.md

## Estado actual del repo

- Base backend del #2 implementada y compilable (solución .NET 10, Host, cuatro BuildingBlocks, esqueleto del módulo Identity, tests de arquitectura). Sin persistencia, sin endpoints de negocio, sin frontend.
- Fase: **Etapa 1 – Fundaciones** (AGENTS.md §26). Discovery aprobado: `docs/` es la referencia de dominio, arquitectura, API y seguridad; reglas `BR-xxx` en `docs/DOMAIN.md`, decisiones en `docs/ROADMAP.md` y `docs/decisions/`. Issues #3–#8 consumen esta base; cada uno se planifica con `planner` antes de ejecutar.

## Comandos

```bash
# Restaurar y compilar
dotnet restore backend/Platform.sln
dotnet build backend/Platform.sln -c Release

# Formato (debe pasar sin cambios)
dotnet format backend/Platform.sln --verify-no-changes

# Tests de arquitectura
dotnet test backend/tests/ArchitectureTests/Platform.ArchitectureTests/Platform.ArchitectureTests.csproj

# Arranque del Host (http://localhost:5211)
dotnet run --project backend/src/Host/Platform.Host/Platform.Host.csproj
# chequeos: curl http://localhost:5211/   y   curl http://localhost:5211/health
```

El agente `tester` toma los comandos de acá y los replica del CI.
## Reglas, agentes y memoria

- `.claude/rules/`: `workflow.md`, `code.md`, `comments.md`, `git.md`, `testing.md`, `security.md`; `backend.md` y `frontend.md` se cargan solo bajo `backend/**` y `frontend/**`.
- `.claude/agents/`: `planner` → (dueño aprueba) → `executor` → `comments` → `docs` → `tester` → `pr` → (merge) → `memory`.
- `.claude/plans/<issue>.md`: plan aprobado en curso. `.claude/memory/lessons.md`: correcciones del dueño, leer antes de arrancar.
- Tracker: GitHub Issues. Rama base `mainline`.

## Precedencia frente a reglas globales

Las reglas de `AGENTS.md` especializan las globales del usuario. Donde chocan, gana el proyecto:

| Tema | Global | Este proyecto |
|---|---|---|
| Base de datos | SQL Server | PostgreSQL + PostGIS, EF Core |
| CI/CD | Azure DevOps | GitHub Actions |
| Cobertura | mínimo 70% | sin porcentaje arbitrario; umbrales graduales sobre invariantes, casos críticos, autorización |
| Reporte final | respuesta mínima | formato fijo de AGENTS.md §27.4 |

Se mantienen de las reglas globales: identificadores en inglés, comentarios en español, sin estimaciones salvo pedido, sin atribución de IA en commits/PRs/código/docs.

## Restricciones que más se rompen sin querer

- Gratuidad total: no introducir planes, destacados, créditos ni rankings pagos (§2.3). Cambiarlo requiere decisión del dueño del producto.
- Una sola cuenta por persona; "profesional" es un perfil sobre el usuario, no otro tipo de cuenta (§3.3).
- Módulos no comparten entidades ni tablas; Discovery consume contratos públicos de Profiles/Catalog/Geography (§5.5, §6.5).
- Nunca exponer domicilio, email o teléfono privados del profesional sin consentimiento (§6.4, §13.2).
- Profesional sin valoraciones ≠ puntuación cero; no penalizar nuevos en ranking (§12.4, §14.3).
- Decisiones de §27.5 (arquitectura, tecnología, infraestructura, migraciones destructivas, costos externos) requieren aprobación previa.
