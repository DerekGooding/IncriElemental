# Project Suggestions & Future Improvements

This document outlines my professional review of **IncriElemental** and provides a roadmap for future technical, gameplay, and agentic improvements.

## 📋 Technical Review
The current architecture is solid but will face scalability challenges as the game expands beyond 20 minutes.

### 1. Data-Driven Manifestations
**Current Issue:** `ManifestationManager` uses a large `switch` statement.
**Suggestion:** Move manifestation definitions (costs, effects, discoveries) into a `manifestations.json` file. Create a `ManifestationDefinition` model to parse this at runtime. This will eliminate switch-based monoliths and allow for easier balance tuning.

### 2. Input & UI Refactoring
**Current Issue:** `Game1.cs` still handles mouse clicks by iterating through a list of buttons.
**Suggestion:** Implement an `InputManager` to handle mouse/keyboard events. Move the `Button` list and layout logic into a more robust `ScreenManager` or `UIManager` that can handle multiple "pages" or "tabs" (e.g., a tab for "The Void" and another for "The Spire").

### 3. Content Pipeline Optimization
**Current Issue:** The CI uses a system-font copy hack (`Arimo.ttf`) to build the spritefont.
**Suggestion:** License/bundle a specific pixel font directly in the repository to remove dependencies on Ubuntu's native font packages.

---

## 🎮 Gameplay & Narrative Additions
To move beyond the 20-minute loop, the "unfolding" needs more layers.

### 1. Elemental Reactions (The Alchemist)
**Suggestion:** Add a "Mix" mechanic where players can combine active essences to produce temporary boosts.
- *Fire + Air:* "Combustion" (Global speed +50% for 30s).
- *Water + Earth:* "Fertility" (Life gen +100% for 60s).

### 2. Interactive World Map
**Suggestion:** As the Spire grows, reveal a minimalist grid-based map. Players can "send" Familiars to explore the void, finding rare essences or lore fragments.

### 3. Sound & Atmosphere
**Suggestion:** Implement a minimalist procedural soundtrack (using simple sine waves or a library like `DynamicSoundEffectInstance`). Add "click" sounds for UI and "magical hums" for passive generation.

---

## 🤖 Agentic Optimizations
The current agentic support is excellent, but can be further automated.

### 1. Auto-Balancing Tool
**Suggestion:** Update the `BalanceSimulator` to not just *verify* balance, but *suggest* values. A script could run 100 simulations with varied costs and find the "sweet spot" for a 20-minute loop automatically.

### 2. Auto-Healing CI
**Suggestion:** Add a GitHub Action that triggers me (Gemini) when a health check fails. I could automatically refactor a file that exceeds 250 lines and submit a PR with the fix.

### 3. Visual Regression Testing
**Suggestion:** The `agentic_review.py` script captures screenshots. We can implement a "Visual Diff" tool that compares the latest screenshot against a "Golden Master" to catch accidental UI regressions.

---
*End of Review.*
