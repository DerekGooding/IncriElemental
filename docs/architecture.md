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
- **Layout:** A simple UI system (potentially custom or a library like Myra/MonoGame.Extended) to manage buttons and text.
- **Binding:** The Desktop layer reads the `GameState` and renders UI elements based on the discovery state.

## 5. Agentic Piloting & Headless Mode
To support heavy agentic development, the architecture allows for automated execution and UI verification:
- **GameEngine Driver:** The `GameEngine` in `Core` is the primary interface.
- **AiModeSystem:** A Desktop-layer system that reads `ai_commands.txt` and drives the engine during agentic review.
- **LayoutSystem:** Decouples button configuration and positioning from the main `Game1` class.
- **Validation:** Automated scripts in `scripts/` use these drivers to verify game balance and UI layout via screenshots.

## 6. Source Control & DevOps
See [docs/devops.md](docs/devops.md) for details on the agentic ownership of the GitHub repository, CI/CD, and deployment strategy.

---
*Follow these patterns for all new features.*
