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

### Phase 4: Atmospheric Fidelity & Performance

#### 16. Performance Analysis
- **Sub-task 16.1:** Add a `--profile` flag to `agentic_review.py`.
- **Sub-task 16.2:** Measure the real-world time between `BeginRenderToTarget` and `EndRenderToTarget`.
- **Sub-task 16.3:** Stress the engine with 5000+ particles and check for frame-time deviation.
- **Sub-task 16.4:** Log performance regressions if the "Aetherial Glow" post-processing costs exceed 4ms per frame.

#### 17. Visual Component Testing
- **Sub-task 17.1:** Setup a "Component Lab" scene that renders every UI primitive in isolation.
- **Sub-task 17.2:** Capture baseline images for every button type, resource icon, and scrollbar.
- **Sub-task 17.3:** Run a bit-for-bit comparison on every commit that touches `UI/` or `Visuals/`.
- **Sub-task 17.4:** Provide a "Visual Diff" artifact in GitHub Actions when tests fail.

#### 18. Ascension Brightness
- **Sub-task 18.1:** Reach the Ascension state via AI fast-forward.
- **Sub-task 18.2:** Capture the screen during the transition peak.
- **Sub-task 18.3:** Calculate the global average pixel brightness.
- **Sub-task 18.4:** Assert that brightness >= 245/255 (the "Pure White" requirement).

#### 19. Parallax Audit
- **Sub-task 19.1:** Trigger a "Screen Shake" event.
- **Sub-task 19.2:** Use optical flow analysis to track the movement vectors of UI elements vs. background stars.
- **Sub-task 19.3:** Verify that background movement vectors are significantly shorter than UI movement vectors.
- **Sub-task 19.4:** Fail if the background is "sticky" to the UI.

#### 20. Aesthetic Grading
- **Sub-task 20.1:** Implement a "Glow Heuristic" based on the ratio of Bloom pixels to non-Bloom pixels.
- **Sub-task 20.2:** Sample screenshots at different stages of production.
- **Sub-task 20.3:** Ensure the "Atmospheric Density" (Glow Score) correlates positively with Aether generation rates.
- **Sub-task 20.4:** Verify the scene doesn't look "washed out" (too much white) or "empty" (too much black) at any stage.

---
*Last Updated: Friday, March 13, 2026 (Updated by Agent Gemini)*
