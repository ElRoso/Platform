# Forma de trabajo (sesión principal y agentes)

Complementa el protocolo de AGENTS.md §1.4 y §27.

## Antes de arrancar
- Tarea larga: decir en 2-3 frases qué se entiende que busca el dueño y esperar el sí.
- Leer `.claude/memory/lessons.md`: cada línea es una corrección ya hecha.
- Mapear el código con agentes de exploración antes de tocar. No saltear análisis → plan → implementación → verificación.
- Tracker: GitHub Issues. El plan aprobado vive en `.claude/plans/<issue>.md` (sin issue todavía: `.claude/plans/<slug>.md`): pasos, cómo se demuestra cada uno y estado. Se actualiza al avanzar y se deja al pausar, para que otra sesión retome.
- Dos intentos fallidos en un mismo paso: frenar, anotar en el plan qué falló y volver a planificar.

## Alcance
- Ante un compromiso, pesar UX (usuarios), DX (el próximo desarrollador) y AX (el próximo agente), y decir cuál ganó.
- Sin dependencias nuevas, renombres ni refactors que nadie pidió.
- Antes de borrar o sobrescribir algo fuera de git (`.env`, bases, volúmenes): copia de respaldo y decir dónde quedó.

## Bugs
- Reproducirlo primero con un test que replique los pasos del dueño. Si no falla, el bug no está reproducido: decir qué falta.
- Arreglar la causa y volver a correr los mismos pasos.
- Nunca silenciar un error: ni `catch` vacío, ni test salteado, ni regla de lint apagada, ni `|| true`.

## Agentes
- Cadena: `planner` → (dueño aprueba) → `executor` → `comments` → `docs` → `tester` → `pr` → (merge) → `memory`.
- Roles: `planner` y `tester` solo leen y reportan; `comments` y `docs` editan solo comentarios y docs; `executor` edita código; `pr` solo integra; `memory` solo toca `.claude/memory/`.
- Cada encargo lleva una tarea, una condición de fin y vuelve con un informe de 5 líneas arriba de todo.
- En paralelo sí; dos agentes sobre el mismo archivo, no.
- Pedido que cruza backend y frontend: primero backend hasta verde, después frontend armado con el contrato real. En paralelo la PC se satura y en cola termina antes.
- Tests: el `executor` corre solo los de lo nuevo, filtrados; la suite completa la corre el `tester` una vez, antes del PR.
- Un encargo dice qué verificar, nunca con qué comando: la tabla del `tester` manda y es la misma que corre el CI.
- Comprobar la afirmación clave de un informe de agente (el test verde, el chequeo limpio, el archivo tocado) corriéndola uno mismo antes de construir sobre ella.

## Cierre
- Una comprobación no ejecutada se dice como tal; no cuenta como verde.
- UI: abrirla e intentar romperla (entrada vacía, doble envío, recarga, viewport móvil). Si no se pudo abrir, va como no ejecutado.
- Cambio de backend que el dueño va a probar en local: reiniciar el server de dev antes de avisar (`dotnet run` no recarga).
- Informe final con el formato de AGENTS.md §27.4.

## Lessons
- Cuando el dueño corrige, se suma una línea arriba de todo en `.claude/memory/lessons.md`: "Cuando X, hacer Y".
- El mismo error dos veces significa que la lección no se entendió: reescribirla.
- Antes de cambiar una regla de `.claude/rules/`, `CLAUDE.md` o `AGENTS.md` a raíz de una corrección, preguntar.
