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

## 24. Goal: The Final Polish (Completed)
- [x] **Tutorialization:** Add a guided first-time experience for new players.
- [x] **Visual Polish:** Add screen-shake and transition effects for Ascension.
- [x] **Localization:** Externalize all strings for multi-language support.

## 25. Goal: Visual Overhaul: "The Aetherial Glow" (Completed)
- [x] **HLSL Post-Processing:** Implement Bloom, Chromatic Aberration, and Aether Wave shaders.
- [x] **Procedural Background:** Create a dynamic Aether Nebula background reactive to game state.
- [x] **Interactive Parallax:** Implement multi-layered background movement tied to mouse/input.

## 26. Goal: Mechanical Depth: "Resonant Harmony" (Completed)
- [x] **Grid Synergies:** Implement an "Aura" system where manifestation placement on the map affects production.
- [x] **Alchemical Mixing Table:** A dedicated mini-game/UI for active multi-element reactions.
- [x] **Reactive Feedback:** Visual indicators for stability and harmony on the map and mixing table.

## 27. Goal: Architecture: "Component-Driven Manifestations" (Completed)
- [x] **Data-Driven Components:** Refactor manifestations into a modular component-based system (ECS-lite).
- [x] **Event-Driven UI:** Transition UI updates from polling to an observer pattern using the `EventBus`.
- [x] **Polymorphic Serialization:** Implement custom JSON converters for component-based manifestation data.

## 28. Goal: UI/UX: "The Wizard's Interface" (Planned)
- [ ] **Rich Text & Icons:** Implement a markdown-lite parser for tooltips and logs with inline icons.
- [ ] **Resource Flow Graphs:** Add a visual "Flow" tab to trace resource production chains.
- [ ] **Power User Controls:** Implement hotkeys, tooltip pinning, and manual UI scaling.

---
*Last Updated: Thursday, March 12, 2026 (Updated by Agent Gemini)*
