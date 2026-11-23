# AGENTS.md

**Plataforma de Conexión entre Profesionales, Oficios y Usuarios**

Documento maestro de arquitectura, producto, desarrollo y calidad.

| Campo | Valor |
|---|---|
| Versión | 1.0 |
| Estado | Documento inicial |
| Arquitectura | Domain-Driven Design + Clean Architecture + Modular Monolith |
| Metodología | Test-Driven Development |
| Backend | C# / ASP.NET Core |
| Frontend | React / Next.js / TypeScript |
| Base de datos | PostgreSQL / PostGIS |
| Diseño | Mobile First |
| Idioma principal | Español |
| Mercado inicial | Argentina |
| Moneda y pagos | No aplican al modelo de negocio inicial. |
| Licencia | AGPL-3.0-only. Cero dependencias de pago; solo licencias compatibles con AGPL 3. |

---

## 1. Instrucciones generales para los agentes de IA

Este repositorio puede ser trabajado por Claude Code, OpenAI Codex u otros agentes de desarrollo. Todos los agentes deben respetar las mismas reglas.

### 1.1. Prioridades

Orden de prioridades:

1. Correctitud funcional.
2. Integridad del dominio.
3. Seguridad.
4. Calidad arquitectónica.
5. Testabilidad.
6. Mantenibilidad.
7. Experiencia de usuario.
8. Rendimiento.
9. Escalabilidad.
10. Velocidad de implementación.

Nunca sacrificar una prioridad superior para mejorar una inferior.

### 1.2. Comportamiento esperado

Antes de realizar cualquier cambio:

- Inspeccionar el repositorio.
- Leer este documento.
- Leer el README.
- Revisar la arquitectura existente.
- Revisar los módulos involucrados.
- Revisar los tests existentes.
- Identificar convenciones.
- Identificar dependencias.
- Identificar posibles efectos colaterales.

No asumir que el repositorio está vacío.
No sobrescribir implementaciones existentes sin comprenderlas.
No modificar contratos públicos sin evaluar su impacto.
No realizar refactorizaciones masivas fuera del alcance de la tarea.

### 1.3. Prohibiciones

Está prohibido:

- Implementar lógica de negocio en controladores.
- Implementar reglas de dominio en componentes React.
- Utilizar `DbContext` directamente desde controladores.
- Crear entidades anémicas por comodidad.
- Introducir dependencias circulares.
- Utilizar `catch` vacíos.
- Ocultar excepciones.
- Desactivar tests para conseguir un build exitoso.
- Utilizar `any` en TypeScript sin justificación documentada.
- Utilizar `dynamic` indiscriminadamente en C#.
- Duplicar reglas de negocio.
- Agregar paquetes sin justificación.
- Agregar dependencias de pago, con licencia comercial o no OSI. Revisar la licencia de la versión exacta: hay librerías que pasaron a pago (MediatR, FluentAssertions 8+).
- Crear abstracciones que no aporten valor.
- Agregar microservicios prematuramente.
- Introducir infraestructura compleja sin necesidad.
- Almacenar secretos en el repositorio.
- Utilizar información privada en logs.
- Considerar un feature terminado sin pruebas.

### 1.4. Protocolo de trabajo

Para cada tarea:

1. Analizar el problema.
2. Identificar el bounded context.
3. Identificar las reglas de negocio.
4. Definir criterios de aceptación.
5. Identificar pruebas necesarias.
6. Escribir primero los tests.
7. Ejecutar tests y verificar RED.
8. Implementar la solución mínima.
9. Verificar GREEN.
10. Refactorizar.
11. Ejecutar toda la suite relevante.
12. Ejecutar build.
13. Revisar seguridad y rendimiento.
14. Actualizar documentación.
15. Informar resultados.

No avanzar si existen errores de compilación o tests fallidos relacionados con el alcance.

## 2. Visión del producto

### 2.1. Propósito

Crear una plataforma digital que conecte a personas que ofrecen sus conocimientos, profesiones, habilidades y oficios con personas que necesitan esos servicios.

La plataforma debe permitir que cualquier persona pueda mostrar lo que sabe hacer y conseguir oportunidades de trabajo.

No debe existir ninguna barrera económica para utilizar el servicio.

### 2.2. Propuesta de valor

"Conectamos personas que saben hacer con personas que necesitan."

Principios:

- Gratuita para todos.
- Sin comisiones.
- Sin suscripciones.
- Sin publicaciones pagas.
- Sin posicionamiento preferencial comprado.
- Sin restricciones artificiales de visibilidad.
- Sin costos ocultos.
- Accesible para personas con diferentes niveles de alfabetización digital.

### 2.3. Modelo de negocio

El producto no debe incorporar mecanismos de monetización que contradigan su propósito.

No implementar:

