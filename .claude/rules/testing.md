# Tests

## Backend (.NET)
- Integración con `WebApplicationFactory`: config por variables de entorno (`Section__Key`), no `ConfigureAppConfiguration`. `Program` lee config antes de `builder.Build()` y la otra vía llega tarde (síntoma: "X is required" al arrancar).
- Token de test con todos los claims que exige el pipeline. Todo test de autorización lleva su contraparte positiva: si no, un 403 por claim faltante pasa por el motivo equivocado.
- Fechas de test: día calendario fijo, nunca `UtcNow`. Cerca de medianoche AR el rango salta de día.
- BD de integración por corrida: nombre `platform_it_<utc>_<guid8>`, borrada al final con `DROP DATABASE ... WITH (FORCE)`. Guard que se niega a borrar nombres fuera de ese formato. Purga de las de más de 24 h. Usuario de test con `CREATEDB` y nada más.
- BD compartida entre tests: sembrar datos propios por test (ids nuevos) y respetar los índices únicos del seed.
- Testcontainers en paralelo agotan RAM: suites de integración de a una. `OutOfMemoryException` o `MSB4018` es entorno, no el cambio.
- `Moq.Verify` sobre firmas con opcionales: todos los argumentos o nombrados. Con menos posicionales el predicado cae en otro parámetro.
- Cobertura: gate sobre Domain y Application (excluir controllers, composition root, clientes de I/O). El `<Threshold>` del runsettings no hace nada con el colector XPlat: el gate real es un paso del CI que parsea `line-rate` de `coverage.cobertura.xml`.
- Cambio que suma o saca algo que un test cuenta (endpoints, handlers, tipos registrados): correr la suite de integración entera antes del push.
- Al borrar o migrar código, migrar sus tests. Si cambia de qué depende un método, revisar los tests que siguen en verde: pueden pasar por el motivo equivocado.
- Validar un test con un mutante: confirmar que cae su caso y solo ese. Revertir reescribiendo el archivo (con `mv` MSBuild puede dejar el binario mutado por mtimes).

## Contratos front/back
- Enums espejados entre backend y frontend: test de snapshot JSON en el backend (regenerable con variable de entorno) para detectar deriva.
- Cambio de contrato: aditivo primero, deploy backend antes que frontend.
