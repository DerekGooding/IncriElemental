# GEMINI.md - Agent Project Mandates

This document contains foundational mandates that I (Gemini) must follow throughout the development of **IncriElemental**.

## 1. System-Wide Directives
- **Architecture First:** Prioritize separation between `Core` logic and `Desktop` rendering.
- **Agentic DevOps:** I am responsible for the entire build, test, and visual audit lifecycle.
- **Validation:** No change is complete without passing unit tests AND visual health audits.

## 2. Technical Standards
- **Framework:** .NET 10.0 (MonoGame DesktopGL).
- **UI:** Glassmorphism panels with runic accents and dynamic anchoring.
- **Visuals:** High-fidelity "Aetherial Glow" using HLSL Bloom and multi-layered parallax.
- **Verification:** Utilize UI Metadata (JSON) for state validation to avoid pixel-match fragility.

## 3. Visual Integrity Mandate
- **Automated Gallery:** Maintain up-to-date screenshots in `review/` and `docs/screenshots.md`.
- **Audit Suite:** Run `scripts/palette_audit.py`, `scripts/contrast_check.py`, and `scripts/parallax_audit.py` after significant visual changes.
- **Staleness:** Visuals are considered "stale" if more than **16 UI/Visual source files** have changed since the last capture.

## 4. Project Health & Quality
- **Test Coverage:** Overall coverage must remain above **70%**.
- **Monolith Prevention:** No single source file (`.cs`) should exceed **250 lines**.
- **Documentation FRESHNESS:** Documentation is stale if more than **8 source files** have changed. Resolve by providing meaningful content improvements.

---
*Last Updated: Friday, March 13, 2026 (Updated by Agent Gemini)*
