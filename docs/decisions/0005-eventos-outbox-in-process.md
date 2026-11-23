# ADR-0005: Eventos de integración in-process con outbox

Estado: aceptada · 2026-11-23

**Contexto.** Los módulos se comunican por eventos (Profiles → Discovery, Contact → Notifications). Un solo proceso.

**Problema.** Garantizar que un evento se publique si y solo si el cambio que lo origina se guardó.

**Alternativas.** Publicar en memoria tras `SaveChanges` · broker externo (RabbitMQ) · outbox en Postgres con despacho in-process.

**Decisión.** Cada módulo escribe sus eventos de integración en `outbox_message` de su schema en la misma transacción. Un `BackgroundService` los despacha a los handlers suscriptos; cada consumidor registra `inbox_message` para procesar una sola vez.

**Justificación.** Atomicidad sin infraestructura extra. El contrato de eventos queda listo para un broker futuro.

**Consecuencias.** Latencia de segundos entre módulos. El despachador asume una réplica (o lock por fila con `FOR UPDATE SKIP LOCKED` al escalar).
