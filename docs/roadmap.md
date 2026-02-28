# ROADMAP.md - IncriElemental Task History

This document tracks the long-term goals and task history for **IncriElemental**.

## 20. Goal: Technical Perfection & Immersion (Completed)
- [x] **UI Pagination:** Implement a tab system (Void / Spire / World / Constellation / Debug) to manage UI density.
- [x] **Dynamic Layout:** Implement relative positioning (anchors/margins) via `UiLayout` to support various resolutions.
- [x] **Procedural Atmosphere:** Implement a minimalist background "void hum" using dynamic sound generation.
- [x] **CI Font Portability:** Replace the system-font hack in `ci.yml` with the portable download script.
- [x] **Scrollable Viewport:** Add a vertical scrolling system for buttons using `ScissorRectangle` clipping.

## 21. Goal: Architectural Decoupling & Stability (Completed)
- [x] **EventBus:** Implement a lightweight event system for `ResourceGained`, `ThingManifested`, etc.
- [x] **Save Migration:** Add versioning to `GameState` and a migration layer to `SaveManager`.
- [x] **Refactoring:** Split `Game1.cs` into specialized systems (`StatusSystem`, `EndingSystem`, `AiModeSystem`) to satisfy strict monolith constraints.

## 22. Goal: Deep Unfolding & Synergies (Completed)
- [x] **The Constellation:** Implement a prestige tree using "Void Embers" earned upon Ascension.
- [x] **Landmark Exploration:** Add fixed coordinates on the map with unique narrative and manifestations.
- [x] **Elemental Synergies:** Expand `AlchemySystem` with long-term structures like "Magma Forge" and "Clouds."

## 23. Goal: Advanced Agentic Tooling (Completed)
- [x] **Efficiency Observer:** A debug UI for the agent to visualize resource bottlenecks.
- [x] **Hover Tooltips:** Dynamic feedback for all manifestations explaining their ongoing effect and total scaling.

## 24. Goal: The Final Polish (Next)
- [ ] **Tutorialization:** Add a guided first-time experience for new players.
- [ ] **Visual Polish:** Add screen-shake and transition effects for Ascension.
- [ ] **Localization:** Externalize all strings for multi-language support.

---
*Last Updated: Friday, February 27, 2026*
