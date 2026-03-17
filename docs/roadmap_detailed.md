# ROADMAP_DETAILED.md - Implementation Requirements

This document provides granular technical and gameplay requirements for the unfinished goals defined in `roadmap.md`.

---

## Goal 31: The Aesthetic Awakening (Implemented)
- **Status:** Complete.
- **Summary:** Transitioned the project from a basic prototype to a high-fidelity glassmorphic UI with runic accents, multi-layered parallax, and smart HLSL bloom.

---

## Goal 32: The Visionary's Ascendance (Active)
*Expanding the visual depth and agentic safeguards to create a showcase-ready masterpiece.*

### Phase 1: Volumetric Depths & Fluidity (Implemented)

#### Volumetric Aether Clouds & Interactive Ripples
- **Requirement:** Replace background layers with a reactive fluid shader.
- **Implementation:** `BackgroundManager.cs` manages a 64x48 grid for fluid simulation; `Fluid.fx` renders interactive "Aether" that ripples on mouse clicks.
- **Verification:** `parallax_audit.py` (updated logic) confirms non-zero pixel deltas in the background shader on input events.
- **Status:** Complete.

#### Element-Spec Scene Tinctures
- **Requirement:** Shift the entire game scene's color profile based on the dominant resource.
- **Implementation:** `Game1.UpdateGameLogic` calculates dominant production; `VisualManager.Update` interpolates `_globalTint` towards the dominant color.
- **Status:** Complete.

---

### Phase 2: Kinetic Runics & Holography (Implemented)

#### Adaptive Runic HUD & Frame Animation
- **Requirement:** Make UI frame runes shift shape or speed based on production intensity.
- **Implementation:** `UiVisuals.DrawPanel` draws moving "runic" dots along the border; speed scales with `ProductionIntensity`.
- **Status:** Complete.

#### Holographic Transaction Popups (Runic Distortion)
- **Requirement:** Replace floating text popups with a holographic distorted effect.
- **Implementation:** `ParticleSystem.EmitPopup` uses `Hologram.fx` for numeric popups (jitter, scanlines).
- **Status:** Complete.

#### Cinematic Camera Swells (Matrix Transforms)
- **Requirement:** Dynamic matrix-based camera transforms for "reveal" moments.
- **Implementation:** `VisualManager` implements `GetCameraMatrix()` with zoom and rotation around screen center.
- **Status:** Complete.

---

### Phase 3: Agentic Sight & Visual Integrity (Implemented)

#### Semantic Intent Metadata (Intent-Aware Tagging)
- **Requirement:** Add "Semantic Intent" tags to the exported `screenshot.json`.
- **Implementation:** `UiMetadataTracker` now includes `Intent` fields. `Button` and `VisualManager` register elements with semantic tags (e.g., "TabNavigation," "ManifestStructure").
- **Status:** Complete.

#### Automated Visual Regression & "Golden References"
- **Requirement:** Automate comparing the current game state against a baseline.
- **Implementation:** `scripts/visual_sanity_check.py` compares live `screenshot.json` vs `docs/golden_reference.json`, checking for intent shifts and positional delta.
- **Status:** Complete.

#### Visual Gallery Module & Documentation Integration
- **Requirement:** Automated script to capture and organize screenshots for documentation.
- **Implementation:** `scripts/generate_gallery.py` runs a multi-tab capture script and generates `review/gallery.md`.
- **Status:** Complete.

### Phase 5: Resonance & Fractal Audits (Implemented)

#### Acoustic UI Resonance (Shiver Effects)
- **Requirement:** Panels "shiver" and pulse in sync with void hum and production peaks.
- **Implementation:** `VisualManager.ResonanceIntensity` tracks alchemical activity. `GetResonanceMatrix()` applies shaky offsets to UI panels and logs.
- **Status:** Complete.

#### Tooltip Materialization Metadata (Audit Safety)
- **Requirement:** Track tooltip state in JSON to prevent click collisions.
- **Implementation:** `UiVisuals.DrawTooltip` registers itself in `UiMetadataTracker` with "InformationPopup" intent.
- **Status:** Complete.

#### Fractal Safety Maps (Obscuration Tracking)
- **Requirement:** Export screen "safe zones" during high-intensity sequences.
- **Implementation:** `VisualManager` registers "VisualObscuration" elements in metadata when flashes exceed opacity thresholds.
- **Status:** Complete.
