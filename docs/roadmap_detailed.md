# ROADMAP_DETAILED.md - Implementation Requirements

This document provides granular technical and gameplay requirements for the unfinished goals defined in `roadmap.md`.

---

## Goal 30: The Autonomous Awakening (InProgress)

### Phase 1: Perception & Standardized Vision (Implemented)

#### 1. OCR Verification (Pivoted to Metadata Verification)
- **Requirement:** Confirm UI text presence via exported metadata.
- **Implementation:** Added `SaveMetadata` to `AiModeSystem.cs` and verification logic to `agentic_review.py`.
- **Status:** Complete.

#### 2. Palette Audit
- **Requirement:** Enforce the "Aetherial Glow" color scheme.
- **Implementation:** `scripts/palette_audit.py` parses `ColorPalette.cs` and samples screenshots for compliance.
- **Status:** Complete.

#### 3. Contrast Check
- **Requirement:** Accessibility audit for the Void.
- **Implementation:** `scripts/contrast_check.py` calculates WCAG contrast ratios using UI metadata and screenshot sampling.
- **Status:** Complete.

#### 4. Icon Alignment
- **Requirement:** Pixel-perfect rich text.
- **Implementation:** `scripts/icon_alignment.py` verifies presence and formatting of icon tags in tooltips.
- **Status:** Complete.

#### 5. Button States
- **Requirement:** Visual feedback responsiveness.
- **Implementation:** Added `hover` command to `AiModeSystem` and `scripts/button_state_audit.py` for visual delta analysis.
- **Status:** Complete.

---

### Phase 2: Structural Integrity & Scalability (Implemented)

#### 6. Semantic Review
- **Requirement:** Detect UI overlaps and clipping.
- **Implementation:** `scripts/semantic_review.py` uses metadata to detect coordinate collisions between buttons.
- **Status:** Complete.

#### 7. Stress Testing
- **Requirement:** Verify UI robustness at scale.
- **Implementation:** Enhanced `AiModeSystem.cs` with `key` commands to automate UI scaling tests.
- **Status:** Complete.

#### 8. Font Scaling
- **Requirement:** Verify `RichTextSystem` legibility.
- **Implementation:** Scaling logic integrated into `VisualManager.cs` and `InputManager.cs` to ensure resolution independence.
- **Status:** Complete.

#### 9. Status Bar Scaling
- **Requirement:** Handle many concurrent buffs.
- **Implementation:** `StatusSystem.cs` handles dynamic buff rendering, verified via automated metadata checks.
- **Status:** Complete.

#### 10. Log Auto-Scroll
- **Requirement:** Ensure latest events are visible.
- **Implementation:** `LogSystem.cs` auto-scroll logic verified through sequential metadata capture of high-frequency events.
- **Status:** Complete.

---

### Phase 3: Dynamic Perception & Interaction (Implemented)

#### 11. Tooltip Pinning
- **Requirement:** Verify persistence of pinned rich text.
- **Implementation:** `AiModeSystem.cs` supports `pin` command; verified by simulating hover followed by mouse movement and checking tooltip metadata.
- **Status:** Complete.

#### 12. Graph Readability
- **Requirement:** Ensure `FlowSystem` nodes are legible.
- **Implementation:** `scripts/graph_audit.py` analyzes visual density and contrast of flow graph screenshots.
- **Status:** Complete.

#### 13. Tutorial Audit
- **Requirement:** Verify "Highlight" focus.
- **Implementation:** `scripts/tutorial_audit.py` detects the high-brightness mask used to highlight tutorial objectives.
- **Status:** Complete.

#### 14. Aura Pulse Validation
- **Requirement:** Verify animation of World Map influences.
- **Implementation:** `scripts/aura_pulse_audit.py` performs multi-frame delta analysis to verify mathematical pulse logic in rendering.
- **Status:** Complete.

#### 15. Reaction Visuals
- **Requirement:** Capture successful Alchemical mixes.
- **Implementation:** `scripts/reaction_audit.py` verifies the resulting buff metadata after an AI-triggered alchemical reaction.
- **Status:** Complete.

---

### Phase 4: Atmospheric Fidelity & Performance (Implemented)

#### 16. Performance Analysis
- **Requirement:** Track particle-induced frame-time spikes.
- **Implementation:** Added `Performance` metrics to `AiModeSystem.cs` metadata export for automated frame-time analysis.
- **Status:** Complete.

#### 17. Visual Component Testing
- **Requirement:** Unit test for UI rendering.
- **Implementation:** `agentic_review.py` integrated bit-for-bit baseline comparison for UI regression testing.
- **Status:** Complete.

#### 18. Ascension Brightness
- **Requirement:** Verify the "White-Out" effect.
- **Implementation:** `scripts/ascension_audit.py` calculates average screenshot brightness during the Ascension transition.
- **Status:** Complete.

#### 19. Parallax Audit
- **Requirement:** Depth during screen shake.
- **Implementation:** `scripts/parallax_audit.py` verifies pixel delta between frames during high-intensity camera shake events.
- **Status:** Complete.

#### 20. Aesthetic Grading
- **Requirement:** Automated atmosphere review.
- **Implementation:** `scripts/aesthetic_audit.py` uses a glow heuristic to verify atmospheric intensity scales with production.
- **Status:** Complete.

---
*Last Updated: Friday, March 13, 2026 (Updated by Agent Gemini)*
