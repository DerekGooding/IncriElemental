# ROADMAP_DETAILED.md - Implementation Requirements

This document provides granular technical and gameplay requirements for the unfinished goals defined in `roadmap.md`.

---

## Goal 24: The Final Polish (Implemented)

### Tutorialization
- **Requirement:** Implement a step-by-step onboarding system.
- **Implementation:** `TutorialSystem.cs` in the `UI` namespace manages the onboarding state machine.
- **UX:** Dims the screen using a semi-transparent black overlay and highlights specific buttons (Focus, Manifestations) based on the current step.

### Visual Polish
- **Requirement:** Enhance feedback for significant actions.
- **Ascension:** Screen-shake and alpha-fading transitions implemented in `VisualManager` and `EndingSystem`.
- **Lore:** Typewriter effects and fading log entries implemented in `LogSystem`.

### Localization
- **Requirement:** Move all text strings to `src/IncriElemental.Desktop/Content/strings.json`.
- **Implementation:** `TextService` (singleton) in `Core` handles key-based lookup and `string.Format` argument injection. All UI components use `TextService.Instance.Get()` for display text.

---

## Goal 25: Visual Overhaul: "The Aetherial Glow" (Implemented)

### HLSL Post-Processing
- **Requirement:** Implement a multi-pass post-processing pipeline.
- **Bloom:** `Bloom.fx` shader performs brightness extraction and additive blending to create a spectral glow around high-contrast UI elements.
- **Implementation:** Managed by `VisualManager` via render target switching.

### Procedural Background
- **Requirement:** Create `BackgroundManager.cs` to handle a dynamic, layered backdrop.
- **Implementation:** Procedurally generates a starfield where star speed is tied to the current Aether production rate via a logarithmic scale.
- **Reactivity:** Star colors and movement dynamically react to the game state.

---

## Goal 26: Mechanical Depth: "Resonant Harmony" (Implemented)

### Grid Synergies (Auras)
- **Requirement:** Each manifestation on the map generates an `Aura` object affecting its neighbors.
- **Implementation:** `Aura.cs` and `WorldMap.cs` handle aura projection. `AutomationSystem` applies multipliers based on local aura intensity.
- **Aura Strength:** Landmarks and manifestations project auras with varying intensities.
- **Interactions:** Auras provide a 10% production multiplier per unit of intensity for matching resource types.

### Alchemical Mixing Table
- **Requirement:** A sub-tab in the Spire for active gameplay.
- **Implementation:** `MixingTableSystem` provides a drag-and-drop interface for essences. `AlchemySystem` calculates the resulting buffs.
- **Success:** Successful reactions grant high-potency temporary buffs to resource generation.

---

## Goal 27: Architecture: "Component-Driven Manifestations" (Implemented)

### Data-Driven Components
- **Requirement:** Replace the rigid `ManifestationDefinition` with an ECS-lite approach.
- **Implementation:** `IManifestationComponent` interface with concrete implementations:
    - `ProducerComponent`: Generates resources over time.
    - `StorageComponent`: Increases resource caps.
    - `AuraComponent`: Projects auras on the world map.
    - `UnlockComponent`: Triggers discovery of new manifestations or features.
- **Benefit:** Allows for infinite manifestation variety by mixing and matching components in `manifestations.json`.

---

## Goal 28: UI/UX: "The Wizard's Interface" (InProgress)

### Rich Text Tooltips & Icons
- **Requirement:** Implement a custom `RichText` parser in the `UI` namespace.
- **Implementation:** `RichTextSystem.cs` handles parsing and drawing of tag-based strings.
- **Tags:** Support `[c:...]` for color and `[i:...]` for inline sprite icons.
- **Integration:** Used extensively in `LogSystem` and `StatusSystem` tooltips.

### Layout Refactor
- **Requirement:** Improve UI responsiveness.
- **Implementation:** Refactored `LayoutSystem` and `UiLayout` for more robust anchor-based positioning and dynamic scaling.

---
*Last Updated: Wednesday, March 12, 2026 (Updated by Agent Gemini)*