- Planes premium.
- Perfiles patrocinados.
- Publicaciones pagas.
- Comisiones por contratación.
- Créditos de contacto.
- Límites artificiales para generar ingresos.

Cualquier futura modificación de este principio requiere una decisión explícita del propietario del producto y una revisión de impacto.

## 3. Actores del sistema

### 3.1. Visitante

Persona que utiliza la plataforma sin autenticarse.

Puede:

- Consultar la página principal.
- Buscar profesionales.
- Explorar categorías.
- Ver perfiles públicos.
- Compartir perfiles.
- Consultar ubicaciones generales.

No puede:

- Guardar favoritos.
- Enviar solicitudes identificadas.
- Administrar perfiles.

### 3.2. Usuario registrado

Puede:

- Editar su perfil.
- Guardar favoritos.
- Contactar profesionales.
- Consultar sus solicitudes.
- Reportar contenido.
- Gestionar sus preferencias.

### 3.3. Profesional

Es un usuario registrado que decide ofrecer uno o más servicios.

Puede:

- Crear perfil profesional.
- Publicar servicios.
- Definir especialidades.
- Subir trabajos realizados.
- Definir zonas de cobertura.
- Recibir solicitudes.
- Administrar disponibilidad.
- Pausar o reactivar su perfil.

Una persona puede ser usuario y profesional al mismo tiempo.
No crear cuentas separadas por tipo de actor.

### 3.4. Moderador

Puede:

- Revisar reportes.
- Moderar contenido.
- Ocultar perfiles.
- Resolver denuncias.
- Registrar motivos de moderación.

### 3.5. Administrador

Puede:

- Gestionar usuarios.
- Gestionar categorías.
- Administrar permisos.
- Configurar parámetros.
- Consultar auditorías.
- Gestionar moderadores.

Aplicar principio de mínimo privilegio.

## 4. Alcance del MVP

El MVP debe concentrarse en conseguir conexiones reales.

### Funcionalidades obligatorias

1. Registro.
2. Inicio de sesión.
3. Recuperación de contraseña.
4. Perfil de usuario.
5. Creación de perfil profesional.
6. Categorías y especialidades.
7. Publicación de perfil.
8. Edición de perfil.
9. Búsqueda por profesión u oficio.
10. Búsqueda por ubicación.
11. Filtros.
12. Resultados.
13. Detalle del profesional.
14. Contacto por WhatsApp.
15. Solicitud de contacto interna.
16. Favoritos.
17. Reportes.
18. Panel básico de administración.
19. SEO de perfiles públicos.
20. Diseño responsive.

### Fuera del MVP

No implementar inicialmente:

- Pagos.
- Marketplace transaccional.
- Comisiones.
- Chat en tiempo real.
- Videollamadas.
- Contrataciones con firma digital.
- Facturación.
- Gestión de presupuestos avanzada.
- Inteligencia artificial para matching.
- Aplicaciones móviles nativas.

Preparar la arquitectura para futuras extensiones, sin implementarlas anticipadamente.

## 5. Arquitectura general

### 5.1. Estilo arquitectónico

Utilizar Modular Monolith. No iniciar con microservicios.

Cada módulo debe tener:

- Responsabilidad definida.
- Modelo de dominio propio.
- Contratos explícitos.
- Reglas de negocio encapsuladas.
- Tests independientes.
- Dependencias controladas.

### 5.2. Principios

- Domain-Driven Design.
- Clean Architecture.
- SOLID.
- Separation of Concerns.
- Dependency Inversion.
- Explicit Dependencies.
- High Cohesion.
- Low Coupling.
- Fail Fast.
- Secure by Default.

### 5.3. Capas

**Domain:** Entidades. Value Objects. Agregados. Domain Events. Reglas de negocio. Domain Services. Excepciones de dominio.

**Application:** Casos de uso. Commands. Queries. DTOs. Interfaces. Validaciones. Orquestación.

**Infrastructure:** Persistencia. EF Core. Repositorios. Integraciones. Almacenamiento. Servicios externos.

**Presentation:** Controllers. Endpoints. Request Models. Response Models. Middleware. Autorización HTTP.

### 5.4. Reglas de dependencia

- Domain no depende de ninguna otra capa.
- Application depende de Domain.
- Infrastructure implementa contratos definidos por Application y Domain.
- Presentation invoca Application.

Nunca invertir estas dependencias por comodidad.

### 5.5. Comunicación entre módulos

Preferir contratos explícitos.

Permitir:

- Interfaces públicas de módulos.
- Domain Events.
- Integration Events internos.
- Mensajes asincrónicos cuando sea necesario.

Evitar acceder directamente a tablas o repositorios privados de otros módulos.
No compartir entidades de dominio entre bounded contexts.

