# DEVOPS.md - IncriElemental CI/CD & Automation

This document defines the DevOps and CI/CD strategy for **IncriElemental**, emphasizing agentic ownership.

## 1. CI/CD Pipeline
Managed via GitHub Actions (`.github/workflows/ci.yml`) on **Windows**:
- **Builds:** Solution-wide compilation on every push.
- **Tests:** XUnit suite with code coverage tracking (70% minimum).
- **Health Scan:** Enforces monolith prevention and documentation freshness.
- **Shields Deployment:** Updates README badges via a dedicated `shields` branch.

## 2. Advanced Agentic Auditing
Goal 31 introduced a suite of visual verification tools that allow the agent to "see" and "audit" the game's aesthetic state:
- **`palette_audit.py`:** Ensures all screenshots adhere to the "Aetherial Glow" color profiles.
- **`contrast_check.py`:** Verifies WCAG 2.1 accessibility for UI text against dynamic backgrounds.
- **`parallax_audit.py`:** Confirms independent movement of background layers and screen shake effects.
- **`aura_pulse_audit.py`:** Verifies animation smoothness for UI borders and map shaders.
- **`aesthetic_audit.py`:** Calculates a "Glow Score" to ensure atmosphere scales with production.

## 3. Metadata-Driven Verification
Instead of fragile pixel-matching, the agent uses **UI Metadata** exported by `AiModeSystem.cs`:
- **State Analysis:** Confirms text, visibility, and coordinates of all UI elements.
- **Performance Profiling:** Tracks frame-time and total-time to detect regressions during heavy particle effects.

---
*Last Updated: Friday, March 13, 2026 (Updated by Agent Gemini)*
