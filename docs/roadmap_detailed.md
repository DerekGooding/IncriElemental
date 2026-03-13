# ROADMAP_DETAILED.md - Implementation Requirements

This document provides granular technical and gameplay requirements for the unfinished goals defined in `roadmap.md`.

---

## Goal 31: The Aesthetic Awakening (InProgress)

### Phase 1: Foundational Atmosphere & Depth

#### Nebula Vistas
- **Requirement:** Replace static starfield with multi-layered textures.
- **Verification:** `parallax_audit.py` - Ensure background layers have varying movement vectors during camera transitions.

#### Glassmorphism Panels
- **Requirement:** Semi-transparent UI backgrounds with Gaussian blur.
- **Verification:** `contrast_check.py` and `palette_audit.py` - Verify text legibility against dynamic backgrounds.

#### Smart Bloom
- **Requirement:** Tie HLSL Bloom intensity to `GameState.TotalProduction`.
- **Verification:** `aesthetic_audit.py` - Verify "Glow Score" correlates with resource rates.

#### Void Atmosphere & Color Grading
- **Requirement:** Pulsing stars and elemental-themed LUTs (Lookup Tables).
- **Verification:** `palette_audit.py` - Ensure scene colors remain within Element-specific profiles.

---

### Phase 2: Kinetic Interface & Feedback

#### Aura Pulse Borders
- **Requirement:** Use `Math.Sin` to animate button border thickness and glow.
- **Verification:** `aura_pulse_audit.py` - Verify border luminosity changes over a 1-second interval.

#### Impact Shake & Custom Mouse Trails
- **Requirement:** Apply camera matrices offsets on click; emit particles behind the cursor.
- **Verification:** `Performance Analysis` (metadata) - Ensure particle-heavy trails don't spike frame-time.

#### Runically Shaped Button States
- **Requirement:** High-fidelity Runes replacing standard boxes.
- **Verification:** `button_state_audit.py` and `semantic_review.py` - Ensure complex shapes have accurate hover detection and no clipping.

---

### Phase 3: Information Dynamics & Visualization

#### Resource Sparklines & Flowing Lines
- **Requirement:** Animate resource trends and energy movement in the Flow Tab.
- **Verification:** `visual_component_testing` and `graph_audit.py` - Ensure mini-graphs scale correctly and lines remain distinct.

#### RPG Narrative Log & Status Medals
- **Requirement:** Premium typography and high-quality status icons.
- **Verification:** `icon_alignment.py` and `log_scroll_audit.py` - Verify pixel-perfect alignment of medals and dialogue text.

---

### Phase 4: Mastery & High-Fidelity Events

#### Alchemical Events & Celebration Sequences
- **Requirement:** Full-screen visual payloads for narrative and mechanical milestones.
- **Verification:** `reaction_audit.py` and `Sequential Screenshot Analysis` - Verify frame-by-frame payload delivery.

#### Map Biome Shaders
- **Requirement:** Heat-haze and refraction shaders for specific World Map biomes.
- **Verification:** `aura_pulse_audit.py` - Use pixel-delta analysis to confirm shader activity.

#### Ultra-Wide Support
- **Requirement:** Dynamic anchoring for 21:9 aspect ratios.
- **Verification:** `stress_testing` - Use `key:OemOpenBrackets` sequences to verify UI re-anchoring.

---
*Last Updated: Friday, March 13, 2026 (Updated by Agent Gemini)*
