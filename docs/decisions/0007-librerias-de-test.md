# ADR-0007: Librerías de test OSS

Estado: aceptada · 2026-11-23

**Contexto.** AGENTS.md §17.2 lista FluentAssertions y "NSubstitute o Moq". FluentAssertions 8+ tiene licencia comercial. El proyecto no usa nada de pago.

**Problema.** Elegir assertions y dobles sin riesgo de licencia.

**Alternativas.** FluentAssertions 7.x (congelado) · AwesomeAssertions · Shouldly · Moq · NSubstitute.

**Decisión.** xUnit v3, AwesomeAssertions, NSubstitute, Testcontainers, NetArchTest.eNhancedEdition.

**Justificación.** AwesomeAssertions es el fork Apache 2.0 de FluentAssertions con la misma API y mantenimiento activo. NSubstitute (BSD-3) evita la controversia de telemetría de Moq.

**Consecuencias.** AGENTS.md §17.2 se actualizó a AwesomeAssertions.
