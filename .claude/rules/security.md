# Seguridad de identidad y sesiones

Complementa AGENTS.md §15. Trampas que ya costaron caro en otro proyecto.

- Refresh tokens: rotación con detección de reuso. Un token ya rotado que vuelve revoca toda la familia.
- Vida de la sesión se decide contra la tabla de sesiones, no contra "hay un refresh token activo": hay carrera entre rotación y validación.
- Apertura y rotación de sesión atómicas: `SELECT ... FOR UPDATE` dentro de la transacción.
- Sello de cambio de contraseña (`PasswordChangedAt`, security stamp): lo pone solo el caso de uso de cambio de contraseña, nunca los callers.
- Update genérico de usuario excluye hash y security stamp: una edición de perfil no puede pisarlos.
- Segundo factor u otro control de seguridad con config faltante: falla cerrado.
- Al usuario no se lo borra para cortar acceso: se lo bloquea. Borrado de cuenta (Ley 25.326) es un flujo aparte con anonimización.
- Tokens nunca en query string: terminan en logs de proxy y del server.
- Respuestas de login, registro y recuperación no revelan si el email existe.
