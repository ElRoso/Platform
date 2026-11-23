# Seguridad y privacidad

Etapa 0 (AGENTS.md §28, punto 15). Base: AGENTS.md §15; `.claude/rules/security.md`.

## Objetivo

OWASP ASVS 5.0 **nivel 2** como referencia para el MVP (maneja datos personales y cuentas), más OWASP Top 10. Revisión completa en Etapa 7.

## Autenticación (Identity)

| Control | Decisión |
|---|---|
| Hash | Argon2id (parámetros OWASP vigentes) vía librería OSS; o PBKDF2 de ASP.NET Core Identity si se adopta su hasher. Se decide en Etapa 2 con ADR. |
| Contraseñas | ≥ 10 caracteres, sin reglas de composición, chequeo contra lista de filtradas (k-anonymity de HIBP o lista local). |
| Fuerza bruta | Bloqueo temporal por cuenta (BR-006) + rate limit por IP en `/auth/*`. |
| Sesión | Access token corto (10 min) + refresh rotativo con detección de reuso (BR-007). Sesión validada contra `user_session` (BR-008). Apertura y rotación con `SELECT … FOR UPDATE`. |
| Cookies (Web/BFF) | `HttpOnly`, `Secure`, `SameSite=Lax`, prefijo `__Host-`. Nada en `localStorage`. |
| CSRF | SameSite + verificación de `Origin` en mutaciones del BFF. |
| Verificación y recuperación | Tokens aleatorios de 256 bits, guardados como hash, un uso, vencimiento corto (24 h / 1 h). |
| Enumeración | Registro, login y recuperación responden igual exista o no el email. Tiempos de respuesta equiparados. |
| Cambio de contraseña | Revoca todas las sesiones; `password_changed_at` y `security_stamp` solo los toca ese caso de uso. |
| Usuarios | Se bloquean, no se borran; borrado de cuenta = anonimización. |

## Autorización

- Policies por rol (`Moderator`, `Admin`) y por requisito (`EmailVerified`).
- Ownership en el handler: el recurso se carga y se compara `OwnerId` con el usuario actual. Nunca confiar en un id del request para decidir propiedad.
- Claims leídos del principal en cada acceso.
- Admin no edita perfiles ajenos; solo moderación auditada.
- Cada test de autorización tiene su caso positivo.

## API

| Control | Decisión |
|---|---|
| Rate limiting | Particionado por usuario autenticado, si no por IP. Después de `UseAuthorization`. Forwarded headers con proxies conocidos. Límites específicos para auth, búsqueda y contacto. |
| Validación | FluentValidation en Application + invariantes en Domain. Tamaño máximo de body. |
| Inyección | Solo EF Core / parámetros; FTS con `websearch_to_tsquery`, nunca concatenando. |
| CORS | Solo el origen de la Web. La API no se expone a otros orígenes de browser. |
| Headers | HSTS, `X-Content-Type-Options`, `Referrer-Policy`, CSP en la Web (sin `unsafe-inline` en scripts), `Permissions-Policy`. |
| Errores | Problem Details sin stack ni detalles internos; `traceId` para correlacionar. |
| Secretos | Variables de entorno; gestor de secretos en producción; `.env.example` sin valores reales. |

## Archivos (portfolio, avatar)

1. Límite de tamaño en el servidor web y en el endpoint (5 MB).
2. Tipo real por magic bytes (JPEG, PNG, WebP); la extensión no cuenta.
3. Decodificar y re-encodear con librería OSS. ImageSharp tiene licencia dividida que depende del uso: preferir SkiaSharp o NetVips (MIT). Esto elimina EXIF y payloads.
4. Nombre aleatorio (UUID); nunca el nombre original en el path.
5. Servir desde dominio o path sin ejecución, con `Content-Disposition` y `Content-Type` fijos.
6. Límite de cantidad por perfil (BR-029).

## Antiabuso

- Solicitudes internas solo con email verificado; límite diario (BR-064); una abierta por par (BR-063).
- Rate limit en reportes.
- Bloqueo de usuario por admin corta acceso y suspende perfiles (BR-011).
- Descripción sin URLs ni teléfonos embebidos en el MVP (VO `ProfessionalDescription`) para frenar spam y evadir la elección de canales.

## Privacidad (Ley 25.326)

| Principio | Cómo se cumple |
|---|---|
| Minimización | Sin domicilio (BR-031); ubicación por localidad; IP solo hasheada en sesiones. |
| Consentimiento | Términos y política de privacidad aceptados al registrarse; publicación de teléfono/email es opt-in explícito por canal. |
| Acceso y exportación | `GET /me/export` (Etapa 7) con los datos del usuario en JSON. |
| Rectificación | Edición de perfil y cuenta. |
| Supresión | Borrado de cuenta = anonimización en Identity + borrado de perfil, portfolio, favoritos; solicitudes conservan texto anonimizado por el período de retención. |
| Retención | Mensajes de contacto 12 meses tras cierre; logs 30 días; parámetros configurables. |
| Seguridad | Este documento. |
| Registro de base | Evaluar inscripción ante la AAIP antes del lanzamiento (decisión pendiente D20). |

## Logs

Nunca: contraseñas, tokens, cookies, emails completos, teléfonos, cuerpo de mensajes, contenido de reportes. Se loguea `UserId`, `traceId`, ruta, status, duración.

## Amenazas por flujo

| Flujo | Amenaza | Mitigación |
|---|---|---|
| Registro | Creación masiva de cuentas | Rate limit por IP, verificación de email antes de acciones con efecto en otros |
| Login | Credential stuffing | Bloqueo por cuenta + rate limit por IP + lista de contraseñas filtradas |
| Sesión | Robo de refresh token | Rotación con detección de reuso, cookie httpOnly |
| Perfil | Edición de perfil ajeno (IDOR) | Ownership en handler + tests negativos y positivos |
| Perfil | Imagen maliciosa | Re-encodeo, magic bytes, límites |
| Búsqueda | Scraping de contactos | Canales públicos solo en detalle, rate limit, sin exponer canales en listados |
| Contacto | Spam a profesionales | Email verificado, límite diario, una abierta por par, reportes |
| Moderación | Abuso de reportes para tumbar a un competidor | Suspensión solo por decisión humana, auditada; sin automatismo por cantidad de reportes |
| Admin | Escalada de privilegios | Roles solo por admin, auditoría, no quitar el último admin |
