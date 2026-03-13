# ROADMAP_DETAILED.md - Implementation Requirements

This document provides granular technical and gameplay requirements for the unfinished goals defined in `roadmap.md`.

---

## Goal 30: The Autonomous Awakening (InProgress)

### 1. OCR Verification
- **Requirement:** Confirm UI text presence via screenshots.
- **Step 1:** Integrate `pytesseract` or a similar OCR library into `scripts/agentic_review.py`.
- **Step 2:** Add a `--verify-text "STRING"` argument to the review script.
- **Step 3:** After capturing a screenshot, run OCR on the image and assert that the string exists.
- **Step 4:** Integrate this check into the health suite.

### 2. Semantic Review
- **Requirement:** Detect UI overlaps and clipping.
- **Step 1:** Enhance `agentic_review.py` to identify "high-density" pixel areas that don't match expected layouts.
- **Step 2:** Use edge detection (Sobel/Canny) to find button boundaries in screenshots.
- **Step 3:** Compare detected boundaries against the `LayoutSystem` coordinate logic.
- **Step 4:** Flag any overlapping rectangles as health failures.

### 3. Palette Audit
- **Requirement:** Enforce the "Aetherial Glow" color scheme.
- **Step 1:** Parse `src/IncriElemental.Desktop/Visuals/ColorPalette.cs` to extract allowed RGBA values.
- **Step 2:** Implement a script that samples pixel colors from generated screenshots.
- **Step 3:** Assert that >95% of non-background pixels match the defined palette (allowing for bloom gradients).
- **Step 4:** Generate a report of "Off-Palette" colors found.

### 4. Stress Testing
- **Requirement:** Verify UI robustness at scale.
- **Step 1:** Update `scripts/update_screenshots.py` to loop through scales `[0.5, 1.0, 1.5, 2.0]`.
- **Step 2:** Issue `[ ]` scaling commands via `AiModeSystem`.
- **Step 3:** Verify that the scrollable area in `Game1.Draw` correctly adjusts its `ScissorRectangle`.
- **Step 4:** Confirm buttons remain clickable by checking their transformed bounds.

### 5. Performance Analysis
- **Requirement:** Track particle-induced frame-time spikes.
- **Step 1:** Add a hidden "Performance" counter to `AiModeSystem` that logs `ElapsedGameTime`.
- **Step 2:** Capture 10 sequential screenshots during a "Combustion" event.
- **Step 3:** Analyze the delta between captures to estimate real-world FPS.
- **Step 4:** Fail health check if FPS drops below 30 in AI mode.

### 6. Tooltip Pinning
- **Requirement:** Verify persistence of pinned rich text.
- **Step 1:** Command AI to `hover:speck` then `pin`.
- **Step 2:** Command AI to `move:0,0` (moving mouse away).
- **Step 3:** Capture screenshot and use OCR/Template Matching to confirm the tooltip is still rendered.
- **Step 4:** Command `unpin` and verify it disappears.

### 7. Graph Readability
- **Requirement:** Ensure `FlowSystem` nodes are legible.
- **Step 1:** Capture `spire_flow.png`.
- **Step 2:** Use Python's `PIL` to check for white-on-white or dark-on-dark text collisions.
- **Step 3:** Verify that "Focus" and "Aether" nodes have a minimum distance of 100 pixels.
- **Step 4:** Ensure production lines do not cross directly through text labels.

### 8. Contrast Check
- **Requirement:** Accessibility audit for the Void.
- **Step 1:** Sample text color vs. background color in `void_main.png`.
- **Step 2:** Calculate contrast ratio (WCAG standard).
- **Step 3:** Assert a minimum 4.5:1 ratio for all resource labels.
- **Step 4:** Adjust `ColorPalette.cs` automatically if contrast fails.

### 9. Visual Component Testing
- **Requirement:** Unit test for UI rendering.
- **Step 1:** Create a "test manifestation" with all possible components (Aura, Producer, Storage).
- **Step 2:** Render only that button to a clean `RenderTarget`.
- **Step 3:** Compare against a `baseline_component.png`.
- **Step 4:** Fail if the rendering logic in `Button.cs` changes unexpectedly.

