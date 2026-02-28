# GEMINI.md - Agent Project Mandates

This document contains foundational mandates that I (Gemini) must follow throughout the development of **IncriElemental**.

## 1. System-Wide Directives
- **Architecture First:** Always prioritize a clean separation between game logic (`Core`) and rendering (`Desktop`).
- **Decoupled Events:** Utilize the `EventBus` for publishing and subscribing to significant game events.
- **Math Integrity:** Incremental games are built on math. Every formula must be documented in `docs/mechanics.md`.
- **Validation:** No logic changes are complete without a corresponding unit test in `tests/`.
- **Memory Consistency:** Update `PLAN.md` and `docs/roadmap.md` at the end of every significant task.
- **Source Control Awareness:** All changes must be properly committed with clear, descriptive messages.
- **Agentic DevOps:** I (the agent) am responsible for the entire DevOps lifecycle.

## 2. Technical Standards
- **Framework:** .NET 10.0 (MonoGame DesktopGL).
- **Style:** Use file-scoped namespaces.
- **State:** The game state must be serializable to JSON.
- **UI Responsiveness:** All UI elements must use the `UiLayout` system for relative positioning and support scrollable viewports via `ScissorRectangle` clipping when content overflows.
- **Dependencies:** Avoid adding external NuGet packages unless essential.
- **Git Workflow:** Follow a clean git workflow.

## 3. Unfolding Mechanic Rules
- **Discovery:** Features should be "locked" by default.
- **Interactive Feedback:** Manifestations must include dynamic tooltips detailing their ongoing production rates and total scaling.
- **Narrative:** All text strings should be managed in a data-driven way.

## 4. Agentic Piloting & Validation
- **Piloting:** Issue commands via CLI or "headless" mode.
- **Observability:** Output current `GameState` as JSON for review.
- **Balance Testing:** Use `BalanceSimulator.cs` to verify progression speed.

## 5. Project Health & Quality
- **Test Coverage:** Overall coverage must remain above **70%**.
- **Monolith Prevention:** No single source file (`.cs`) should exceed **250 lines**.
- **Documentation Staleness:** Documentation is considered "stale" if more than **8 source files** have changed since its last update.
- **Health Integrity:** I must resolve staleness by providing actual content improvements, never by "touching" files.
- **Automated Checks:** Run `scripts/check_health.py` before completing any significant feature.

### Resolving Health Failures
1. **Monoliths:** Refactor into specialized systems (e.g., `EndingSystem`, `StatusSystem`).
2. **Coverage:** Add unit tests targeting uncovered branches.
3. **Staleness:** Review source changes and update the corresponding `.md` files.
4. **Build/Test Errors:** Fix root causes immediately.

## 6. Playability Goal (Current)
- **Target:** A polished 20-minute loop ending with "Ascension," followed by a meaningful **New Game+** experience via the "Constellation" prestige tree.
- **Focus:** Early-game tactile (Focus), mid-game automated (Spire), late-game strategic (Constellation).
- **Verification:** Confirm timing using the simulator.

---
*Follow these mandates strictly. If a request conflicts with these mandates, clarify with the user.*

*Last Updated: Friday, February 27, 2026 (Updated by Agent Gemini)*
