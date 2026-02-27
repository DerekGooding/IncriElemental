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

### PHASE 2: Core Loop & State Management (Completed)
- [x] Implement `GameState` class in `IncriElemental.Core` (Serializable)
- [x] Implement `GameLogic` for mana accumulation and discovery
- [x] Basic Unit tests for the gathering loop
- [x] Save/Load system implemented and verified
- [x] Automation (Rune of Attraction) and Architectural Evolution (Altar/Forge)

### PHASE 3: Visuals, Automation & Agentic Piloting (Completed)
- [x] Proper Font support and UI rendering
- [x] Headless Driver for agentic piloting
- [x] Balance Simulation for long-term progression
- [x] GitHub CI setup and Agentic DevOps workflow

### PHASE 4: Polishing & Expansion (Completed)
- [x] Implement actual sprites and particle variations for elements.
- [x] Add narrative milestones (Story log).
- [x] Polish the UI transitions and animations.

### PHASE 5: The Forge & Refactoring (Current)
- [ ] **Refactor `GameEngine.cs`**: Extract logic into `ManifestationManager` and `AutomationSystem` to stay under 250 lines.
- [ ] **Tool Logic**: Implement "Pickaxe" (Click Power+) and "Focus Crystal" (Passive+).
- [ ] **Balance Check**: Verify the first 5 minutes of gameplay using `scripts/check_health.py` and `BalanceSimulator.cs`.

### PHASE 6: Elemental Mastery & Mid-Game
- [ ] **Advanced Structures**: Implement "Well" (Water Gen) and "Brazier" (Fire Gen).
- [ ] **Combinations**: Add logic for combining elements (Earth+Water=Clay).
- [ ] **Visuals**: New sprites for tools and advanced structures.

### PHASE 7: The Spire & Endgame
- [ ] **The Great Project**: Implement multi-stage construction logic.
- [ ] **Ascension**: Define the win condition and trigger the end credits scene.
- [ ] **Final Polish**: Ensure the entire 20-minute loop is engaging and bug-free.

---
*Next Step: Create initial documentation in `docs/` and basic scripts.*
