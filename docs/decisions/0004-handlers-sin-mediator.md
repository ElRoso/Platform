# ADR-0004: Handlers propios, sin librería de mediator

Estado: aceptada · 2026-11-23

**Contexto.** Casos de uso como commands y queries. MediatR pasó a licencia comercial. El proyecto es AGPL y no usa nada de pago.

**Problema.** Despachar casos de uso sin depender de una librería con riesgo de licencia.

**Alternativas.** MediatR (pago) · Wolverine · handlers propios.

**Decisión.** Interfaces `ICommandHandler<TCommand, TResult>` e `IQueryHandler<TQuery, TResult>` en BuildingBlocks, registro por escaneo, endpoint invoca al handler inyectado. Cross-cutting con decoradores.

**Justificación.** Pocas líneas, explícito, sin magia ni dependencia externa. El endpoint muestra qué handler llama.

**Consecuencias.** Sin pipeline behaviors listos: los decoradores se escriben a mano cuando hacen falta.
