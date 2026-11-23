# Event Storming inicial

Etapa 0 (AGENTS.md §28, punto 5). Nivel "big picture" + process level de los flujos MVP.

Convención:
- **Comando** (azul): intención, imperativo. `RegisterUser`.
- **Evento** (naranja): hecho pasado. `UserRegistered`.
- **Política** (lila): "cuando X, entonces Y".
- **Read model** (verde): lo que alguien mira para decidir.
- Prefijo de módulo en eventos de integración: `Identity.UserRegistered`.

Eventos **de dominio** quedan dentro del módulo. Eventos **de integración** cruzan módulos y forman parte del contrato público (ver `BOUNDED-CONTEXTS.md`). Marcados con ⇄.

## 1. Identidad

```mermaid
flowchart LR
  C1[RegisterUser]:::cmd --> E1([UserRegistered ⇄]):::evt
  E1 --> P1{{Enviar email de verificación}}:::pol
  P1 --> C2[VerifyEmail]:::cmd --> E2([EmailVerified ⇄]):::evt
  C3[SignIn]:::cmd --> E3([UserSignedIn]):::evt
  C3 --> E3b([SignInFailed]):::evt
  E3b --> P2{{N fallos → bloqueo temporal}}:::pol
  C4[RefreshSession]:::cmd --> E4([SessionRefreshed]):::evt
  C4 --> E4b([RefreshTokenReuseDetected]):::evt
  E4b --> P3{{Revocar familia de sesión}}:::pol --> E5([SessionRevoked]):::evt
  C6[RequestPasswordReset]:::cmd --> E6([PasswordResetRequested]):::evt
  C7[ResetPassword]:::cmd --> E7([PasswordChanged]):::evt
  E7 --> P4{{Revocar todas las sesiones}}:::pol
  C8[LockUser]:::cmd --> E8([UserLocked ⇄]):::evt
  classDef cmd fill:#a7c7e7,stroke:#333,color:#000
  classDef evt fill:#ffb347,stroke:#333,color:#000
  classDef pol fill:#cdb4db,stroke:#333,color:#000
```

## 2. Perfil profesional

```mermaid
flowchart LR
  C1[CreateProfessionalProfile]:::cmd --> E1([ProfessionalProfileCreated]):::evt
  C2[UpdateProfessionalProfile]:::cmd --> E2([ProfessionalProfileUpdated ⇄]):::evt
  C3[PublishProfessionalProfile]:::cmd --> E3([ProfessionalProfilePublished ⇄]):::evt
  C4[PauseProfessionalProfile]:::cmd --> E4([ProfessionalProfilePaused ⇄]):::evt
  C5[ResumeProfessionalProfile]:::cmd --> E3
  C6[AddPortfolioItem]:::cmd --> E6([PortfolioItemAdded]):::evt
  E3 --> P1{{Indexar en búsqueda}}:::pol
  E2 --> P1
  E4 --> P2{{Quitar de búsqueda}}:::pol
  X1([Moderation.ProfileSuspended ⇄]):::evt --> P3{{Suspender perfil}}:::pol --> E7([ProfessionalProfileSuspended ⇄]):::evt
  E7 --> P2
  X2([Identity.UserLocked ⇄]):::evt --> P3
  classDef cmd fill:#a7c7e7,stroke:#333,color:#000
  classDef evt fill:#ffb347,stroke:#333,color:#000
  classDef pol fill:#cdb4db,stroke:#333,color:#000
```

Read models: "Mi perfil profesional" (borrador con checklist de requisitos faltantes), "Perfil público".

## 3. Búsqueda

Búsqueda es solo lectura: no emite eventos de dominio. Consume eventos para mantener su índice.

