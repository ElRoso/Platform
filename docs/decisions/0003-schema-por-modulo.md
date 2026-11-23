# ADR-0003: Un schema y un DbContext por módulo

Estado: aceptada · 2026-11-23

**Contexto.** AGENTS.md §5.5 prohíbe acceder a tablas de otros módulos.

**Problema.** Hacer cumplir el límite a nivel datos, no solo de código.

**Alternativas.** Un `DbContext` compartido con prefijos de tabla · una BD por módulo · un schema por módulo en una BD.

**Decisión.** Un schema de Postgres y un `DbContext` por módulo, cada uno con su historial de migraciones. Sin FK entre schemas.

**Justificación.** Límite visible y verificable, migraciones independientes, una sola instancia que operar. Permite separar BD más adelante.

**Consecuencias.** Sin joins entre módulos: las vistas combinadas se construyen con read models (Discovery) alimentados por eventos. Consistencia eventual entre módulos.
