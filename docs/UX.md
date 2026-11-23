# UX/UI

Etapa 0 (AGENTS.md §28, punto 16). Base: AGENTS.md §11, §12, §16.

## Principios

1. **Mobile first real.** Se diseña a 360 px; desktop agrega columnas, no funciones.
2. **Una acción principal por pantalla.** Buscar, publicar, contactar.
3. **Lenguaje de la calle.** "¿Qué necesitás?" en vez de "Ingrese criterio de búsqueda". Voseo.
4. **Alfabetización digital diversa.** Textos cortos, íconos con etiqueta, sin jerga, formularios de a un paso.
5. **Nada se pierde.** Borradores automáticos en "Publicar"; errores que dicen cómo seguir.
6. **Sin presión comercial.** Sin banners, destacados ni "premium".

## Tokens de diseño

Definidos como variables CSS y mapeados a Tailwind. Valores iniciales, ajustables en Etapa 1.

| Token | Valor | Uso |
|---|---|---|
| `--color-primary` | Verde azulado accesible (contraste ≥ 4.5:1 sobre blanco) | Acción principal |
| `--color-accent` | Ámbar | Favoritos, resaltados |
| `--color-surface` / `--color-surface-muted` | Blanco / gris muy claro | Fondos |
| `--color-text` / `--color-text-muted` | Casi negro / gris oscuro | Texto (contraste AA) |
| `--color-danger`, `--color-success`, `--color-warning` | — | Estados |
| Tema oscuro | Mismos tokens redefinidos | `prefers-color-scheme` |
| Tipografía | Inter o sistema; base 16 px, escala 14/16/18/20/24/30 | Legibilidad en móvil |
| Espaciado | Escala de 4 px (4, 8, 12, 16, 24, 32, 48) | — |
| Radios | 8 px (controles), 12 px (cards), full (chips, avatar) | — |
| Elevación | 2 niveles: card, modal/drawer | — |
| Breakpoints | `sm 640`, `md 768`, `lg 1024`, `xl 1280` | Mobile por defecto |
| Target táctil | mínimo 44 × 44 px | WCAG 2.2 (2.5.8) |
| Foco | Anillo 2 px `--color-primary` + offset 2 px | Siempre visible |

## Navegación

Móvil: barra inferior fija.

| Ítem | Visitante | Autenticado |
|---|---|---|
| Inicio | ✓ | ✓ |
| Buscar | ✓ | ✓ |
| Publicar | ✓ (lleva a registro con contexto) | ✓ |
| Favoritos | — (se muestra al iniciar sesión) | ✓ |
| Mi perfil / Ingresar | "Ingresar" | "Mi perfil" |

Desktop: header con buscador persistente y los mismos destinos.

## Pantallas MVP

Wireframes en texto. `[ ]` = control, `( )` = chip.

### Inicio
```
┌──────────────────────────┐
│ ¿Qué necesitás?          │
│ [Ej: electricista en …]🔍│
│ (Cerca mío) (Remoto)     │
│                          │
│ Categorías populares     │
│ [🔨 Construcción] [⚡ …] │
│                          │
│ ¿Sabés hacer algo?       │
│ [Publicá gratis]         │
└──────────────────────────┘
```

### Resultados
```
┌──────────────────────────┐
│ [electricista don torc]🔍│
│ Entendimos: (Electricista ✕) (Don Torcuato ✕) (+5 km) │
│ [Filtros ▾]  Orden: Relevancia ▾ │
│ ┌──────────────────────┐ │
│ │ 👤 Juan P. · Electric.│ │
│ │ Don Torcuato · 3 km  │ │
│ │ A domicilio · ♡      │ │
│ └──────────────────────┘ │
│ … [Ver más]              │
└──────────────────────────┘
```
Filtros en drawer inferior. Chips de interpretación editables (BR-050). Sin resultados: sugerencias (ampliar radio, quitar filtro, categoría similar).

### Perfil público
Header (foto, nombre, especialidades, localidad y radio), botones de contacto según lo publicado (WhatsApp principal si existe), descripción, portfolio en grilla, "Sin valoraciones todavía" nunca como 0 (cuando haya Reputation), reportar en menú secundario, compartir.

### Publicar (perfil profesional)
Stepper de 4 pasos, cada uno guardable:
1. ¿Qué hacés? → buscador de especialidades con sinónimos.
2. ¿Dónde trabajás? → modalidad + localidad base + radio (o "solo remoto").
3. ¿Cómo te contactan? → canales a publicar (cada uno opt-in) y si acepta solicitudes internas.
4. Contá sobre vos → descripción con contador, experiencia, foto opcional.

Resumen final con checklist de requisitos faltantes (`missingRequirements`) y botón "Publicar".

### Mis solicitudes
Tabs "Recibidas" / "Enviadas"; card con estado, fecha relativa, acciones según la máquina de estados.

### Favoritos, Mi perfil, Sesiones
Listas simples; estados vacíos con acción ("Todavía no guardaste a nadie. [Buscar]").

### Admin / Moderación
Desktop primero (excepción justificada: uso interno). Tabla de casos, detalle con historial y acciones con motivo obligatorio.

## Componentes base

AGENTS.md §11.4: `Button`, `Input`, `SearchInput`, `SearchFilters`, `ProfessionalCard`, `CategoryCard`, `ProfileHeader`, `RatingStars` (cuando haya Reputation), `LocationBadge`, `ContactButton`, `FavoriteButton`, `EmptyState`, `LoadingSkeleton`, `ErrorState`, `BottomNavigation`, `Modal`, `Drawer`, `Toast`, `Pagination`. Primitivas desde shadcn/ui; se crean a medida que una feature los usa.

## Estados obligatorios por pantalla

Loading (skeleton, no spinner a pantalla completa), vacío (con acción), error (mensaje + reintentar), sin conexión (mensaje), éxito (toast breve).

## Formularios

React Hook Form + Zod. Labels visibles (no solo placeholder), `inputmode` y `autocomplete` correctos (`tel`, `email`), errores bajo el campo y resumen al enviar, nunca borrar lo escrito ante error.

## Accesibilidad (WCAG 2.2 AA)

Landmarks semánticos, orden de foco lógico, foco visible, contraste AA, `aria-live` para resultados y toasts, alternativa textual en imágenes del portfolio (caption), navegación completa por teclado, sin depender solo del color, respeto de `prefers-reduced-motion`. Verificación con axe en E2E.

## SEO

| Ruta | Render | Indexable |
|---|---|---|
| `/` | SSG/ISR | Sí |
| `/profesionales/{categoria}` | ISR | Sí |
| `/profesionales/{categoria}/{provincia}` | ISR | Sí, si hay resultados (BR-056) |
| `/profesionales/{categoria}/{provincia}/{localidad}` | ISR | Sí, si hay resultados |
| `/profesional/{slug}` | SSR/ISR | Sí, si `Published` |
| `/buscar?…` | SSR | No (`noindex`, canonical a la landing más cercana) |
| Rutas autenticadas | CSR/SSR | No |

Metadata dinámica, Open Graph con foto o imagen por categoría, `sitemap.xml` generado por etapas, `robots.txt`, canonical, breadcrumbs y JSON-LD (`Person` + `Service`, `BreadcrumbList`).

## Performance percibida

LCP < 2,5 s móvil: imágenes con `next/image`, tamaños responsivos, WebP; fuentes con `font-display: swap`; JS mínimo en páginas públicas (Server Components).
