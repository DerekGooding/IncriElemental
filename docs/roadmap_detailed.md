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

### Phase 2: Kinetic Runics & Holography

#### Adaptive Runic HUD & Frame Animation
- **Requirement:** Make UI frame runes shift shape or speed based on production intensity.
- **Implementation:** `VisualManager.DrawPanel` updated with runic-specific `textureRect` offsets that cycle faster at high production rates.
- **Verification:** `rune_sweep_audit.py` to compare frame-to-frame rune offsets.

#### Holographic Transaction Popups (Runic Distortion)
- **Requirement:** Replace floating text popups with a holographic distorted effect.
- **Implementation:** `ParticleSystem.EmitPopup` will use a specialized `Hologram.fx` shader that jitters and cycles between runic and numeric characters.
- **Verification:** `particle_density_test` to ensure numbers remain readable during distortion.

#### Cinematic Camera Swells (Matrix Transforms)
- **Requirement:** Dynamic matrix-based camera transforms for "reveal" moments.
- **Implementation:** `VisualManager.CameraMatrix` will apply smooth `zoom` and `rotation` offsets during Alchemical Mixes and Ascension sequences.
- **Verification:** `camera_matrix_audit.py` (new) to confirm smooth interpolation of view matrices.

---

### Phase 3: Agentic Sight & Visual Integrity

#### Semantic Intent Metadata (Intent-Aware Tagging)
- **Requirement:** Add "Semantic Intent" tags to the exported `screenshot.json`.
- **Implementation:** `LayoutSystem.GetLayoutMetadata` updated to include `Intent` strings (e.g., "CriticalAction," "Navigation," "Information").
- **Verification:** `json_schema_audit.py` to ensure all metadata files conform to the new intent-aware schema.

#### Automated Visual Regression & "Golden References"
- **Requirement:** Automate comparing the current game state against a baseline.
- **Implementation:** `scripts/visual_sanity_check.py` will load `screenshot.json` and compare element positions and visibility against a "Golden Reference" layout.
- **Verification:** If elements shift > 5 pixels or overlap illegally, the script fails the visual audit.

#### Visual Gallery Module & Documentation Integration
- **Requirement:** Automated script to capture and organize screenshots for documentation.
- **Implementation:** `scripts/generate_gallery.py` will trigger the game's internal screenshot system for every major tab and output a markdown mosaic to `review/gallery.md`.
- **Status:** Planning.

---
*Last Updated: Tuesday, March 17, 2026 (Updated by Agent Gemini)*
