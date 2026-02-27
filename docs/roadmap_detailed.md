# ROADMAP_DETAILED.md - Implementation Requirements

This document provides granular technical and gameplay requirements for the unfinished goals defined in `roadmap.md`.

---

## Goal 20: Technical Perfection & Immersion

### UI Pagination
- **Requirement:** Implement a "Tab" or "Screen" controller.
- **Views:** 
    - `Tab: THE VOID`: Main manifestation and manual focus area.
    - `Tab: THE SPIRE`: Progress on the Great Project.
    - `Tab: THE WORLD`: Exploration map and landmarks.
- **Logic:** Only one tab is active at a time. The resource panel (right) and log (left) remain persistent.

### Dynamic Layout
- **Requirement:** Replace hardcoded `Rectangle` coordinates with relative logic.
- **Anchors:** Support `TopLeft`, `Center`, `BottomRight`.
- **Scaling:** UI should remain legible at 800x600 and 1920x1080 resolutions.
- **Spacing:** Implement a standard padding constant (e.g., `8px`) applied between all modular components.

### Procedural Atmosphere
- **Requirement:** Create a `ProceduralAudioSystem`.
- **Implementation:** Use `DynamicSoundEffectInstance` to generate low-frequency sine waves.
- **Dynamics:** The pitch or volume should subtly shift based on the current highest-level Essence (e.g., deeper for Earth, ethereal for Aether).

### CI Font Portability
- **Requirement:** Remove `sudo apt-get install fonts-dejavu-core` from `ci.yml`.
- **Action:** Ensure `scripts/download_font.py` is called during the `build` phase of the CI and that the MGCB points to the downloaded `.ttf` file.

---

## Goal 21: Architectural Decoupling & Stability

### EventBus
- **Requirement:** Implement a static or singleton `EventBus` class in `IncriElemental.Core`.
- **Event Types:** `ResourceChanged`, `DiscoveryUnlocked`, `ManifestationComplete`.
- **Benefit:** `ManifestationManager` shouldn't call `LogCallback` directly; it should publish a `ThingManifested` event that the `LogSystem` subscribes to.

### Save Migration
- **Requirement:** Add `public int Version { get; set; }` to `GameState`.
- **Manager:** Update `SaveManager.LoadFromFile` to check the version.
- **Migrations:** If version < current, apply a series of "upgraders" (e.g., adding missing keys to dictionaries) before returning the object.

### Auto-Healing CI (Research)
- **Requirement:** Investigate GitHub Webhooks or direct API calls from the `health` job.
- **Goal:** If `check_health.py` fails due to a monolith (>250 lines), the CI should trigger a specialized prompt to the agent to refactor that specific file.

---

## Goal 22: Deep Unfolding & Synergies

### The Constellation (Prestige Tree)
- **Requirement:** Create `ConstellationNode.cs` model.
- **Resource:** Implement `Void Embers` (earned only via Ascension).
- **Persistence:** Nodes remain unlocked across New Game+ resets.
- **UI:** A separate screen visualizing the stars and their connections.

### Landmark Exploration
- **Requirement:** Hardcode specific `CellType.Landmark` coordinates in `WorldMap.cs`.
- **Specifics:**
    - `[0,0]`: The Spire Foundation.
    - `[7,3]`: The Forbidden Library (Unlocks Lore Fragments automatically).
    - `[2,8]`: The Sunken Altar (Boosts Water Essence).

### Elemental Synergies
- **Requirement:** Add "Permanent Reaction" support to `AlchemySystem`.
- **Magma Forge:** Increases Fire/Earth cap significantly but prevents them from reaching 0.
- **Clouds:** Consumes Air/Water to periodically grant Life bursts.

---

## Goal 23: Advanced Agentic Tooling

### Efficiency Observer
- **Requirement:** A headless data-view or a hidden UI overlay.
- **Metrics:** Calculate `Time until next manifestation` for all available buttons.
- **Visualization:** Highlight the button with the lowest time-to-completion.

### Auto-Balancer V2
- **Requirement:** Update `BalanceSimulator.cs`.
- **Algorithm:**
    1. Run simulation.
    2. If total time > 20 mins, reduce costs of bottleneck resources by 5%.
    3. If total time < 15 mins, increase costs by 5%.
    4. Repeat until target window is hit.
    5. Output the final dictionary as a valid `manifestations.json`.

---
*Created: Friday, February 27, 2026*
