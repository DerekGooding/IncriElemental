# IncriElemental - An Unfolding Incremental Game

![.NET CI](https://github.com/DerekGooding/IncriElemental/actions/workflows/ci.yml/badge.svg)
![Tests](https://img.shields.io/endpoint?url=https://raw.githubusercontent.com/DerekGooding/IncriElemental/shields/tests.json)
![Coverage](https://img.shields.io/endpoint?url=https://raw.githubusercontent.com/DerekGooding/IncriElemental/shields/project_coverage.json)
![Monoliths](https://img.shields.io/endpoint?url=https://raw.githubusercontent.com/DerekGooding/IncriElemental/shields/monoliths.json)
![Docs](https://img.shields.io/endpoint?url=https://raw.githubusercontent.com/DerekGooding/IncriElemental/shields/docs.json)

> **Note:** This project is developed with **heavy agentic support**. All code, documentation, and design decisions are a collaborative effort between the user and an AI agent (Gemini CLI), following the mandates defined in `GEMINI.md`.

**IncriElemental** is an atmospheric, unfolding incremental game built with **MonoGame** and **.NET 10.0**. You play as a consciousness awakening in a void, manifesting physical reality through elemental magic.

## 🌌 The Experience
- **Unfolding Gameplay:** Start with a single "Focus" action and gradually manifest a complex elemental engine.
- **20-Minute Loop:** A complete experience from awakening to "Ascension," with a balanced progression path.
- **Interactive Feedback:** Dynamic tooltips explain the ongoing effects of manifestations and their total scaling.
- **Scrollable UI:** A robust scrolling system with vertical button stacking ensures all manifestations are accessible regardless of resolution.
- **Full Immersion:** Support for **fullscreen mode** and a procedurally generated "void hum" background atmosphere.
- **New Game+:** Reach Ascension to earn "Cosmic Insight," providing permanent multipliers for future incarnations.

## 🛠️ Technical Highlights
- **Architecture:** Clean separation between `Core` logic and `Desktop` rendering. Logic is split into specialized systems (`ManifestationManager`, `AutomationSystem`, `LogSystem`, `WorldMapSystem`, `StatusSystem`, `EndingSystem`).
- **Health Suite:** Strict enforcement of 70% test coverage and 250-line monolith prevention via `scripts/check_health.py`.
- **Agentic DevOps:** Automated CI/CD pipeline that manages build, test, health scans, and dynamic documentation shields.
- **AI Review Mode:** Headless execution and automated screenshotting tools for agent-driven UI verification.

## 🚀 Quick Start
1.  **Clone:** `git clone https://github.com/DerekGooding/IncriElemental`
2.  **Build:** `dotnet build`
3.  **Run:** `dotnet run --project src/IncriElemental.Desktop/IncriElemental.Desktop.csproj`
4.  **Test:** `dotnet test`

## 🤖 Agentic Review
To run an automated UI review:
```powershell
python scripts/agentic_review.py focus:30 manifest:rune_of_attraction
```
The resulting snapshot will be saved to `review/screenshot.png`.

## 📜 License
This project is licensed under the **MIT License**.

---
*Last Updated: Friday, February 27, 2026 (Updated by Agent Gemini)*
