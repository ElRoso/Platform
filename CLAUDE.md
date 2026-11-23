# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Fuente de verdad

`AGENTS.md` es el documento maestro (producto, arquitectura, dominio, TDD, seguridad, roadmap). Compartido con otros agentes (Codex). Cambios de reglas van ahí, no acá.

@AGENTS.md

## Estado actual del repo

- Git inicializado, rama `mainline`, remoto `github.com/ElRoso/Platform`. Árbol de carpetas de AGENTS.md §23 creado vacío (`.gitkeep`). Sin código ni `README.md`.
- Fase: **Etapa 0 – Discovery** (AGENTS.md §26 y §28). Entregables solo en `/docs`. No crear solución .NET, app Next.js, Docker ni instalar dependencias hasta aprobación explícita.

## Comandos

Ninguno todavía. Se definen en Etapa 1 y deben replicar el CI; el agente `tester` los toma de acá.

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
