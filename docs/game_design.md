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
| **Life** | Consciousness, Growth. | Manifested Gardens | Fuels Familiars and the Spire Foundation. |

## 3. Unfolding Progression

### Phase 3: The World Map (Exploration)
- **Requirement:** Manifestation of a "Familiar".
- **Interaction:** A grid-based map appears in the `World` tab.
- **Exploration:** Click on grid cells to send Familiars to explore.
- **Landmarks:** Fixed coordinates (e.g., Ancient Archive, Eternal Spring) provide unique narrative descriptions and significant resource rewards.
- **Outcome:** Exploration unlocks Lore fragments and rare discoveries.

### Phase 4: The Spire & Ascension
- **Inventory:** A dedicated UI area in the `StatusSystem` tracks all manifested items and structures beyond simple essences.
- **Goal:** Manifest the Great Spire in the center of the void.
- **Stages:** Foundation (Earth/Life), Shaft (Fire/Air), Core (Aether/Water).
- **Outcome:** Constructing the Core unlocks the **ASCEND** button, leading to the victory sequence.
- **New Game Plus:** "A New Awakening" grants **Void Embers** and "Cosmic Insight," multiplying all manual and passive resource gains.
- **The Constellation:** A prestige tree (available in the `Constellation` tab) where Void Embers are spent on permanent, high-scaling upgrades like "Eternal Focus" and "Dense Matter."
- **Void Infusion:** A Post-Ascension mechanic allowing the direct infusion of elemental storage using raw Aether.

## 4. Elemental Synergies
Beyond simple manifestations, combining elements in advanced structures creates synergies:
- **Magma Forge (Fire + Earth):** Significantly boosts production of both elements.
- **Clouds (Water + Air):** Enhances the flow of atmospheric essences.
- **Alchemy Mixes:** Temporary but powerful generation multipliers triggered by combining opposing elements (e.g., Combustion from Fire/Air).

---
*Last Updated: Friday, February 27, 2026 (Updated by Agent Gemini)*
- **Minimalist Magical:** Dark background. Text is glowing/spectral.
- **Particles:** When manifesting, use particle effects (MonoGame) to show the magic condensing.
- **The Map:** A minimalist 10x10 grid visualizing the manifested reality. Explorations are marked by changing colors of the cells.

---
*Keep this document updated with every mechanic change.*
