# ARCHITECTURE.md - IncriElemental Design Patterns

This document defines the system design, data flow, and class hierarchies for **IncriElemental**.

## 1. Project Organization
The project is split into three main layers:

| Layer | Responsibility |
|---|---|
| **Core (Shared)** | Game state, logic, formulas, serialization. |
| **Desktop (Runner)** | MonoGame `Game1.cs`, rendering, input, assets. |
| **Tests (Validation)** | Unit tests for Core logic. |

## 2. Game State and Persistence
The `GameState` class in `IncriElemental.Core` is the single source of truth.
- **Serialization:** System.Text.Json is used for save games.
- **State Updates:** All mutations must go through a dedicated `GameLogic` or `Update` method in the `Core` project.

## 3. Unfolding Engine
- **Discovery State:** A `Dictionary<string, bool>` or similar structure to track which features are "unlocked."
- **Event Bus:** (Planned) A simple event system for notifying the UI layer when a discovery is made.

## 4. UI Abstraction
- **Layout:** A modular UI system managed by `LayoutSystem` which decouples button configuration and positioning from the main `Game1` class.
- **Specialized Systems:** 
    - `LogSystem`: Manages the narrative log, historical message tracking, and fading text rendering.
    - `WorldMapSystem`: Handles grid-based exploration logic, coordinate translation, and map rendering.
    - `InputManager`: Abstracts MonoGame mouse and keyboard state for cleaner interaction logic.
- **Binding:** The Desktop layer reads the `GameState` and employs these specialized systems to render UI elements and process input.

## 5. Agentic Piloting & Headless Mode
To support heavy agentic development, the architecture allows for automated execution and UI verification:
- **GameEngine Driver:** The `GameEngine` in `Core` is the primary interface.
- **HeadlessDriver:** A text-based interface for `GameEngine` used in tests and balance simulations.
- **AiModeSystem:** A Desktop-layer system that reads `ai_commands.txt` and drives the engine during agentic review.
- **Validation:** Automated scripts in `scripts/` and unit tests in `tests/` use these drivers to verify game balance and UI layout via screenshots.

## 6. Data-Driven Logic
- **Manifestations:** Defined in `manifestations.json`. Loaded at runtime by `GameEngine` via `ManifestationManager`.
- **Lore:** Narrative fragments defined in `lore.json`, triggered by `GameEngine` based on discoveries or map exploration.
- **Assets:** Fonts and textures are managed through the MonoGame Content Pipeline, with fallback logic for missing assets.

---
*Follow these patterns for all new features.*
