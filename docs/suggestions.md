# Project Suggestions: The Autonomous Awakening

This document contains 20 suggestions focused on empowering the AI Agent to build, verify, and polish **IncriElemental** using visual feedback systems.

## 🤖 Visual Agentic Suggestions

### 1. OCR-Powered Menu Verification
Implement a Python-based OCR (Optical Character Recognition) layer in `agentic_review.py` to allow the AI to confirm that specific buttons (e.g., "Manifest Speck") actually appear on screen after a command.

### 2. Semantic Pixel Comparison
Move beyond simple pixel-diffing. Implement a "Semantic Review" where the AI analyzes screenshots to detect if UI elements are overlapping or if text is clipping outside of button bounds.

### 3. Automated Color Palette Audit
The AI should use screenshots to verify that all manifested elements adhere to the "Aetherial Glow" palette (defined in `ColorPalette.cs`), ensuring visual consistency across new features.

### 4. Dynamic Layout Stress Testing
The AI uses `update_screenshots.py` with extreme UI scaling (0.5x and 2.0x) to verify that the scrolling and anchoring systems handle all resolutions without breaking.

### 5. Particle Density Validation
Implement a system where the AI captures short GIF/Video clips (or sequential screenshots) to analyze particle density, ensuring performance doesn't degrade during high-production phases.

### 6. Tooltip Pinning Verification
Agentic tests should verify that the "Tooltip Pinning" feature works by hovering, pinning, moving the mouse, and confirming the tooltip remains visible in a screenshot.

### 7. Resource Flow Graph Readability
The AI reviews `spire_flow.png` to ensure that node labels are not overlapping and that production lines are clearly visible against the background.

### 8. Automated "Void" Contrast Check
AI analyzes screenshots to ensure text contrast remains high enough for accessibility against the dynamic starfield and bloom effects.

### 9. Component-Driven UI Generation
AI implements new UI components (e.g., a "Sparkline" for production) and uses screenshots to verify the component renders correctly before submitting the PR.

### 10. Tutorial Highlight Accuracy
Agentic review captures screenshots during the Tutorial phase to ensure the "Dimming" effect correctly highlights the intended button and doesn't obscure critical information.

### 11. World Map Aura Visualization
AI verifies that Auras on the World Map are visually distinct and that their "pulse" effect is captured correctly in sequential screenshots.

### 12. Alchemical Reaction Visuals
When a new alchemical mix is added, the AI must capture the "Success" animation and verify the resulting buff icon appears in the Status Bar.

### 13. Resolution-Independent Font Rendering
AI compares screenshots of the same UI at different window sizes to ensure `RichTextSystem` scaling doesn't result in blurry or unreadable text.

### 14. "Ascension" Visual Payload
The AI verifies the transition to the white-out Ascension screen by checking if the average pixel brightness of a screenshot exceeds a specific threshold.

### 15. Status Bar Overflow Management
AI fast-forwards the game to a state with 10+ active buffs and takes a screenshot to verify the Status Bar wraps or scales correctly.

### 16. Icon Sprite Alignment
Automated check to ensure that inline icons in `RichTextSystem` are vertically centered with their surrounding text.

### 17. Button State Feedback (Hover/Click)
AI captures screenshots of "Idle," "Hover," and "Click" states for manifestations to ensure visual feedback is responsive and consistent.

### 18. Narrative Log Auto-Scroll Verification
AI fills the log with text and verifies via screenshot that the most recent entry is visible at the bottom of the log area.

### 19. Background Starfield Parallax Audit
AI captures screenshots while the camera "shakes" (e.g., during manifestation) to verify that background layers move independently as intended.

### 20. Automated Aesthetic Grading
AI uses a lightweight vision model (or heuristic-based image analysis) to "grade" a scene's atmosphere, ensuring the Bloom intensity matches the current Aether production level.

---
*Last Updated: Friday, March 13, 2026 (Updated by Agent Gemini)*
