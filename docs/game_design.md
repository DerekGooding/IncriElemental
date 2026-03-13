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
| **Void Embers** | Residue of a collapsed reality. | Ascension | Currency for permanent upgrades in the Constellation. |

## 3. UI & Experience
- **Minimalist Magical:** Dark background with glowing/spectral text and particle effects enhanced by **HLSL Bloom** for a high-fidelity magical aura.
- **Rich Text & Icons:** Tooltips and narrative logs utilize a custom engine for color-coded text and inline resource icons, enhancing readability and flavor.
- **Onboarding:** A multi-stage **Tutorial System** guides players through their initial awakening, highlighting key interactions while dimming unrelated UI elements.
- **Reactive Backdrop:** A procedural **Background Manager** generates a dynamic starfield that accelerates as the player's Aether production increases, visualizing the acceleration of reality.
- **Responsive Stacking:** All buttons are dynamically stacked in the center column. If they exceed the window height, the tab becomes scrollable via the mouse wheel.
- **Interactive Feedback:** Hovering over any manifestation (button or inventory item) reveals a detailed rich-text tooltip detailing its production rates and total scaling bonuses.
- **The Map:** A minimalist 10x10 grid visualizing the manifested reality. Explorations are marked by changing colors of the cells.

## 4. Unfolding Progression

### Phase 3: The World Map (Exploration & Synergies)
- **Requirement:** Manifestation of a "Familiar".
- **Interaction:** A grid-based map appears in the `World` tab.
- **Exploration:** Click on grid cells to send Familiars to explore.
- **Spatial Synergies (Auras):** Manifestations and landmarks project "Auras" to neighboring cells. These auras provide production multipliers and unique elemental effects to any manifestations placed within their influence.
- **Landmarks:** Fixed coordinates (e.g., Ancient Archive, Eternal Spring) provide unique narrative descriptions, significant resource rewards, and powerful static auras.
- **Outcome:** Exploration unlocks Lore fragments and rare discoveries.

### Phase 4: Alchemy & The Spire
- **Alchemical Mixing Table:** A specialized sub-tab where players can drag-and-drop captured elemental essences to perform high-level reactions. Successful mixes grant temporary but massive buffs (e.g., "Aetheric Surge" x5 production).
- **Stages:** Foundation (Earth/Life), Shaft (Fire/Air), Core (Aether/Water).
- **Outcome:** Constructing the Core unlocks the **ASCEND** button, leading to the victory sequence.
- **New Game Plus:** "A New Awakening" grants **Void Embers** and "Cosmic Insight," multiplying all manual and passive resource gains.
- **The Constellation:** A prestige tree (available in the `Constellation` tab) where Void Embers are spent on permanent, high-scaling upgrades like "Eternal Focus" and "Dense Matter."

## 5. Elemental Synergies
Beyond simple manifestations, combining elements in advanced structures creates synergies:
- **Magma Forge (Fire + Earth):** Significantly boosts production of both elements.
- **Clouds (Water + Air):** Enhances the flow of atmospheric essences.
- **Alchemy Mixes:** Temporary but powerful generation multipliers triggered by combining opposing elements (e.g., Combustion from Fire/Air).

---
*Last Updated: Thursday, March 12, 2026 (Updated by Agent Gemini)*
