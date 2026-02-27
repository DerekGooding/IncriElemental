# PLAN.md - IncriElemental Development Plan

This document tracks the active, evolving work plan for **IncriElemental**.

## CURRENT GOAL: Project Scaffolding & Initial Game State
Set up the basic infrastructure and a simple "unfolding" loop (e.g., Gather Mana -> Form First Elemental Spark).

### PHASE 1: Infrastructure (In Progress)
- [x] Solution & Project Scaffolding (.NET 10.0, MonoGame)
- [x] Core <-> Desktop <-> Tests references
- [x] GEMINI.md & README.md initialization
- [ ] Initial `docs/architecture.md` and `docs/game_design.md`
- [ ] Basic build & validation scripts (`scripts/build.ps1`, `scripts/validate.ps1`)

### PHASE 2: Core Loop & State Management
- [ ] Implement `GameState` class in `IncriElemental.Core` (Serializable)
- [ ] Implement `GameLogic` for mana accumulation and discovery
- [ ] Basic Unit tests for the gathering loop

### PHASE 3: Visuals & MonoGame Integration
- [ ] Simple UI system for displaying resources and actions (buttons/text)
- [ ] State -> View mapping (unfolding logic)
- [ ] Initial rendering of "The Void" (dark screen with some text)

---
*Next Step: Create initial documentation in `docs/` and basic scripts.*
