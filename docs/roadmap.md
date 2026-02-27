# ROADMAP.md - IncriElemental Task History

This document tracks the long-term goals and task history for **IncriElemental**.

## 1. Goal: The Void & Aether
- [x] Initial Project Setup (.NET 10.0, MonoGame)
- [x] Implement `Aether` resource (Core).
- [x] Implement `Focus` action (Core).
- [x] Visual: The Dark Screen with a single, glowing "Focus" button.
- [x] Visual: Aether condensation particle effect.

## 2. Goal: Manifestation & Earth
- [x] Implement `Manifest` action (Core).
- [x] Implement `Earth` resource (Core).
- [x] Visual: Particle effect when manifesting.
- [x] UI: Resource tracking for Earth.

## 3. Goal: Elemental Expansion
- [x] Add Fire/Water/Air as resources.
- [x] Implement manifestation rules for Spark (Fire), Droplet (Water), Breeze (Air).
- [x] Tiers: Fire requires Earth, Water requires Fire, etc.

## 4. Goal: Persistence & Automation
- [x] Save/Load System: Serialize `GameState` to JSON.
- [x] "Rune of Attraction": Manifestation to automate manual "Focus".
- [x] Proper Font Support: Replace colored boxes with legible text for log and resources.

## 5. Goal: Architectural Evolution
- [x] The Altar: A physical central structure that increases storage capacity.
- [x] Mana Flow: Water element automating resource transfers between containers.
- [x] Elemental Reaction: Fire + Earth -> Forge (Manifesting tools).

## 6. Goal: Validation & Agentic Piloting
- [x] Comprehensive Unit Testing: Expand `IncriElemental.Tests` for all new mechanics.
- [x] Headless Mode: Implement a mode in `IncriElemental.Desktop` that runs without a window for quick state validation.
- [x] Agentic Piloting Interface: A CLI-friendly "driver" that allows me to issue commands (e.g., `Focus`, `Manifest`) and read the resulting `GameState` JSON directly.
- [x] Balance Simulator: A tool to "fast-forward" the game state to check for resource bottlenecks or runaway growth.

## 7. Goal: Polishing & Expansion (Completed)
- [x] Visual Assets: Proper sprites and icons for all elements.
- [x] Sound Effects: (Deferred) Audio feedback for magical actions.
- [x] Story Arc: Narrative milestones based on manifestations.

## 8. Goal: The Forge & Refactoring (Completed)
- [x] **Refactoring:** Split `GameEngine.cs` into `ManifestationManager` and `AutomationSystem`.
- [x] **Tools Mechanic:** Manifest "Pickaxe" and "Focus Crystal".
- [x] **Advanced Materials:** Combine elements (Earth + Water = Clay, Earth + Fire = Glass).

## 9. Goal: Elemental Mastery (Completed)
- [x] **Structures:** Implement "Well" (Water) and "Brazier" (Fire) for passive generation and storage.
- [x] **Balance:** Tune generation rates to ensure a smooth 5-10 minute mid-game.

## 10. Goal: The Garden of Life (Completed)
- [x] **New Resource:** "Life" Essence.
- [x] **Manifestation:** "Garden" (Consumes Water/Earth, generates Life).
- [x] **Familiars:** Manifest entities that automate resource gathering.

## 11. Goal: The Spire (Completed)
- [x] **The Great Project:** A multi-stage construction (Foundation, Shaft, Core).
- [x] **Visuals:** The Spire grows physically in the center of the screen.

## 12. Goal: Ascension & Credits (Completed)
- [x] **The Final Act:** "Ascend" button unlocks when The Spire is complete.
- [x] **Ending:** Visual transition to a "Pure Light" state.
- [x] **Credits:** Scrolling text acknowledging the user and the agent.

## 13. Goal: Technical Decoupling (Completed)
- [x] **Data-Driven:** Move all manifestation definitions to `manifestations.json`.
- [x] **Input & UI:** Implement `InputManager` and specialized systems (`LogSystem`, `WorldMapSystem`).
- [x] **Font Portability:** Automated font download script for CI/local builds.

## 14. Goal: The Alchemist (Completed)
- [x] **Elemental Reactions:** Implement a "Mix" mechanic for temporary resource boosts via `AlchemySystem`.
- [x] **Buff System:** Track active temporary modifiers in the `Core` logic.

## 15. Goal: Beyond the Spire (Completed)
- [x] **World Map:** Implement a minimalist grid-based map of the manifested reality.
- [x] **Exploration:** Allow Familiars to explore grid cells to find Lore or rare Essences.

## 16. Goal: Sensory Immersion (Completed)
- [x] **Audio:** Implement `AudioManager` for magic actions, exploration, and UI.
- [x] **Visuals:** Refactor `VisualManager` and `ParticleSystem` for consistent elemental feedback.

## 17. Goal: Advanced Agentic DevOps (Completed)
- [x] **Balance Simulator:** Fully functional test verifying the 20-minute loop.
- [x] **Agentic UI:** Automated screenshot capture in `Game1.cs` for visual verification.
- [x] **Visual Regressions:** Implement screenshot diffing in the CI using `xvfb` and `Pillow`.

## 18. Goal: Narrative Expansion (Completed)
- [x] **Lore Fragments:** Data-driven story bits in `lore.json` found through exploration.
- [x] **Deep Unfolding:** New Post-Ascension tier ("Void Infusion") providing enhanced scaling in New Game+.

## 19. Goal: Code Health & Maintenance (Completed)
- [x] **Decoupling:** Refactor `Game1.cs` into specialized systems (`LogSystem`, `WorldMapSystem`) to satisfy monolith constraints (< 250 lines).
- [x] **Test Robustness:** Ensure all tests load external data-driven definitions via `TestHelper`.
- [x] **CI Reliability:** Ensure the shields deployment handles empty diffs gracefully.
- [x] **Asset Portability:** Ensure data files are correctly copied to the output directory.



---
*Last Updated: Friday, February 27, 2026*

---
*Follow this roadmap for overall progress tracking.*
