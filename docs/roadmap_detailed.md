# ROADMAP_DETAILED.md - Implementation Requirements

This document provides granular technical and gameplay requirements for the unfinished goals defined in `roadmap.md`.

---

## Goal 24: The Final Polish

### Tutorialization
- **Requirement:** Implement a step-by-step onboarding system.
- **Stages:** 
    - `Focusing`: Guide the player to click the Focus button.
    - `Manifesting`: Highlight the first available manifestation.
    - `Automating`: Explain how passive production works.
- **UX:** Dim the rest of the UI and point to the relevant button with a glowing arrow.

### Visual Polish
- **Requirement:** Enhance feedback for significant actions.
- **Ascension:** Screen-shake on clicking "Ascend," followed by a fade-to-white effect.
- **Lore:** Add a typewriter effect to the LogSystem for new narrative entries.

### Localization
- **Requirement:** Move all text strings to `src/IncriElemental.Desktop/Content/strings.json`.
- **System:** Create a `TextService` to handle string lookup by key (e.g., `TXT_FOCUS_BTN`).

---

## Goal 25: Visual Overhaul: "The Aetherial Glow"

### HLSL Post-Processing
- **Requirement:** Implement a multi-pass post-processing pipeline.
- **Bloom:** A 2-pass Gaussian blur on a brightness-filtered version of the main render target, blended additively back onto the scene.
- **Chromatic Aberration:** Offsetting Red and Blue channels towards the edges of the screen, proportional to `focus_intensity`.
- **Aether Waves:** A distortion shader using a time-scrolling normal map to ripple the UI.

### Procedural Background
- **Requirement:** Create `BackgroundManager.cs` to handle a dynamic, layered backdrop.
- **Noise:** Use Simplex noise shaders to generate nebula-like patterns.
- **Reactivity:** Tie noise speed and color (Aether-purple vs Fire-red) to the highest production resource.
- **Parallax:** Split the background into three layers (Distant Stars, Middle Nebula, Foreground Dust) and offset them by `mousePosition * depthFactor`.

---

## Goal 26: Mechanical Depth: "Resonant Harmony"

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
