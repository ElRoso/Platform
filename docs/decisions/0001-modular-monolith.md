# ADR-0001: Modular monolith

Estado: aceptada · 2026-11-23

**Contexto.** Producto nuevo, un equipo chico, MVP enfocado en conexiones reales. AGENTS.md §5.1 pide no arrancar con microservicios.

**Problema.** Separar responsabilidades por dominio sin pagar el costo operativo de servicios distribuidos.

**Alternativas.** Monolito en capas sin módulos · microservicios · modular monolith.

**Decisión.** Modular monolith: un proceso, un deploy, una BD; diez módulos con proyectos propios y contratos explícitos (`*.Contracts`).

**Justificación.** Aislamiento de dominio verificable con tests de arquitectura, refactor barato, operación simple. Permite extraer un módulo a servicio si alguna vez hace falta.

**Consecuencias.** Disciplina de límites obligatoria (tests de arquitectura en CI). Un fallo de un módulo puede afectar al proceso entero. Escalado vertical primero.
