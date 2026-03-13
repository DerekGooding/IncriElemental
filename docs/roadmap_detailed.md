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

## Goal 26: Mechanical Depth: "Resonant Harmony" (Planned)

### Grid Synergies (Auras)
- **Requirement:** Each manifestation on the map generates an `Aura` object affecting its neighbors.
- **Aura Strength:** Proportional to the number of manifestations in that cell.
- **Interactions:**
    - `Harmony`: Matching auras (e.g. Earth/Earth) provide a +10% production multiplier.
    - `Steam (Air + Water)`: Increases Air generation, reduces Earth.
    - `Ash (Fire + Earth)`: Increases Earth generation, reduces Fire.

### Alchemical Mixing Table
- **Requirement:** A sub-tab in the Spire for active gameplay.
- **The Vessel:** A 2D area where players drag-and-drop elemental essence icons.
- **Stability:** Each recipe has a "Vibration Frequency." Players must keep the vessel centered (using mouse or hotkeys) to prevent the reaction from collapsing.
- **Success:** Grants temporary high-potency buffs (e.g. "Pure Aether Focus" x10).

---

## Goal 27: Architecture: "Component-Driven Manifestations"

### Data-Driven Components
- **Requirement:** Replace the rigid `ManifestationDefinition` with an ECS-lite approach.
- **Structure:** `ManifestationDefinition { List<IManifestationComponent> Components }`.
- **Components:**
    - `ProducerComponent`: Generates resources over time.
    - `StorageComponent`: Increases resource caps.
    - `AuraComponent`: Projects auras on the world map.
- **Benefit:** Allows for infinite manifestation variety by mixing and matching components in JSON.

### Event-Driven UI Binding
- **Requirement:** The UI should be an observer of the GameState, not a frequent poller.
- **Update Logic:** `StatusSystem` subscribes to `ResourceChanged` events. It only updates the "Aether Amount" text if the change exceeds 1% of the total or a threshold (e.g. every 0.1 units).
- **String Caching:** Resource amounts are formatted into strings only when they change, reducing garbage collection pressure.

---

## Goal 28: UI/UX: "The Wizard's Interface"

### Rich Text Tooltips & Icons
- **Requirement:** Implement a custom `RichText` parser in the `UI` namespace.
- **Tags:** Support `[color:...]`, `[size:...]`, and `[icon:...]` tags.
- **Icons:** Inline sprite rendering for resource icons (e.g. "Requires 10 [icon:aether] Aether").

### Resource Flow Graphs
- **Requirement:** A visual representation of the game's economy.
- **The Flow Tab:** A node-based graph using the `ScissorRectangle` for infinite panning.
- **Edges:** Lines connecting resources to manifestations, with thickness representing the relative production/consumption rates.

### Power User Controls
- **Requirement:** Implement a robust `InputManager` binding system.
- **Pinning:** Right-clicking a resource or manifestation in the Status panel pins a "Monitoring Box" to the corner of the screen that stays visible across all tabs.
- **Scaling:** UI coordinates in `UiLayout` are multiplied by a `GlobalScale` float (0.5 to 2.0).

---
*Created: Thursday, March 12, 2026 (Updated by Agent Gemini)*