## 6. Bounded contexts

Definir inicialmente los siguientes módulos.

### 6.1. Identity

Responsabilidad: identidad y autenticación.

Entidades: User. RefreshToken. UserSession.

Funciones: Registro. Login. Logout. Recuperación de contraseña. Verificación de email. Administración de sesiones. Gestión de credenciales.

### 6.2. Profiles

Responsabilidad: administración de perfiles personales y profesionales.

Entidades: UserProfile. ProfessionalProfile. PortfolioItem. ProfessionalSpecialty.

Funciones: Crear perfil. Editar perfil. Publicar perfil. Pausar perfil. Reactivar perfil. Administrar fotografías. Administrar especialidades.

### 6.3. Catalog

Responsabilidad: clasificación de servicios.

Entidades: Category. Profession. Specialty. ServiceTag.

Soportar categorías jerárquicas. Ejemplo:

- Construcción: Albañilería. Electricidad. Plomería. Pintura. Techista.
- Salud: Kinesiología. Psicología. Nutrición.
- Tecnología: Desarrollo web. Soporte técnico. Diseño UX/UI.

Permitir categorías configurables sin modificar código.

### 6.4. Geography

Responsabilidad: representación geográfica.

Conceptos: Country. Province. Locality. PostalCode. GeoLocation. ServiceArea.

Implementar inicialmente Argentina. Preparar el diseño para incorporar otros países.
Utilizar PostGIS cuando corresponda.
No exponer domicilios particulares de profesionales.

### 6.5. Discovery

Responsabilidad: búsqueda y descubrimiento.

Funciones: Búsqueda textual. Búsqueda por categoría. Búsqueda por ubicación. Búsqueda por distancia. Filtros. Ordenamiento. Paginación. Ranking orgánico.

El módulo debe consumir contratos públicos de otros módulos.
No debe apropiarse de las entidades internas de Profiles o Catalog.

### 6.6. Contact

Responsabilidad: conexión entre usuarios y profesionales.

Entidades: ContactRequest. ContactChannel. ContactPreference.

Funciones: Crear solicitud. Consultar solicitudes. Aceptar o rechazar solicitudes cuando corresponda. Registrar estado. Consultar historial.

### 6.7. Favorites

Responsabilidad: administrar profesionales guardados.

Entidad: Favorite.

Regla: un usuario no puede guardar dos veces el mismo profesional.

### 6.8. Reputation

Responsabilidad: administrar reputación y valoraciones.

Entidades: Review. Rating. ReviewReport.

Las valoraciones deben estar asociadas a interacciones elegibles.

### 6.9. Moderation

Responsabilidad: seguridad de la comunidad.

Entidades: Report. ModerationAction. ModerationCase.

Funciones: Reportar perfil. Reportar imágenes. Reportar contenido. Investigar denuncias. Aplicar acciones. Registrar auditoría.

### 6.10. Notifications

Responsabilidad: notificaciones de eventos relevantes.

Canales futuros: Email. Notificaciones web. Push notifications.

En el MVP, implementar solamente los canales necesarios.

## 7. Modelado del dominio

### 7.1. Reglas generales

Las entidades deben proteger sus invariantes.
Evitar setters públicos indiscriminados.
Utilizar constructores y métodos de dominio expresivos.

Ejemplo conceptual:

```
ProfessionalProfile.Publish()
ProfessionalProfile.Pause()
ProfessionalProfile.UpdateDescription()
ProfessionalProfile.AddSpecialty()
ProfessionalProfile.AddPortfolioItem()
```

No permitir modificar directamente estados internos desde Application.

### 7.2. ProfessionalProfile

Atributos conceptuales: Id. UserId. DisplayName. Description. ProfileImage. Categories. Specialties. ExperienceYears. WorkModality. ServiceAreas. PublicContactPreferences. Status. CreatedAt. UpdatedAt. PublishedAt.

Estados: Draft. Published. Paused. Suspended.

Transiciones controladas por reglas de dominio.
Un perfil suspendido no puede publicarse sin resolución de moderación.

### 7.3. Reglas de publicación

Un perfil solamente puede publicarse si:

- Tiene nombre visible.
- Tiene descripción válida.
- Tiene al menos una categoría o especialidad.
- Tiene una modalidad de trabajo.
- Tiene ubicación o zona de cobertura cuando el servicio lo requiere.
- Cumple requisitos de publicación.

No exigir teléfono público.
No exigir fotografías de trabajos.

### 7.4. Value Objects

Evaluar: EmailAddress. PhoneNumber. PersonName. ProfessionalDescription. GeoCoordinate. PostalCode. RatingValue. ServiceRadius. ProfileSlug.

Los Value Objects deben ser inmutables.
No crear Value Objects innecesarios.

### 7.5. Domain Events

