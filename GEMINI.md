# GEMINI.md - Agent Project Mandates

This document contains foundational mandates that I (Gemini) must follow throughout the development of **IncriElemental**.

## 1. System-Wide Directives
- **Architecture First:** Always prioritize a clean separation between game logic (`Core`) and rendering (`Desktop`).
- **Math Integrity:** Incremental games are built on math. Every formula must be documented in `docs/mechanics.md`.
- **Validation:** No logic changes are complete without a corresponding unit test in `tests/`.
- **Memory Consistency:** Update `PLAN.md` and `docs/roadmap.md` at the end of every significant task to maintain project context.

## 2. Technical Standards
- **Framework:** .NET 10.0 (MonoGame DesktopGL).
- **Style:** Use file-scoped namespaces.
- **State:** The game state must be serializable to JSON (for save games).
- **Dependencies:** Avoid adding external NuGet packages unless explicitly requested or for essential functionality (like JSON serialization).

## 3. Unfolding Mechanic Rules
- **Discovery:** Features should be "locked" by default. The game state must track discovered elements and available actions.
- **Narrative:** All text and narrative strings should be managed in a data-driven way (e.g., `Strings.json` or similar) to allow for easy editing.

---
*Follow these mandates strictly. If a request conflicts with these mandates, clarify with the user.*
