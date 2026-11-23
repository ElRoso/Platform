# Código concreto y legible

El código hace lo que pide el issue y se lee de arriba abajo sin saltar. Lo que no aporta a eso, no entra.

## No se agrega
- Abstracción sin segundo uso: interfaz con una sola implementación (salvo puertos que exige la regla de dependencias de AGENTS.md §5.4), helper de un solo llamado, clase base "por si acaso".
- Parámetros, flags, overloads u opciones que nadie usa hoy.
- Wrappers que solo renombran o delegan, y variables intermedias que solo repiten el valor.
- Defensivas imposibles: null check sobre no-nullable, `try/catch` que relanza igual, validar dos veces lo que ya validó otra capa (Application valida, Domain protege invariantes: eso no es duplicar).
- Código muerto o comentado, logs de depuración, `TODO` sin `#<issue>`.
- Constantes, enums o configuración para un valor que no cambia ni se reusa.

## Se prefiere
- Retorno temprano antes que anidar.
- Una función, un nivel de abstracción; si hay que explicar el bloque con un comentario, va a una función con nombre.
- Nombres que dicen qué es, sin relleno (`Manager`, `Helper`, `Data`, `Info`, `Util`).
- Reusar lo que ya existe en el repo antes de escribir algo parecido (buscar primero).
- En tests, los builders y fixtures existentes; sin setup copiado entre tests.

## Medida
- Antes de cerrar un paso, releer el diff: cada línea nueva tiene que ser necesaria para el comportamiento o su test. Lo que no, se borra.
- Si el diff suma más estructura que lógica, decirlo en el informe con el por qué.

Comentarios: `comments.md`.