Evaluar eventos: UserRegistered. ProfessionalProfileCreated. ProfessionalProfilePublished. ProfessionalProfilePaused. ContactRequestCreated. ReviewSubmitted. ProfileReported.

Los eventos deben expresar hechos que ocurrieron, no comandos.

## 8. Base de datos

### 8.1. Tecnología

- PostgreSQL como motor principal.
- EF Core para persistencia.
- PostGIS para capacidades geográficas.

### 8.2. Reglas

- UUID como identificador cuando sea conveniente.
- Foreign Keys reales.
- Índices explícitos.
- Restricciones únicas.
- Restricciones CHECK cuando correspondan.
- Migraciones versionadas.
- Timestamps en UTC.
- Transacciones explícitas cuando corresponda.
- Soft delete solamente cuando exista una necesidad funcional.
- Auditoría de cambios sensibles.

### 8.3. Índices iniciales

Evaluar índices para: UserId. ProfileStatus. CategoryId. SpecialtyId. LocalityId. PublishedAt. Slug. Ubicación geográfica. Búsquedas textuales.

Usar índices GIN, GiST y otros según el tipo de consulta.
No indexar indiscriminadamente.

### 8.4. Búsquedas

Evaluar PostgreSQL Full Text Search y trigramas.
No incorporar Elasticsearch inicialmente salvo que exista evidencia de que PostgreSQL no alcanza los requisitos.

### 8.5. Migraciones

Cada modificación estructural debe incluir una migración.
No modificar bases productivas manualmente.
Evaluar compatibilidad de migraciones con despliegues progresivos.

## 9. API backend

### 9.1. Convenciones

API REST versionada. Prefijo: `/api/v1`. Utilizar JSON. Utilizar nombres consistentes.

Ejemplos:

```
POST   /api/v1/auth/register
POST   /api/v1/auth/login
GET    /api/v1/professionals
GET    /api/v1/professionals/{slug}
POST   /api/v1/professionals
PUT    /api/v1/professionals/{id}
POST   /api/v1/professionals/{id}/publish
POST   /api/v1/professionals/{id}/pause
POST   /api/v1/contact-requests
GET    /api/v1/contact-requests/mine
POST   /api/v1/favorites
DELETE /api/v1/favorites/{professionalId}
GET    /api/v1/categories
GET    /api/v1/locations
```

### 9.2. Respuestas

Utilizar Problem Details conforme a RFC 9457.
Errores consistentes.
No exponer excepciones internas.

### 9.3. Paginación

Utilizar paginación explícita.
Limitar tamaño máximo de página.
Evitar devolver colecciones ilimitadas.

### 9.4. Validaciones

Validar en Application y reforzar invariantes en Domain.
La validación del frontend no reemplaza la validación del backend.

### 9.5. Idempotencia

Evaluar idempotencia para operaciones que puedan repetirse por reintentos.
Evitar duplicaciones de solicitudes de contacto.

## 10. Frontend

### 10.1. Stack

- Next.js App Router.
- React.
- TypeScript.
- Tailwind CSS.
- shadcn/ui.
- React Hook Form.
- Zod.
- TanStack Query cuando aporte valor.
- Vitest.
- React Testing Library.
- Playwright.

Verificar compatibilidad de versiones antes de instalar.

### 10.2. Organización

Separar: App routes. Features. Components. Hooks. Services. API clients. Schemas. Types. UI primitives. Tests.

Organizar por funcionalidades cuando mejore la cohesión.
No crear una carpeta genérica con cientos de componentes mezclados.

### 10.3. Reglas

- TypeScript strict.
- Evitar `any`.
- Componentes pequeños.
- Componentes reutilizables cuando exista una necesidad real.
- Separar lógica de presentación.
- Manejar estados de loading.
- Manejar errores.
- Manejar estados vacíos.
- Manejar formularios.
- Evitar prop drilling innecesario.
- Evitar `useEffect` para sincronizaciones que puedan resolverse declarativamente.
- No almacenar información sensible en localStorage.
- Mantener contratos tipados con la API.

### 10.4. Rendering

Utilizar Server Components por defecto cuando corresponda.
Utilizar Client Components solamente cuando exista interacción del lado del cliente.
Evitar convertir páginas completas en Client Components sin necesidad.
Utilizar SSR o generación estática para contenido público indexable.

## 11. UX/UI: Mobile First

### 11.1. Principio

Diseñar primero para móviles.
La versión desktop debe ser una evolución natural, no el diseño original adaptado.

### 11.2. Referencias

Tomar inspiración de: Airbnb. Linear. Mercado Libre. Google Maps. Directorios profesionales modernos.

No copiar diseños propietarios.

### 11.3. Sistema de diseño

