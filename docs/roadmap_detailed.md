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

### Phase 3: Dynamic Perception & Interaction

#### 11. Tooltip Pinning
- **Sub-task 11.1:** Hover over a manifestation button.
- **Sub-task 11.2:** Issue the `pin` command (Right-click or 'P').
- **Sub-task 11.3:** Move the mouse to the opposite corner of the screen and capture.
- **Sub-task 11.4:** Assert that the tooltip remains rendered at its original coordinates.

#### 12. Graph Readability
- **Sub-task 12.1:** Navigate to the `Flow` tab.
- **Sub-task 12.2:** Analyze the pixel density of the `FlowSystem` graph.
- **Sub-task 12.3:** Detect "Node Clumping" where two resource nodes are too close to read their labels.
- **Sub-task 12.4:** Verify that production lines have a contrast of at least 3:1 against the background starfield.

#### 13. Tutorial Audit
- **Sub-task 13.1:** Implement a "Step-by-Step" capture mode for the Tutorial.
- **Sub-task 13.2:** For each step, identify the "Highlight Mask" area.
- **Sub-task 13.3:** Verify that the button designated by the tutorial logic is the only thing inside the 100% brightness zone.
- **Sub-task 13.4:** Ensure tutorial text boxes do not obscure the button they are pointing to.

#### 14. Aura Pulse Validation
- **Sub-task 14.1:** Capture a 1-second sequence of screenshots (4 frames) of the World Map.
- **Sub-task 14.2:** Isolate the border pixels of an explored cell with an Aura.
- **Sub-task 14.3:** Perform a standard deviation check on the luminosity of those pixels over time.
- **Sub-task 14.4:** Assert that the luminosity varies by at least 15% (verifying the `Math.Sin` pulse logic).

#### 15. Reaction Visuals
- **Sub-task 15.1:** Initiate a "Combustion" reaction via AI command.
- **Sub-task 15.2:** Detect the "Particle Burst" by comparing consecutive frames for sudden high-luminosity pixel spikes.
- **Sub-task 15.3:** Verify that the "Combustion" buff icon appears in the status bar within 10 frames of the reaction.
- **Sub-task 15.4:** Verify the log entry has the correct `[color]` tag via OCR.

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
