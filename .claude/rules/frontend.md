---
paths:
  - "frontend/**"
---

# Frontend Next.js

- Chequeos obligatorios antes del PR: `npx tsc --noEmit`, lint, vitest y gate de cobertura. Los comandos exactos los fija el `tester` y el CI.
- `npx react-doctor --yes --no-score --scope changed --base origin/mainline`: cualquier aviso bloquea el push, aunque el hook lo deje pasar. Se corrige con test.
- Un "react-doctor limpio" o "lint verde" informado por un agente se vuelve a correr antes de seguir.
- Revertir lo que `next dev` agrega solo (`next-env.d.ts`, bloques en `CLAUDE.md`) antes de commitear.
- Los tipos de la API salen del contrato del backend; un enum espejado tiene su snapshot en el backend (`testing.md`).
