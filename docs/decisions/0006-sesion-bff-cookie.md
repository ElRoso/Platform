# ADR-0006: Sesión web en cookie httpOnly vía BFF Next.js

Estado: aceptada · 2026-11-23

**Contexto.** AGENTS.md §10.3 prohíbe información sensible en `localStorage`. Frontend Next.js con Server Components.

**Problema.** Dónde viven los tokens de sesión del usuario.

**Alternativas.** JWT en `localStorage` · cookie emitida por la API en otro origen · BFF: Next.js guarda los tokens en cookie httpOnly y llama a la API del lado servidor.

**Decisión.** BFF. Cookie `__Host-` `HttpOnly`, `Secure`, `SameSite=Lax`. El browser nunca ve el access token. La API valida bearer tokens y no expone CORS a otros orígenes.

**Justificación.** Mitiga robo de tokens por XSS, encaja con Server Components (fetch del lado servidor), CORS simple.

**Consecuencias.** La Web es parte del perímetro de seguridad: rotación de refresh y CSRF (verificación de `Origin`) se implementan en el BFF.
