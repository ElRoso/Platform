---
paths:
  - "backend/**"
---

# Backend .NET

## Invariantes
- EF Core: `QueryTrackingBehavior.NoTracking` global; `.AsTracking()` explícito en toda lectura que se va a guardar. Sin eso EF ignora la mutación en silencio.
- Fechas: UTC en base y backend; conversión de zona solo en el borde. Nunca `DateTime.Now` ni `ToLocalTime`. Zona siempre por id IANA (`America/Argentina/Buenos_Aires`): un id de Windows no existe en Linux y da 500.
- Filtro por día: rango `[inicio, fin)` en UTC. Nunca `.Date` ni `.AddDays` sobre un `DateTime` sin convertir.
- Enums persistidos: nunca reusar ni renumerar valores. Una fila vieja resucita con otro significado.
- Orden "el más reciente": por columna secuencial (identity), nunca por timestamp. Dos cambios seguidos comparten instante.
- Claims del usuario: leerlos del principal vivo en cada getter, nunca en el constructor (queda cacheado vacío para todo el request).
- Un valor que decide estado, permisos o propiedad se lee de la base, nunca del request.
- Mapper sin I/O. Lectura no normaliza; escritura sí.
- Estado en memoria de proceso (`IMemoryCache`, locks, contadores) asume una sola réplica. Decidirlo explícito al introducirlo.

## Rate limiting
- Todo limiter particionado.
- `UseRateLimiter` va después de `UseAuthorization`: antes `User` está vacío y todo cae a la partición por IP.
- Tras un proxy, sin `ForwardedHeaders` la partición por IP no hace nada. `KnownNetworks`/`KnownProxies` explícitos si la red docker no es RFC1918.

## Validación
- Test de arquitectura que rompa si un `AbstractValidator<T>` apunta a un tipo que ningún endpoint usa.
- Un validator mockeado sin stub devuelve `null` y revienta: stubear siempre `ValidateAsync`.

## Build y formato
- Build Release y `dotnet format --verify-no-changes` dan 0 en `mainline`: cualquier warning o diagnóstico es del cambio propio.
- Editar un `.cs` anclando por la firma del método, no por sus últimas líneas: hay métodos que terminan igual.
- Renombrar una clave de config deja la nueva vacía en entornos ya desplegados: el control queda apagado en silencio. Fallar al arrancar si falta.
