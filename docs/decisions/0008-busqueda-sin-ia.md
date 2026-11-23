# ADR-0008: Búsqueda en lenguaje natural sin IA

Estado: aceptada · 2026-11-23

**Contexto.** AGENTS.md §12.1 pide entender frases como "electricista en Don Torcuato"; §4 deja la IA de matching fuera del MVP.

**Problema.** Interpretar servicio, ubicación y modalidad sin modelos de lenguaje.

**Alternativas.** Solo FTS · LLM · parser por diccionario + FTS.

**Decisión.** Parser determinístico en Discovery: normaliza el texto, busca sinónimos de especialidades (Catalog), localidades (Geography, con `pg_trgm`) y palabras de modalidad; lo no reconocido va a FTS en español. La interpretación se devuelve a la UI como chips editables.

**Justificación.** Sin costo, explicable, testeable con un set fijo de frases, transparente para el usuario.

**Consecuencias.** La calidad depende del diccionario de sinónimos: el admin lo mantiene. Se mide con un set de consultas reales (gate de Etapa 4).
