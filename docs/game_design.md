# GAME_DESIGN.md - IncriElemental Game Mechanics

This document defines the mechanics, balance formulas, and progression for **IncriElemental**.

## 1. Game Premise
**IncriElemental** is an unfolding incremental game about a disembodied wizard consciousness in a void. You do not "craft" from resources found; you **manifest** reality from the raw magical potential of the universe.

### The Core Loop
1.  **Focus:** Gather *Aether* (Raw Magic) from the surrounding void.
2.  **Manifest:** Condense Aether into physical forms (Matter/Energy).
3.  **Resonate:** Physical forms generate specific Elemental Essences (Earth, Fire, Water, Air).
4.  **Complexify:** Combine Elemental Essences to manifest advanced structures and automate the gathering process.

## 2. Resources & Elements

| Resource | Description | Source | Function |
|---|---|---|---|
| **Aether** | Raw, unformed potential. | Manual "Focus" | Currency for initial manifestations. |
| **Earth** | Stability, Mass. | Manifested Stone/Soil | Increases storage, forms the "base" of reality. |
| **Fire** | Energy, Transformation. | Manifested Sparks/Flames | Speeds up processes, transforms matter. |
| **Water** | Flow, Connection. | Manifested Condensation | Automates transfer of resources. |
| **Air** | Space, Expansion. | Manifested Gusts | Unlocks new UI areas/tabs (expanding perception). |

## 3. Unfolding Progression

### Phase 1: The Void (The Beginning)
- **State:** A black screen. Single button: "Focus".
- **Action:** Clicking "Focus" generates +1 Aether.
- **Unlock:** At 10 Aether, button appears: "Manifest Speck" (Cost: 10 Aether).
- **Effect:** A Speck exists. It generates 0.1 Earth Essence/sec.

### Phase 2: The Spark (Energy)
- **Requirement:** 100 Earth Essence.
- **Unlock:** "Manifest Spark" (Cost: 50 Aether, 50 Earth).
- **Effect:** Fire Essence unlocks.
- **Mechanic:** Fire consumes Earth to burn? Or just exists? (TBD: Balance burn rates).

### Phase 3: Matter & Form
- **Concept:** Unlike "Wood -> Plank", you manifest "Geometric Forms" or "Runes".
- **Example:** Manifest "Rune of Solidity" (Uses Earth) -> Increases Max Aether.
- **Example:** Manifest "Rune of Attraction" (Uses Aether) -> Automates passive Aether gathering.

## 4. Visual Style
- **Minimalist Magical:** Dark background. Text is glowing/spectral.
- **Particles:** When manifesting, use particle effects (MonoGame) to show the magic condensing.
- **Grid:** As reality forms, a grid/map might appear representing the physical space you've created.

---
*Keep this document updated with every mechanic change.*
