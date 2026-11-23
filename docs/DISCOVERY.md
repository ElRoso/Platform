# Discovery

Etapa 0 (AGENTS.md §28, puntos 1–4). Issue #1.

## 1. Diagnóstico del repositorio

| Aspecto | Estado |
|---|---|
| Código | No existe. Árbol de carpetas vacío según AGENTS.md §23. |
| Documentación | `AGENTS.md` (maestro), `CLAUDE.md`, `.claude/` (reglas y agentes), este `docs/`. |
| Git | Rama base `mainline`, remoto público `ElRoso/Platform`, licencia AGPL-3.0-only. |
| Tracker | GitHub Issues (#1–#8 abiertos para Etapas 0 y 1). |
| CI | No existe. |
| Restricciones fijas | Gratuito, sin monetización (§2.3); open source sin dependencias pagas; mobile first; Argentina primero; Ley 25.326. |

Conclusión: proyecto greenfield. No hay deuda ni contratos que preservar. El riesgo principal es sobrediseñar: el MVP se mide por conexiones reales, no por módulos.

## 2. Visión funcional

> Conectamos personas que saben hacer con personas que necesitan.

Tres momentos definen el producto:

1. **Mostrarse.** Una persona crea un perfil profesional en minutos, desde el celular, sin pagar ni completar datos innecesarios.
2. **Encontrar.** Otra persona escribe lo que necesita en lenguaje natural ("electricista en Don Torcuato") y obtiene resultados cercanos y relevantes, ordenados sin favoritismos pagos.
3. **Conectar.** Contacta por el canal que el profesional eligió publicar (WhatsApp, teléfono, email) o con una solicitud interna.

Métrica norte: **contactos iniciados por semana** hacia profesionales publicados. Métricas de soporte: perfiles publicados, búsquedas con al menos un resultado, tasa de respuesta a solicitudes internas.

Lo que el producto no es en el MVP: marketplace, plataforma de pagos, chat en tiempo real, agencia de matching con IA.

## 3. Actores

Una persona tiene una sola cuenta (`User`). Ser profesional es tener un `ProfessionalProfile`, no otro tipo de cuenta. Moderador y administrador son roles asignados.

| Acción | Visitante | Usuario | Profesional | Moderador | Admin |
|---|:-:|:-:|:-:|:-:|:-:|
| Buscar, explorar categorías, ver perfiles públicos, compartir | ✓ | ✓ | ✓ | ✓ | ✓ |
| Ver canales públicos de contacto (WhatsApp, teléfono, email publicados) | ✓ | ✓ | ✓ | ✓ | ✓ |
| Registrarse | ✓ | — | — | — | — |
| Editar perfil personal y preferencias | — | ✓ | ✓ | ✓ | ✓ |
| Guardar favoritos | — | ✓ | ✓ | ✓ | ✓ |
| Enviar solicitud de contacto interna | — | ✓ (email verificado) | ✓ (a otro profesional) | ✓ | ✓ |
| Reportar contenido | — | ✓ | ✓ | ✓ | ✓ |
| Crear, publicar, pausar, editar su perfil profesional | — | ✓ (se convierte en profesional) | ✓ | — | — |
| Recibir y responder solicitudes | — | — | ✓ | — | — |
| Revisar reportes, ocultar o suspender perfiles, resolver casos | — | — | — | ✓ | ✓ |
| Gestionar usuarios, roles, categorías, parámetros, auditoría | — | — | — | — | ✓ |

Mínimo privilegio: un moderador no administra roles; un admin no edita perfiles ajenos, solo aplica acciones de moderación auditadas.

## 4. Casos de uso del MVP

Formato: actor · precondición · flujo principal · reglas (ver `DOMAIN.md`) · módulo.

### Identidad

| ID | Caso | Actor | Flujo | Reglas | Módulo |
|---|---|---|---|---|---|
| UC-01 | Registrarse | Visitante | Email + contraseña + nombre → cuenta creada sin verificar → email de verificación | BR-001..004 | Identity |
| UC-02 | Verificar email | Usuario | Abre link → cuenta verificada | BR-005 | Identity |
| UC-03 | Iniciar sesión | Visitante | Email + contraseña → sesión con cookie httpOnly | BR-006..008 | Identity |
| UC-04 | Cerrar sesión / revocar sesiones | Usuario | Cierra la actual o todas | BR-009 | Identity |
| UC-05 | Recuperar contraseña | Visitante | Pide link → email (siempre la misma respuesta) → nueva contraseña → sesiones revocadas | BR-002, BR-010 | Identity |

### Perfiles y catálogo

| ID | Caso | Actor | Flujo | Reglas | Módulo |
|---|---|---|---|---|---|
| UC-06 | Editar perfil personal | Usuario | Nombre, foto, localidad aproximada, preferencias | BR-020 | Profiles |
| UC-07 | Crear perfil profesional (borrador) | Usuario | Paso a paso: qué hacés → dónde → cómo contactarte → descripción; se guarda borrador | BR-021, BR-022 | Profiles |
| UC-08 | Publicar perfil | Profesional | Valida requisitos → `Published` → indexado en búsqueda | BR-023..026 | Profiles |
| UC-09 | Pausar / reactivar perfil | Profesional | `Published` ↔ `Paused`; pausado no aparece en búsqueda ni recibe solicitudes | BR-027, BR-028 | Profiles |
| UC-10 | Editar perfil publicado | Profesional | Cambios visibles al guardar; si deja de cumplir requisitos, no se guarda | BR-026 | Profiles |
| UC-11 | Gestionar portfolio | Profesional | Subir, ordenar, borrar fotos de trabajos | BR-029, BR-030 | Profiles |
| UC-12 | Gestionar categorías | Admin | ABM jerárquico de categorías, profesiones, especialidades, sinónimos | BR-040..042 | Catalog |

### Descubrimiento

| ID | Caso | Actor | Flujo | Reglas | Módulo |
|---|---|---|---|---|---|
| UC-13 | Buscar en lenguaje natural | Cualquiera | Texto libre → interpretación (servicio, categoría, ubicación, modalidad) → resultados | BR-050..053 | Discovery |
| UC-14 | Filtrar y ordenar | Cualquiera | Categoría, especialidad, provincia, localidad, radio, modalidad, disponibilidad, experiencia; orden por relevancia, cercanía, actualización | BR-054, BR-055 | Discovery |
| UC-15 | Explorar por categoría y localidad | Cualquiera | Páginas indexables `/profesionales/{categoria}/{provincia}/{localidad}` | BR-056 | Discovery |
| UC-16 | Ver perfil público | Cualquiera | `/profesional/{slug}` con datos publicados y canales permitidos | BR-031, BR-060 | Profiles (lectura) |

### Contacto y favoritos

| ID | Caso | Actor | Flujo | Reglas | Módulo |
|---|---|---|---|---|---|
| UC-17 | Contactar por canal público | Cualquiera | Botón WhatsApp (`wa.me`), teléfono o email según lo publicado | BR-060 | Contact |
| UC-18 | Enviar solicitud interna | Usuario verificado | Mensaje + canal preferido de respuesta → `Pending` → profesional notificado | BR-061..065 | Contact |
| UC-19 | Gestionar solicitudes recibidas | Profesional | Ver (`Viewed`), responder (`Responded`), cerrar (`Closed`) | BR-066 | Contact |
| UC-20 | Ver mis solicitudes enviadas | Usuario | Lista con estado; cancelar si está `Pending` | BR-066 | Contact |
| UC-21 | Guardar / quitar favorito | Usuario | Toggle en perfil o card | BR-070 | Favorites |

### Confianza

| ID | Caso | Actor | Flujo | Reglas | Módulo |
|---|---|---|---|---|---|
| UC-22 | Reportar perfil, imagen o contenido | Usuario | Motivo + detalle → `Report` → caso de moderación | BR-080, BR-081 | Moderation |
| UC-23 | Resolver caso | Moderador | Revisa → descarta, oculta contenido o suspende perfil, con motivo | BR-082..084 | Moderation |
| UC-24 | Panel de administración | Admin | Usuarios (bloquear), roles, categorías, parámetros, auditoría | BR-090..092 | Identity, Catalog, Moderation |

### Fuera del MVP (preparado, no implementado)

Valoraciones (Reputation), notificaciones push, chat, pagos, IA de matching, apps nativas. Ver decisión D13 en `ROADMAP.md`.
