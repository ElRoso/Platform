# Comentarios y docs nuevas

- Comentario nuevo: una línea, en español, solo qué hace. Dos solo si el qué no entra en una.
- Prohibido: por qué, historia del bug, alternativas, links, fechas, números de issue o PR. Eso va al commit o al PR.
- No repite el código. Si el código ya lo dice, sin comentario.
- `/// <summary>` de una línea: qué hace, parámetros y retorno si no son obvios.
- Excepción única: restricción externa no evidente que rompe si se toca (bug de librería, límite de API, orden obligatorio). Una línea seca.
- Lo que se agrega a `CLAUDE.md`, `AGENTS.md`, compose o `.env.example`: la regla y la trampa, en presente y sin fecha. Nunca un conteo de tests ni mediciones fechadas: envejecen.
