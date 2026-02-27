# DEVOPS.md - IncriElemental CI/CD & Automation

This document defines the DevOps and CI/CD strategy for **IncriElemental**, emphasizing agentic ownership.

## 1. Source Control
- **Repository:** Public GitHub Repository (Managed by the Agent).
- **Branching:** Main branch for stable releases. Feature branches can be used for significant, independent work.
- **Commits:** Every agentic task completion must be followed by a commit that clearly describes the change and its impact.

## 2. CI/CD Pipeline
The agent maintains a GitHub Actions pipeline (`.github/workflows/ci.yml`) that:
- **Builds:** The entire solution on every push using .NET 10.0.
- **Tests:** Runs all unit tests and collects code coverage.
- **Visual Review:** Executes `scripts/agentic_review.py` via `xvfb` to catch UI regressions.
- **Health:** Executes `scripts/check_health.py` to enforce quality mandates.
- **Shields:** Automatically updates health data on the `shields` branch for dynamic status badges.

## 3. Deployment
- **Releases:** (Future) Automated generation of GitHub Releases with platform-specific binaries (Windows, Linux, macOS).
- **Manifests:** Updating the `docs/roadmap.md`, `GEMINI.md`, and other documentation is considered part of the "deployment" of project context and is required when files become "stale."

## 4. Agentic Responsibility
- **Pushing:** I am responsible for staging, committing, and pushing my own work to the remote repository.
- **Health:** I must monitor build/test results and fix any CI failures or health regressions immediately. I am prohibited from "touching" documentation files to bypass staleness checks; I must provide actual content updates.

## 5. Health Enforcement
The project maintains strict quality and coverage standards enforced via `scripts/check_health.py`:
- **Coverage:** Overall >= 70% (via `dotnet test --collect:"XPlat Code Coverage"`).
- **Monoliths:** No source file > 250 lines. (Refactored `Game1.cs` to use specialized systems).
- **Documentation Staleness:** Documentation files (README.md, docs/*.md, etc.) are monitored for staleness. A doc is considered "stale" if more than **8 source files** have been changed since its last update. Staleness requires a manual review and update of the document's content.
- **Shields:** The current health status is reflected in the `README.md` shields using data deployed by the CI pipeline.

---
*Follow these patterns for all repository management.*
