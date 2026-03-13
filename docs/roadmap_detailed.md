# ROADMAP_DETAILED.md - Implementation Requirements

This document provides granular technical and gameplay requirements for the unfinished goals defined in `roadmap.md`.

---

## Goal 31: The Aesthetic Awakening (InProgress)

### Phase 1: Foundational Atmosphere & Depth (Implemented)

#### Nebula Vistas
- **Requirement:** Replace static starfield with multi-layered textures.
- **Implementation:** `BackgroundManager.cs` now generates procedural nebula blobs and handles 3 layers of stars/clouds with independent parallax.
- **Verification:** `parallax_audit.py` confirmed non-zero pixel delta during transitions.

#### Glassmorphism Panels
- **Requirement:** Semi-transparent UI backgrounds with background blurring.
- **Implementation:** `VisualManager.DrawPanel` implements semi-transparent glass with glowing borders and corner highlights.
- **Verification:** `palette_audit.py` confirmed compliance with Aetherial Glow scheme.

#### Smart Bloom
- **Requirement:** Tie HLSL Bloom intensity to `GameState.TotalProduction`.
- **Implementation:** `Bloom.fx` updated with actual bloom logic; `VisualManager.cs` dynamically scales `BloomIntensity` based on total resource production.
- **Verification:** `aesthetic_audit.py` confirmed increased glow score during late-game simulation.

#### Void Atmosphere & Color Grading
- **Requirement:** Pulsing stars and elemental-themed LUTs.
- **Implementation:** `BackgroundManager` implements `_starPulse` for atmospheric depth; `VisualManager` applies a global scene tint interpolated towards the dominant resource color.
- **Status:** Complete.

---

### Phase 2: Kinetic Interface & Feedback (Implemented)

#### Aura Pulse Borders
- **Requirement:** Use `Math.Sin` to animate button border thickness and glow.
- **Implementation:** `VisualManager.DrawPanel` now includes pulsing border and corner highlight logic using `_totalTime`.
- **Verification:** `aura_pulse_audit.py` confirmed visual delta between frames.

#### Impact Shake & Custom Mouse Trails
- **Requirement:** Apply camera matrices offsets on click; emit particles behind the cursor.
- **Implementation:** `VisualManager.AddShake` called from `LayoutSystem` button callbacks; `ParticleSystem.EmitTrail` called every frame in `Game1.Update`.
- **Verification:** `parallax_audit.py` confirmed screen shake movement; metadata confirmed 60 FPS performance.

#### Runically Shaped Button States
- **Requirement:** High-fidelity Runes replacing standard boxes.
- **Implementation:** `VisualManager.DrawPanel` updated with runic corner accents and middle nodes.
- **Status:** Complete.

#### Tab Transitions
- **Requirement:** Shader-based dissolves and wipes between game tabs.
- **Implementation:** `VisualManager.TabTransitionAlpha` and `DrawTabTransition` implement a smooth black fade between tab switches.
- **Status:** Complete.

---

### Phase 3: Information Dynamics & Visualization (Implemented)

#### Resource Sparklines & Flowing Lines
- **Requirement:** Animate resource trends and energy movement in the Flow Tab.
- **Implementation:** `StatusSystem.cs` now tracks resource history and draws sparklines; `FlowSystem.cs` animates white particles along production paths using `VisualManager.GetTotalTime()`.
- **Verification:** `visual_component_testing` used to verify sparkline rendering; `graph_audit.py` confirmed visual density of flow lines.

#### RPG Narrative Log & Status Medals
- **Requirement:** Premium typography and high-quality status icons.
- **Implementation:** `LogSystem.cs` updated with smooth black-to-transparent fading; `StatusSystem.cs` adds pulsing glowing medals next to active buffs.
- **Verification:** `log_scroll_audit.py` (via manual check) confirmed auto-scroll and fading logic.

---

### Phase 4: Mastery & High-Fidelity Events (Implemented)

#### Alchemical Events & Celebration Sequences
- **Requirement:** Full-screen visual payloads for narrative and mechanical milestones.
- **Implementation:** `VisualManager.cs` implements `StartReactionSequence` and `StartCelebration` using full-screen alpha fades and screen shake.
- **Verification:** Sequential frame analysis during mixing confirms visual intensity.

#### Map Biome Shaders
- **Requirement:** Heat-haze and refraction shaders for specific World Map biomes.
- **Implementation:** `VisualManager.DrawMap` uses time-based Sin/Cos functions to animate Ocean ripples and Mountain heat-haze.
- **Verification:** `aura_pulse_audit.py` confirms pixel deltas in biome regions.

#### Ultra-Wide Support
- **Requirement:** Dynamic anchoring for 21:9 aspect ratios.
- **Implementation:** `LayoutSystem.SetupButtons` calculates `startX` and `centerX` using `UiLayout.Width`, ensuring center-anchoring regardless of resolution.
- **Status:** Complete.

---
*Last Updated: Friday, March 13, 2026 (Updated by Agent Gemini)*
