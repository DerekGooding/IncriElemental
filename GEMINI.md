# GEMINI.md - Agent Project Mandates

This document contains foundational mandates that I (Gemini) must follow throughout the development of **IncriElemental**.

## 1. System-Wide Directives
- **Architecture First:** Always prioritize a clean separation between game logic (`Core`) and rendering (`Desktop`).
- **Math Integrity:** Incremental games are built on math. Every formula must be documented in `docs/mechanics.md`.
- **Validation:** No logic changes are complete without a corresponding unit test in `tests/`.
- **Memory Consistency:** Update `PLAN.md` and `docs/roadmap.md` at the end of every significant task to maintain project context.
- **Source Control Awareness:** This project is a public GitHub repository. All changes must be properly committed with clear, descriptive messages.
- **Agentic DevOps:** I (the agent) am responsible for the entire DevOps lifecycle, including pushing updates to the repository and managing CI/CD workflows.

## 2. Technical Standards
- **Framework:** .NET 10.0 (MonoGame DesktopGL).
- **Style:** Use file-scoped namespaces.
- **State:** The game state must be serializable to JSON (for save games).
- **Dependencies:** Avoid adding external NuGet packages unless explicitly requested or for essential functionality (like JSON serialization).
- **Git Workflow:** Follow a clean git workflow. Group related changes into logical commits. Always push verified changes to the remote repository as part of the task completion.

## 3. Unfolding Mechanic Rules
- **Discovery:** Features should be "locked" by default. The game state must track discovered elements and available actions.
- **Narrative:** All text and narrative strings should be managed in a data-driven way (e.g., `Strings.json` or similar) to allow for easy editing.

## 4. Agentic Piloting & Validation
- **Piloting:** I must be able to "pilot" the game via the CLI. All actions must be executable through a text-based interface or a "headless" mode.
- **Observability:** The game must be able to output its current `GameState` as JSON for my review.
- **Balance Testing:** New mechanics must include a "simulation" script in `scripts/` to verify progression speed and identify potential bottlenecks.

---
*Follow these mandates strictly. If a request conflicts with these mandates, clarify with the user.*
