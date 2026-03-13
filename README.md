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
- **Guided Onboarding:** A built-in **Tutorial System** dims the void and highlights key manifestations to help new consciousnesses awaken.
- **Visual Depth:** Experience the void through a **dynamic starfield background** and high-fidelity **HLSL Bloom post-processing**.
- **Localized Interface:** Full support for externalized strings via a data-driven **Localization System**.
- **20-Minute Loop:** A complete experience from awakening to "Ascension," with a balanced progression path.
- **Interactive Feedback:** Dynamic tooltips with **Rich Text and Inline Icons** explain the ongoing effects of manifestations and their total scaling.
- **Spatial Synergies:** The World Map now features **Auras**, where manifestations project power to neighboring cells, creating complex production harmonies.
- **Alchemical Mixing:** A dedicated **Mixing Table** allows players to combine elemental essences for temporary high-potency buffs.
- **Scrollable UI:** A robust scrolling system with vertical button stacking ensures all manifestations are accessible regardless of resolution.
- **Full Immersion:** Support for **fullscreen mode** and a procedurally generated "void hum" background atmosphere.
- **New Game+:** Reach Ascension to earn "Cosmic Insight," providing permanent multipliers for future incarnations.

## 🛠️ Technical Highlights
- **Architecture:** Clean separation between `Core` logic and `Desktop` rendering. `Game1.cs` is a thin coordinator delegating to specialized systems (`ManifestationManager`, `AlchemySystem`, `LogSystem`, `WorldMapSystem`, `StatusSystem`, `EndingSystem`, `TutorialSystem`, `BackgroundManager`).
- **Component-Driven Manifestations:** A flexible, ECS-lite architecture for manifestations using specialized components (`Producer`, `Storage`, `Aura`, `Unlock`) defined in JSON.
- **Rich Text Engine:** Custom `RichTextSystem` in the Desktop layer supports tag-based formatting (`[color]`, `[i:icon]`) for tooltips and logs.
- **Graphics Pipeline:** Custom HLSL shaders for bloom effects and a reactive background system tied to resource generation rates.
- **Localization:** Centralized `TextService` in `Core` manages dynamic string injection from `strings.json`.
- **Health Suite:** Strict enforcement of 70% test coverage, 250-line monolith prevention, and comprehensive health monitoring via `scripts/check_health.py`.
- **Agentic DevOps:** Automated CI/CD pipeline on **Windows** that manages build, test, health scans, and dynamic documentation shields.
- **AI Review Mode:** Automated screenshotting tools for agent-driven UI verification.

## 🚀 Quick Start
1.  **Clone:** `git clone https://github.com/DerekGooding/IncriElemental`
2.  **Build:** `dotnet build`
3.  **Run:** `dotnet run --project src/IncriElemental.Desktop/IncriElemental.Desktop.csproj`
4.  **Test:** `dotnet test`
5.  **Health Check:** `python scripts/check_health.py`

## 🤖 Agentic Review
To run an automated UI review:
```powershell
python scripts/agentic_review.py focus:30 manifest:rune_of_attraction
```
The resulting snapshot will be saved to `review/screenshot.png`.

## 📜 License
This project is licensed under the **MIT License**.

---
*Last Updated: Friday, March 13, 2026 (Updated by Agent Gemini)*