Definir: Colores. Tipografías. Espaciados. Bordes. Radios. Elevaciones. Componentes. Estados. Breakpoints.

Crear tokens de diseño.

### 11.4. Componentes

Desarrollar: Button. Input. SearchInput. SearchFilters. ProfessionalCard. CategoryCard. ProfileHeader. RatingStars. LocationBadge. ContactButton. FavoriteButton. EmptyState. LoadingSkeleton. ErrorState. BottomNavigation. Modal. Drawer. Toast. Pagination.

### 11.5. Navegación móvil

Evaluar navegación inferior con: Inicio. Buscar. Publicar. Favoritos. Mi perfil.

No mostrar elementos innecesarios a visitantes.

### 11.6. Formularios

Los formularios deben ser: Simples. Progresivos. Accesibles. Claros. Tolerantes a errores. Optimizados para teclado móvil.

Guardar borradores cuando aporte valor.
Evitar pedir datos innecesarios.

### 11.7. Accesibilidad

Objetivo WCAG 2.2 AA.

Incluir: Navegación por teclado. Focus visible. Contraste adecuado. Labels. Roles semánticos. Mensajes de error accesibles. Compatibilidad con lectores de pantalla. Targets táctiles cómodos.

## 12. Experiencia de búsqueda

La búsqueda es el núcleo del producto.

### 12.1. Búsqueda principal

Permitir expresiones naturales:

- "Necesito un electricista en Don Torcuato".
- "Busco una persona que arregle lavarropas".
- "Necesito diseñador gráfico remoto".

El sistema debe identificar: Servicio. Categoría. Ubicación. Modalidad.

No exigir una coincidencia exacta de palabras.

### 12.2. Filtros

Categoría. Especialidad. Localidad. Provincia. Radio. Modalidad. Disponibilidad declarada. Calificación. Experiencia, cuando esté informada.

### 12.3. Ordenamiento

Relevancia. Cercanía. Mejor valoración. Recientemente actualizado.

No crear rankings pagos.

### 12.4. Ranking orgánico

Diseñar una estrategia transparente.

Considerar: Coincidencia con la búsqueda. Coincidencia geográfica. Especialidades. Perfil publicado y vigente. Completitud del perfil. Calidad de la información.

No penalizar a profesionales nuevos por tener pocas valoraciones.
Evitar que el ranking se convierta en una barrera injusta.

## 13. Contacto y comunicación

### 13.1. Canales

Permitir: WhatsApp. Teléfono. Email. Solicitud interna.

### 13.2. Privacidad

El profesional decide qué datos publicar.
Nunca exponer email o teléfono privados sin consentimiento.

### 13.3. Solicitudes

Estados posibles: Pending. Viewed. Responded. Closed. Cancelled.

Definir transiciones explícitas.

### 13.4. Antiabuso

Implementar: Rate limiting. Límites razonables de solicitudes. Protección contra spam. Reportes. Bloqueos cuando corresponda.

No restringir injustificadamente el uso legítimo.

## 14. Reputación

### 14.1. Valoraciones

Escala de 1 a 5. Permitir comentarios opcionales.

### 14.2. Reglas

- No autovaloraciones.
- No múltiples valoraciones para una misma interacción.
- No permitir valoraciones anónimas sin una política definida.
- Permitir reportar valoraciones abusivas.
- Permitir moderación.
- Mantener trazabilidad.

### 14.3. Profesionales nuevos

No mostrar una puntuación de cero.

Diferenciar claramente: Sin valoraciones. Con valoraciones.

No penalizar a quien recién se registra.

## 15. Seguridad

Aplicar OWASP ASVS como referencia de seguridad y OWASP Top 10.

### 15.1. Autenticación

- Hash seguro de contraseñas.
- Política de contraseñas.
- Recuperación segura.
- Verificación de email.
- Refresh token rotation cuando corresponda.
- Revocación de sesiones.
- Protección contra fuerza bruta.

### 15.2. Autorización

- Autorización por recurso.
- Ownership validation.
- Roles.
- Policies.
- Mínimo privilegio.

Un usuario no puede editar el perfil de otro profesional.

### 15.3. API

- Rate limiting.
- Input validation.
- Protección contra inyección.
- CORS restrictivo.
- Headers de seguridad.
- Manejo seguro de errores.
- Protección contra enumeración de cuentas.

### 15.4. Archivos

- Validación de extensión.
- Validación del MIME real.
- Límites de tamaño.
- Nombres aleatorios.
- Restricción de formatos.
- Eliminación de metadatos EXIF cuando corresponda.
- Protección contra archivos maliciosos.

Nunca confiar exclusivamente en el nombre o extensión del archivo.

### 15.5. Privacidad

- Minimización de datos.
- Consentimiento.
- Eliminación de cuenta.
- Exportación de información cuando corresponda.
- Retención definida.
- Protección de información personal.
- No almacenar ubicación exacta innecesariamente.

