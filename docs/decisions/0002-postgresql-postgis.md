# ADR-0002: PostgreSQL + PostGIS como única base

Estado: aceptada · 2026-11-23

**Contexto.** Búsqueda textual en español, búsqueda por distancia, datos relacionales con integridad fuerte, proyecto open source sin costos de licencia.

**Problema.** Elegir almacenamiento que cubra relacional, texto y geo sin sumar infraestructura.

**Alternativas.** SQL Server · PostgreSQL + Elasticsearch · PostgreSQL + PostGIS + FTS nativo.

**Decisión.** PostgreSQL con PostGIS, `unaccent`, `pg_trgm` y `citext`. Sin motor de búsqueda externo en el MVP (AGENTS.md §8.4).

**Justificación.** Licencia libre, FTS con diccionario español, índices GIN/GiST, `ST_DWithin` para radios. Una sola pieza que operar.

**Consecuencias.** Relevancia más limitada que un motor dedicado; si las métricas de búsqueda no alcanzan, se reevalúa con evidencia. Las reglas globales del dueño que nombran SQL Server no aplican a este proyecto.
