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

## 4. Goal: Persistence & Automation (Current)
- [ ] Save/Load System: Serialize `GameState` to JSON.
- [ ] "Rune of Attraction": Manifestation to automate manual "Focus".
- [ ] Proper Font Support: Replace colored boxes with legible text for log and resources.

## 5. Goal: Architectural Evolution
- [ ] The Altar: A physical central structure that increases storage capacity.
- [ ] Mana Flow: Water element automating resource transfers between containers.
- [ ] Elemental Reaction: Fire + Earth -> Forge (Manifesting tools).

## 6. Goal: Validation & Agentic Piloting
- [ ] Comprehensive Unit Testing: Expand `IncriElemental.Tests` for all new mechanics.
- [ ] Headless Mode: Implement a mode in `IncriElemental.Desktop` that runs without a window for quick state validation.
- [ ] Agentic Piloting Interface: A CLI-friendly "driver" that allows me to issue commands (e.g., `Focus`, `Manifest`) and read the resulting `GameState` JSON directly.
- [ ] Balance Simulation: A tool to "fast-forward" the game state to check for resource bottlenecks or runaway growth.

---
*Follow this roadmap for overall progress tracking.*
