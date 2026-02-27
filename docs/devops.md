# DEVOPS.md - IncriElemental CI/CD & Automation

This document defines the DevOps and CI/CD strategy for **IncriElemental**, emphasizing agentic ownership.

## 1. Source Control
- **Repository:** Public GitHub Repository (Managed by the Agent).
- **Branching:** Main branch for stable releases. Feature branches can be used for significant, independent work.
- **Commits:** Every agentic task completion must be followed by a commit that clearly describes the change and its impact.

## 2. CI/CD Pipeline (Planned)
The agent is responsible for setting up and maintaining a GitHub Actions pipeline that:
- **Builds:** The entire solution on every push.
- **Tests:** Runs all unit tests in `tests/IncriElemental.Tests`.
- **Validates:** (Future) Runs headless balance simulations to ensure no regressions in game feel.

## 3. Deployment (Planned)
- **Releases:** Automated generation of GitHub Releases with platform-specific binaries (Windows, Linux, macOS).
- **Manifests:** Updating the `docs/roadmap.md` and `PLAN.md` is considered part of the "deployment" of documentation.

## 4. Agentic Responsibility
- **Pushing:** I am responsible for staging, committing, and pushing my own work to the remote repository.
- **Health:** I must monitor build/test results and fix any CI failures immediately.

## 5. Health Enforcement
The project maintains strict quality and coverage standards enforced via `scripts/check_health.py`:
- **Coverage:** Overall >= 70% (via `dotnet test --collect:"XPlat Code Coverage"`).
- **Monoliths:** No source file > 250 lines.
- **Shields:** The current health status is reflected in the `README.md` shields.

---
*Follow these patterns for all repository management.*