```mermaid
flowchart LR
  X1([Profiles.ProfessionalProfilePublished ⇄]):::evt --> P1{{Upsert documento de búsqueda}}:::pol
  X2([Profiles.ProfessionalProfileUpdated ⇄]):::evt --> P1
  X3([Profiles.ProfessionalProfilePaused ⇄]):::evt --> P2{{Quitar documento}}:::pol
  X4([Profiles.ProfessionalProfileSuspended ⇄]):::evt --> P2
  X5([Catalog.CategoryChanged ⇄]):::evt --> P3{{Reindexar perfiles afectados}}:::pol
  P1 --> R1[(SearchDocument)]:::rm
  Q[SearchProfessionals]:::cmd --> R1
  classDef cmd fill:#a7c7e7,stroke:#333,color:#000
  classDef evt fill:#ffb347,stroke:#333,color:#000
  classDef pol fill:#cdb4db,stroke:#333,color:#000
  classDef rm fill:#b5e48c,stroke:#333,color:#000
```

## 4. Contacto

```mermaid
flowchart LR
  C1[CreateContactRequest]:::cmd --> E1([ContactRequestCreated ⇄]):::evt
  C1 --> E1b([ContactRequestRejectedByPolicy]):::evt
  E1 --> P1{{Notificar al profesional por email}}:::pol
  C2[MarkContactRequestViewed]:::cmd --> E2([ContactRequestViewed]):::evt
  C3[RespondContactRequest]:::cmd --> E3([ContactRequestResponded ⇄]):::evt
  E3 --> P2{{Notificar al solicitante}}:::pol
  C4[CloseContactRequest]:::cmd --> E4([ContactRequestClosed ⇄]):::evt
  C5[CancelContactRequest]:::cmd --> E5([ContactRequestCancelled]):::evt
  X1([Profiles.ProfessionalProfilePaused ⇄]):::evt --> P3{{Rechazar nuevas solicitudes}}:::pol
  classDef cmd fill:#a7c7e7,stroke:#333,color:#000
  classDef evt fill:#ffb347,stroke:#333,color:#000
  classDef pol fill:#cdb4db,stroke:#333,color:#000
```

`ContactRequestRejectedByPolicy` cubre: destinatario pausado o suspendido, duplicado abierto, límite diario superado, solicitante bloqueado.

## 5. Favoritos

`AddFavorite` → `FavoriteAdded`; `RemoveFavorite` → `FavoriteRemoved`. Política: cuando `Profiles.ProfessionalProfileSuspended`, el favorito se mantiene pero la card muestra "no disponible".

## 6. Moderación

```mermaid
flowchart LR
  C1[ReportContent]:::cmd --> E1([ContentReported]):::evt
  E1 --> P1{{Abrir o sumar a caso existente del mismo objetivo}}:::pol --> E2([ModerationCaseOpened]):::evt
  C2[ResolveCase]:::cmd --> E3([ModerationCaseResolved]):::evt
  C2 --> E4([ProfileSuspended ⇄]):::evt
  C2 --> E5([ContentHidden ⇄]):::evt
  C6[LiftSuspension]:::cmd --> E6([ProfileSuspensionLifted ⇄]):::evt
  E3 --> P2{{Registrar auditoría}}:::pol
  classDef cmd fill:#a7c7e7,stroke:#333,color:#000
  classDef evt fill:#ffb347,stroke:#333,color:#000
  classDef pol fill:#cdb4db,stroke:#333,color:#000
```

## 7. Puntos calientes (hotspots)

| # | Hotspot | Dónde se resuelve |
|---|---|---|
| H1 | ¿Qué "interacción elegible" habilita una valoración? | D14, fuera del MVP |
| H2 | ¿Un visitante sin cuenta puede enviar solicitud interna? | D15: no; usa canales públicos |
| H3 | ¿Qué pasa con solicitudes `Pending` cuando el perfil se pausa? | BR-067: se mantienen; el profesional puede responderlas |
| H4 | Interpretación de consulta natural sin IA | BR-050, `ARCHITECTURE.md` §Búsqueda |
| H5 | Spam de solicitudes en una plataforma gratuita | BR-063..065, `SECURITY.md` |
| H6 | Suspensión por moderación vs pausa voluntaria | Estados separados; BR-028 |
