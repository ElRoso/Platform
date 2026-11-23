# ADR-0009: Licencia AGPL-3.0 y dependencias solo OSS

Estado: aceptada · 2026-11-23

**Contexto.** Plataforma gratuita para siempre (AGENTS.md §2.3), publicada como open source.

**Problema.** Evitar que un fork se ofrezca como servicio cerrado o pago sin devolver su código, y evitar dependencias que impongan costos.

**Alternativas.** MIT · Apache 2.0 · AGPL-3.0.

**Decisión.** AGPL-3.0-only. Dependencias solo con licencias OSI compatibles (MIT, BSD, Apache 2.0, LGPL, MPL 2.0, GPL/AGPL 3). Prohibidas las de pago o con licencia comercial o dividida según uso.

**Justificación.** La AGPL obliga a publicar el código de versiones modificadas que se ofrezcan por red, coherente con "sin costos ocultos".

**Consecuencias.** Revisión de licencia por versión exacta al agregar un paquete. Algunas empresas evitan contribuir a AGPL: costo aceptado.