### 10. Tutorial Audit
- **Requirement:** Verify "Highlight" focus.
- **Step 1:** Launch game in Tutorial mode.
- **Step 2:** Capture screenshot during the "Focus on Focus" step.
- **Step 3:** Assert that the "Focus" button area has 100% brightness while the rest of the screen is < 30%.
- **Step 4:** Verify the highlight moves correctly to the "Speck" button.

### 11. Aura Pulse Validation
- **Requirement:** Verify animation of World Map influences.
- **Step 1:** Capture two screenshots of the World Map 250ms apart.
- **Step 2:** Perform a pixel-diff on the Aura borders.
- **Step 3:** Assert that the borders have moved/changed intensity (verifying the `pulse` logic in `VisualManager.DrawMap`).
- **Step 4:** Fail if the Aura is static.

### 12. Reaction Visuals
- **Requirement:** Capture successful Alchemical mixes.
- **Step 1:** Command AI to `update:7200` and `mix:fire,air`.
- **Step 2:** Capture screenshot immediately after the `mix` command.
- **Step 3:** Verify presence of "Combustion" buff in the `StatusSystem` area.
- **Step 4:** Confirm the log entry "Combustion initiated!" appears via OCR.

### 13. Font Scaling
- **Requirement:** Verify `RichTextSystem` legibility.
- **Step 1:** Render a long string with multiple tags `[c:gold][i:aether]`.
- **Step 2:** Capture at 720p and 1080p.
- **Step 3:** Use structural similarity index (SSIM) to ensure font glyphs are not distorted.
- **Step 4:** Verify icons scale proportionally with text.

### 14. Ascension Brightness
- **Requirement:** Verify the "White-Out" effect.
- **Step 1:** Command AI to `manifest:ascend`.
- **Step 2:** Wait 2 seconds for transition.
- **Step 3:** Capture screenshot.
- **Step 4:** Assert that the average R, G, and B values are all > 240.

### 15. Status Bar Scaling
- **Requirement:** Handle many concurrent buffs.
- **Step 1:** Inject 12 temporary buffs into `GameState`.
- **Step 2:** Capture screenshot of the status bar.
- **Step 3:** Verify that icons either wrap to a second line or scale down to fit.
- **Step 4:** Ensure no icons are clipped by the screen edge.

### 16. Icon Alignment
- **Requirement:** Pixel-perfect rich text.
- **Step 1:** Capture a high-res screenshot of a tooltip containing `[i:aether]`.
- **Step 2:** Find the bounding box of the icon and the adjacent text.
- **Step 3:** Assert that the vertical mid-point of the icon aligns within 2 pixels of the text baseline mid-point.
- **Step 4:** Adjust `RichTextSystem.Draw` offset if misaligned.

### 17. Button States
- **Requirement:** Visual feedback responsiveness.
- **Step 1:** Capture button at `Point(0,0)` (Idle).
- **Step 2:** Move mouse to button center, capture (Hover).
- **Step 3:** Assert that Hover screenshot is brighter or has a border change compared to Idle.
- **Step 4:** Verify `IsHovered` flag is true in AI debug metadata.

### 18. Log Auto-Scroll
- **Requirement:** Ensure latest events are visible.
- **Step 1:** Command AI to `focus` 50 times to fill the log.
- **Step 2:** Capture screenshot of the `LogSystem` area.
- **Step 3:** Use OCR to verify the 50th "Focus" message is present.
- **Step 4:** Scroll up and verify previous messages appear.

### 19. Parallax Audit
- **Requirement:** Depth during screen shake.
- **Step 1:** Set `ScreenShakeIntensity = 20`.
- **Step 2:** Capture two frames during the shake.
- **Step 3:** Verify that the starfield stars moved less than the UI buttons (verifying layer separation).
- **Step 4:** Confirm the "Void" feels deep and separate from the interface.

### 20. Aesthetic Grading
- **Requirement:** Automated atmosphere review.
- **Step 1:** Implement a heuristic that scores "Glow" based on the ratio of saturated pixels to dark pixels.
- **Step 2:** Capture screenshots at 0, 1000, and 10000 Aether.
- **Step 3:** Assert that the "Glow Score" increases as Aether production scales.
- **Step 4:** Fail if the Void feels "flat" during late-game play.

---
*Last Updated: Friday, March 13, 2026 (Updated by Agent Gemini)*