Diseñar conforme a la legislación aplicable en Argentina, incluyendo la Ley 25.326 de Protección de Datos Personales, y evaluar requisitos adicionales antes del lanzamiento.

## 16. SEO

Los perfiles profesionales públicos deben poder posicionarse en buscadores.

Implementar: URLs amigables. Metadata dinámica. Open Graph. Sitemap. Robots.txt. Canonical URLs. Breadcrumbs. Datos estructurados. SSR. Páginas indexables de categorías y localidades.

Ejemplos:

```
/profesionales/electricistas
/profesionales/electricistas/buenos-aires
/profesionales/electricistas/buenos-aires/don-torcuato
/profesional/juan-perez-electricista
```

No generar páginas indexables duplicadas para cada combinación de filtros.

## 17. Test-Driven Development

TDD es obligatorio.

### 17.1. Ciclo

- **RED:** escribir una prueba que falle por el comportamiento esperado.
- **GREEN:** implementar el mínimo código para pasar.
- **REFACTOR:** mejorar diseño sin cambiar comportamiento.

### 17.2. Backend

Herramientas: xUnit. AwesomeAssertions. NSubstitute o Moq. Testcontainers. WebApplicationFactory.

Tipos: Unit tests. Integration tests. API tests. Architecture tests.

### 17.3. Frontend

Herramientas: Vitest. React Testing Library. Playwright.

Tipos: Unit tests. Component tests. Integration tests. E2E.

### 17.4. Casos de prueba prioritarios

- **Registro:** Email válido. Email inválido. Email duplicado. Contraseña inválida.
- **Perfil:** Creación. Publicación válida. Publicación incompleta. Pausa. Reactivación. Edición no autorizada.
- **Búsqueda:** Por categoría. Por localidad. Por distancia. Sin resultados. Filtros combinados. Paginación.
- **Contacto:** Solicitud válida. Destinatario inexistente. Profesional pausado. Duplicados. Abuso de solicitudes.
- **Reputación:** Valoración válida. Autovaloración. Duplicación. Interacción no elegible.

### 17.5. Cobertura

No perseguir un porcentaje arbitrario.

Priorizar cobertura de: Invariantes del dominio. Casos de uso críticos. Autorización. Persistencia. Flujos principales.

Establecer umbrales graduales para impedir regresiones.

## 18. Tests de arquitectura

Incorporar pruebas que verifiquen:

- Domain no depende de Infrastructure.
- Domain no depende de Presentation.
- Application no depende de Presentation.
- No existen dependencias circulares.
- Los módulos respetan sus límites.
- Los controladores no contienen lógica de negocio.
- No se accede a tablas de otros módulos directamente.

Evaluar NetArchTest o una herramienta equivalente.

## 19. Observabilidad

Implementar: Structured Logging. Serilog. Correlation IDs. Health Checks. Métricas. Trazas distribuidas cuando corresponda. OpenTelemetry.

No registrar: Contraseñas. Tokens. Datos sensibles. Contenido privado innecesario.

Incorporar monitoreo de: Errores HTTP. Latencia. Consultas lentas. Fallas de autenticación. Errores de integraciones. Uso de recursos.

## 20. Rendimiento

Objetivos iniciales orientativos:

- API de lectura habitual: p95 menor a 300 ms bajo carga de referencia.
- Búsquedas habituales: p95 menor a 500 ms bajo carga de referencia.
- LCP móvil: idealmente menor a 2,5 segundos.
- CLS: menor a 0,1.
- INP: menor a 200 ms.

Estos objetivos deben medirse en un entorno representativo, no asumirse como garantizados.

Aplicar: Paginación. Índices. Consultas proyectadas. Evitar N+1. Optimización de imágenes. Lazy loading. Cache selectivo. Compresión. CDN cuando corresponda.

No agregar cache indiscriminadamente.

## 21. Infraestructura

### 21.1. Docker

Crear: Dockerfile backend. Dockerfile frontend. Docker Compose. PostgreSQL. Volúmenes persistentes. Health checks.

### 21.2. Ambientes

Separar: Development. Test. Staging. Production.

### 21.3. CI/CD

Configurar GitHub Actions.

Pipeline:

1. Checkout.
2. Restauración de dependencias.
3. Formateo.
4. Análisis estático.
5. Tests backend.
6. Tests frontend.
7. Tests de integración.
8. Build backend.
9. Build frontend.
10. Verificación de migraciones.
11. Publicación de artefactos.

No desplegar si falla una validación obligatoria.

### 21.4. Secretos

Usar variables de entorno y un gestor de secretos en producción.
No incluir archivos `.env` reales en Git.

## 22. Documentación

