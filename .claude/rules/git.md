# Git y GitHub

- Rama base: `mainline`. Toda rama nueva sale de `origin/mainline` actualizado. Con squash merge, una rama que sale de otra ya mergeada nace con conflictos.
  ```bash
  git checkout mainline && git pull && git checkout -b <tipo>/<issue>-<slug>
  git merge-base --is-ancestor origin/mainline HEAD && echo "al día"
  ```
- No encadenar el `checkout mainline` con `2>/dev/null ||`: si hay cambios sin commitear falla en silencio y la rama sale de otro lado.
- Si la rama ya arrastra commits mergeados: `git rebase --onto origin/mainline <ultimo-commit-viejo>`.
- No pushear a una rama con PR mergeado: el delta va en un PR nuevo.
- Borrar una rama mergeada: `git branch -d` la rechaza por el squash. Comparar `git diff mainline <rama>` y recién ahí `-D`.
- El `executor` commitea local, un commit por issue; el push y el PR son del agente `pr`. Executors en paralelo: cada uno en su propio `git worktree`, nunca dos en el mismo checkout.
- Un PR por pedido, un commit por issue con `#<issue>`. El PR cierra el issue con `Closes #<issue>` en la descripción.
- Conventional Commits; el mensaje describe el problema resuelto, no la acción.
- Stagear archivos explícitos, nunca `git add -A`. Nunca `.env` reales, secretos ni artefactos de build.
