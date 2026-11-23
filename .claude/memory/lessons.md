---
name: lessons
description: Correcciones del dueño en formato "Cuando X, hacer Y". Leer antes de arrancar cualquier tarea.
metadata:
  type: feedback
---

# Lessons
<!-- Lo más reciente arriba. Borrar lo que ya no aplique. Mismo error dos veces: reescribir la línea. -->
- Cuando se arranca una tarea, mapear con agentes de exploración y seguir análisis → plan → implementación → verificación; no trabajar directo.
- Cuando un agente informa un chequeo en verde (react-doctor, lint, tests), correrlo uno mismo antes de seguir: hubo informes "limpio" con avisos reales.
- Cuando un pedido toca backend y frontend, terminar backend en verde antes de arrancar frontend y armarlo con el contrato real; en paralelo la PC se satura.
- Cuando el dueño va a probar un cambio de backend en local, reiniciar el server de dev antes de avisar: `dotnet run` no recarga.
- Cuando react-doctor reporta cualquier aviso, aunque no frene el push: frenar, corregirlo con test y recién ahí pushear.
- Cuando un PR ya está mergeado, el delta va en un PR nuevo; nunca commits tardíos sobre esa rama.