Mantener documentación actualizada.

Archivos recomendados:

- README.md
- AGENTS.md
- docs/ARCHITECTURE.md
- docs/DOMAIN.md
- docs/BOUNDED-CONTEXTS.md
- docs/DATABASE.md
- docs/API.md
- docs/TESTING.md
- docs/SECURITY.md
- docs/DEPLOYMENT.md
- docs/ROADMAP.md
- docs/decisions/

### ADR

Cada decisión arquitectónica importante debe documentarse.

Formato: Contexto. Problema. Alternativas. Decisión. Justificación. Consecuencias.

No modificar decisiones anteriores silenciosamente.

## 23. Estructura de repositorio propuesta

```
/
├── AGENTS.md
├── README.md
├── .editorconfig
├── .gitignore
├── docker-compose.yml
├── .github/
│   └── workflows/
│
├── docs/
│   ├── ARCHITECTURE.md
│   ├── DOMAIN.md
│   ├── BOUNDED-CONTEXTS.md
│   ├── DATABASE.md
│   ├── API.md
│   ├── TESTING.md
│   ├── SECURITY.md
│   ├── DEPLOYMENT.md
│   ├── ROADMAP.md
│   └── decisions/
│
├── backend/
│   ├── Platform.sln
│   ├── src/
│   │   ├── BuildingBlocks/
│   │   ├── Modules/
│   │   │   ├── Identity/
│   │   │   ├── Profiles/
│   │   │   ├── Catalog/
│   │   │   ├── Geography/
│   │   │   ├── Discovery/
│   │   │   ├── Contact/
│   │   │   ├── Favorites/
│   │   │   ├── Reputation/
│   │   │   ├── Moderation/
│   │   │   └── Notifications/
│   │   └── Host/
│   │
│   └── tests/
│       ├── UnitTests/
│       ├── IntegrationTests/
│       ├── ArchitectureTests/
│       └── FunctionalTests/
│
├── frontend/
│   ├── package.json
│   ├── src/
│   │   ├── app/
│   │   ├── components/
│   │   ├── features/
│   │   ├── hooks/
│   │   ├── lib/
│   │   ├── services/
│   │   ├── schemas/
│   │   └── types/
│   └── tests/
│       ├── unit/
│       ├── integration/
│       └── e2e/
│
└── infrastructure/
    ├── docker/
    └── scripts/
```

La estructura es orientativa. Ajustarla cuando el análisis arquitectónico demuestre una alternativa más simple.

## 24. Definition of Ready

Una tarea está lista para desarrollarse cuando:

- Tiene una descripción clara.
- Tiene criterios de aceptación.
- Se conoce el bounded context.
- Se conocen las reglas de negocio.
- Se identificaron dependencias.
- Se identificaron riesgos.
- Se definió el alcance.
- Se conoce cómo verificar el resultado.

Si falta información crítica, preguntar antes de asumir.

## 25. Definition of Done

Una funcionalidad está terminada cuando:

- Cumple criterios de aceptación.
- Respeta el dominio.
- Sigue TDD.
- Tiene tests adecuados.
- Todos los tests relevantes pasan.
- Compila.
- No introduce dependencias incorrectas.
- No contiene vulnerabilidades conocidas relacionadas con el cambio.
- Tiene manejo de errores.
- Tiene estados de loading y error cuando corresponda.
- Es responsive.
- Es accesible.
- Tiene documentación actualizada.
- No rompe funcionalidades existentes.

## 26. Roadmap

### Etapa 0: Discovery

No desarrollar funcionalidades.

Entregar: Visión del producto. Actores. Casos de uso. Event Storming inicial. Bounded Context Map. Modelo de dominio. Diagrama de arquitectura. Modelo de datos. ADR iniciales. Roadmap. Riesgos.

Esperar aprobación.

### Etapa 1: Fundaciones

Solución .NET. Aplicación Next.js. PostgreSQL. Docker. Tests. CI. Logging. Health checks.

### Etapa 2: Identidad

Registro. Login. Verificación. Recuperación. Autorización.

### Etapa 3: Perfiles

Perfil personal. Perfil profesional. Categorías. Especialidades. Publicación. Portfolio.

### Etapa 4: Discovery

Búsqueda. Geolocalización. Filtros. Ranking. SEO.

### Etapa 5: Contacto

WhatsApp. Solicitudes. Historial. Favoritos.

### Etapa 6: Confianza

Valoraciones. Reportes. Moderación. Auditoría.

### Etapa 7: Lanzamiento

Seguridad. Performance. Accesibilidad. E2E. Despliegue. Monitoreo.

## 27. Instrucciones específicas para Claude Code y Codex

### 27.1. Al iniciar una sesión

