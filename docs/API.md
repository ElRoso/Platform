# API

Etapa 0 (AGENTS.md §28, punto 13). Base: AGENTS.md §9. Contrato inicial: se versiona con OpenAPI generado desde el código en Etapa 1.

## Convenciones

- Base `/api/v1`. JSON `camelCase`. Fechas ISO 8601 en UTC (`2026-11-23T14:00:00Z`).
- Ids `uuid`. Recursos públicos de perfil por `slug`; operaciones de dueño por `id`.
- Autenticación: la Web (BFF) llama con `Authorization: Bearer <access>`; el browser nunca tiene el token (ADR-0006).
- Errores: Problem Details RFC 9457 con `type`, `title`, `status`, `detail`, `instance`, `traceId` y, en validación, `errors: { campo: [mensajes] }`. Código de regla de negocio en `code` (ej. `profile.publish.missing_specialty`).
- Paginación: `?page=1&pageSize=20` (máx 50). Respuesta `{ items, page, pageSize, totalItems, totalPages }`.
- Idempotencia: `POST` que crean recursos repetibles aceptan `Idempotency-Key`; además las reglas de dominio hacen idempotentes contact-requests (BR-063) y favoritos (BR-070).
- Rate limit: `429` con `Retry-After`.
- Autorización: `anon` (público), `user` (autenticado), `verified` (email verificado), `owner` (dueño del recurso), `mod`, `admin`.

## Endpoints MVP

### Auth (Identity)

| Método | Ruta | Auth | Cuerpo / respuesta | Códigos |
|---|---|---|---|---|
| POST | `/auth/register` | anon | `{ email, password, displayName }` → `202` siempre (BR-003) | 202, 400, 429 |
| POST | `/auth/verify-email` | anon | `{ token }` | 204, 400 |
| POST | `/auth/resend-verification` | user | — | 202, 429 |
| POST | `/auth/login` | anon | `{ email, password }` → `{ accessToken, expiresIn, refreshToken }` | 200, 401, 423, 429 |
| POST | `/auth/refresh` | anon | `{ refreshToken }` → tokens nuevos | 200, 401 |
| POST | `/auth/logout` | user | — revoca sesión actual | 204 |
| GET | `/auth/sessions` | user | lista de sesiones | 200 |
| DELETE | `/auth/sessions/{id}` | user (owner) | — | 204, 404 |
| POST | `/auth/forgot-password` | anon | `{ email }` → `202` siempre | 202, 429 |
| POST | `/auth/reset-password` | anon | `{ token, newPassword }` | 204, 400 |
| GET | `/me` | user | `{ id, email, emailVerified, displayName, roles, professionalProfileId? }` | 200 |

### Perfiles (Profiles)

| Método | Ruta | Auth | Descripción | Códigos |
|---|---|---|---|---|
| GET | `/me/profile` | user | Perfil personal | 200 |
| PUT | `/me/profile` | user | Editar perfil personal | 204, 400 |
| POST | `/professionals` | user | Crear borrador → `201` + `Location` | 201, 409 (ya tiene) |
| GET | `/me/professional` | user | Mi perfil profesional + `missingRequirements[]` | 200, 404 |
| PUT | `/professionals/{id}` | owner | Editar (parcial por secciones) | 204, 400, 403, 409 (concurrencia), 422 (BR-026) |
| POST | `/professionals/{id}/publish` | owner, verified | Publicar | 204, 403, 422 |
| POST | `/professionals/{id}/pause` | owner | Pausar | 204, 403, 409 |
| POST | `/professionals/{id}/resume` | owner, verified | Reactivar | 204, 403, 422 |
| POST | `/professionals/{id}/portfolio` | owner | `multipart/form-data` imagen | 201, 400, 413, 415, 422 (límite) |
| DELETE | `/professionals/{id}/portfolio/{itemId}` | owner | Borrar imagen | 204 |
| GET | `/professionals/{slug}` | anon | Perfil público (solo `Published`; `Paused` → 200 con `available:false`) | 200, 404 |

### Catálogo y geografía

| Método | Ruta | Auth | Descripción |
|---|---|---|---|
| GET | `/categories` | anon | Árbol activo |
| GET | `/categories/{slug}` | anon | Categoría con especialidades |
| GET | `/locations/provinces` | anon | Provincias |
| GET | `/locations/localities?provinceId=&q=` | anon | Localidades (autocompletar, máx 20) |
| GET | `/locations/resolve?q=` | anon | Texto → localidad probable |

### Búsqueda (Discovery)

| Método | Ruta | Auth | Descripción |
|---|---|---|---|
| GET | `/search/professionals` | anon | `q, categoryId, specialtyId, provinceId, localityId, radiusKm, modality, sort (relevance\|distance\|recent), page, pageSize` → `{ interpretation, items, ...paginación, suggestions }` |
| GET | `/search/landing?category=&province=&locality=` | anon | Datos para páginas SEO (resultados + metadata) |

`interpretation` devuelve lo que el sistema entendió (`{ specialty, category, locality, modality, freeText }`) para que la UI lo muestre como chips editables.

### Contacto

| Método | Ruta | Auth | Descripción | Códigos |
|---|---|---|---|---|
| POST | `/contact-requests` | verified | `{ professionalId, message, preferredChannel }` | 201, 200 (existente, BR-063), 403, 422, 429 |
| GET | `/contact-requests/mine` | user | Enviadas | 200 |
| GET | `/contact-requests/received` | owner de perfil | Recibidas | 200 |
| POST | `/contact-requests/{id}/viewed` | profesional destinatario | | 204 |
| POST | `/contact-requests/{id}/respond` | profesional destinatario | | 204, 409 |
| POST | `/contact-requests/{id}/close` | cualquiera de las partes | | 204, 409 |
| POST | `/contact-requests/{id}/cancel` | solicitante | | 204, 409 |

### Favoritos

| Método | Ruta | Auth | Códigos |
|---|---|---|---|
| GET | `/favorites` | user | 200 |
| PUT | `/favorites/{professionalId}` | user | 204 (idempotente) |
| DELETE | `/favorites/{professionalId}` | user | 204 |

AGENTS.md §9.1 propone `POST /favorites`; se usa `PUT` con id en la ruta porque la operación es idempotente por naturaleza (BR-070).

### Moderación y admin

| Método | Ruta | Auth |
|---|---|---|
| POST | `/reports` | user — `{ targetType, targetId, reason, detail }` |
| GET | `/moderation/cases?status=` | mod |
| GET | `/moderation/cases/{id}` | mod |
| POST | `/moderation/cases/{id}/resolve` | mod — `{ action, reason }` |
| POST | `/moderation/cases/{id}/reopen` | admin |
| GET | `/admin/users?q=` | admin |
| POST | `/admin/users/{id}/lock` · `/unlock` | admin |
| PUT | `/admin/users/{id}/roles` | admin |
| POST/PUT/DELETE | `/admin/categories…` | admin |
| GET/PUT | `/admin/settings` | admin |
| GET | `/admin/audit?from=&to=` | admin |

### Operación

`GET /health/live`, `GET /health/ready` (fuera de `/api/v1`, sin auth, sin detalles en producción).

## Ejemplo de error

```json
{
  "type": "https://platform.example/errors/profile.publish.missing_specialty",
  "title": "El perfil no cumple los requisitos de publicación",
  "status": 422,
  "detail": "Agregá al menos una especialidad.",
  "code": "profile.publish.missing_specialty",
  "traceId": "00-4bf92f…-01"
}
```