1. Leer AGENTS.md.
2. Leer README.md.
3. Revisar git status.
4. Identificar la rama actual.
5. Revisar los últimos cambios relevantes.
6. Inspeccionar los módulos afectados.
7. Entender el estado actual del proyecto.

### 27.2. Al desarrollar

Trabajar en cambios pequeños. Preferir vertical slices.

Cada slice debe cubrir, cuando corresponda: Dominio. Aplicación. Infraestructura. API. Frontend. Tests.

No generar toda la arquitectura de una vez.

### 27.3. Al encontrar problemas

Si encontrás un defecto:

- Reproducirlo mediante un test.
- Identificar la causa.
- Implementar una solución mínima.
- Agregar regresión.
- Verificar que no afecte otros módulos.

### 27.4. Al finalizar

Informar siempre:

```
TAREA:
[Descripción]

BOUNDED CONTEXT:
[Módulo]

CAMBIOS:
[Archivos y funcionalidades]

DECISIONES:
[Decisiones relevantes]

TESTS:
[Tests agregados]

RESULTADOS:
[Cantidad y resultado]

BUILD:
[Resultado]

RIESGOS:
[Problemas conocidos]

DEUDA TÉCNICA:
[Deuda detectada]

PRÓXIMO PASO:
[Siguiente tarea]
```

### 27.5. No avanzar autónomamente en decisiones críticas

Solicitar aprobación para:

- Cambiar arquitectura.
- Incorporar infraestructura importante.
- Cambiar tecnologías.
- Eliminar módulos.
- Modificar reglas fundamentales del negocio.
- Cambiar políticas de gratuidad.
- Modificar datos productivos.
- Introducir costos externos.
- Realizar migraciones destructivas.

## 28. Primera instrucción de ejecución

La primera tarea consiste exclusivamente en realizar DISCOVERY.

No implementar funcionalidades.
No generar cientos de archivos.
No instalar dependencias sin necesidad.
No iniciar el desarrollo del backend ni del frontend.

Analizar el proyecto y producir:

1. Diagnóstico del repositorio.
2. Visión funcional.
3. Actores.
4. Casos de uso.
5. Event Storming.
6. Bounded Context Map.
7. Modelo de dominio.
8. Agregados y Value Objects.
9. Reglas de negocio.
10. Arquitectura.
11. Estructura de proyectos.
12. Modelo de datos.
13. Contratos API iniciales.
14. Estrategia TDD.
15. Estrategia de seguridad.
16. Diseño UX/UI.
17. Roadmap MVP.
18. Riesgos y decisiones pendientes.

Documentar todo dentro de `/docs`.

Al finalizar, presentar un resumen ejecutivo y esperar aprobación explícita.
No pasar a la Etapa 1 hasta recibir aprobación.

## 29. Invariantes técnicas

Trampas ya pagadas en otros proyectos. Aplican a todo agente. Detalle en `.claude/rules/`.

- **Tracker:** GitHub Issues. Rama desde `origin/mainline`, un commit por issue con `#<issue>`, PR con `Closes #<issue>`.
- **EF Core:** `NoTracking` global y `.AsTracking()` explícito en lo que se guarda.
- **Fechas:** UTC en base y backend, zona solo en el borde, id IANA. Filtro por día con rango `[inicio, fin)` en UTC. Nunca `DateTime.Now`.
- **Enums persistidos:** nunca reusar ni renumerar valores.
- **Orden por reciente:** por columna secuencial, nunca por timestamp.
- **Claims:** leerlos del principal en cada acceso, nunca en el constructor.
- **Rate limiting:** particionado, después de `UseAuthorization`, con forwarded headers tras proxy.
- **Estado en memoria de proceso:** asume una réplica; decidirlo explícito al introducirlo.
- **Seguridad:** rotación de refresh tokens con detección de reuso; sesión validada contra la tabla de sesiones; controles con config faltante fallan cerrado; usuarios se bloquean, no se borran; tokens nunca en query string.
- **Tests:** fechas fijas, nunca `UtcNow`; todo test de autorización con su contraparte positiva; config de integración por variables de entorno; una BD por corrida con guard de borrado.
- **Verificación:** un chequeo informado como verde por otro agente se vuelve a correr antes de construir sobre él. Lo no ejecutado no cuenta como verde.
- **Docs de agente:** regla y trampa en presente, sin fecha, sin conteos de tests.

## Principio final

Construir una plataforma que sea técnicamente excelente, pero principalmente útil para las personas.

La tecnología debe facilitar las conexiones, no convertirse en una barrera.

El éxito del proyecto no se mide por la cantidad de funcionalidades implementadas, sino por cuántas personas pueden ofrecer sus habilidades, encontrar oportunidades y conectarse sin tener que pagar por ello.

Conectar personas. Dar visibilidad al talento. Democratizar las oportunidades.
