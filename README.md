# IncriElemental - An Unfolding Incremental Game

![.NET CI](https://github.com/DerekGooding/IncriElemental/actions/workflows/ci.yml/badge.svg)
![Tests](https://img.shields.io/endpoint?url=https://raw.githubusercontent.com/DerekGooding/IncriElemental/shields/tests.json)
![Coverage](https://img.shields.io/endpoint?url=https://raw.githubusercontent.com/DerekGooding/IncriElemental/shields/project_coverage.json)
![Monoliths](https://img.shields.io/endpoint?url=https://raw.githubusercontent.com/DerekGooding/IncriElemental/shields/monoliths.json)
![Docs](https://img.shields.io/endpoint?url=https://raw.githubusercontent.com/DerekGooding/IncriElemental/shields/docs.json)

**IncriElemental** is an atmospheric, unfolding incremental game built with **MonoGame** and **.NET 10.0**. You play as a consciousness awakening in a void, manifesting physical reality through elemental magic.

## 🌌 Visuals
The game features a high-fidelity **Aetherial Glow** aesthetic, powered by dynamic HLSL bloom, multi-layered parallax backgrounds, and glassmorphism UI panels.
![The Void](review/void_main.png)
*See the full [Visual Gallery](docs/screenshots.md) for examples of the Spire, World Map, and Alchemical Mixing.*

## 🌌 The Experience
- **Unfolding Gameplay:** Start with a single "Focus" action and manifest a complex elemental engine.
- **Glassmorphism UI:** Modern, semi-transparent frosted glass panels with runic accents and glowing borders.
- **Dynamic Atmosphere:** Multi-layered Nebula backgrounds with independent parallax and pulsing stars synced to the void hum.
- **Interactive Feedback:** Impact screen-shake, procedural mouse trails, and full-screen alchemical reaction visuals.
- **Resource Visualization:** Real-time sparkline graphs for every elemental essence and animated energy flow lines.
- **Spatial Synergies:** The World Map features animated biome-specific shaders (Ocean ripples, Mountain heat-haze) and pulsing Auras.
- **New Game+:** Reach Ascension to earn "Cosmic Insight," earned through an intense white-out celebration sequence.

## 🛠️ Technical Highlights
- **Advanced Agentic DevOps:** 
    - **Visual Perception:** AI utilizes screenshots and **UI Metadata (JSON)** to verify game state autonomously.
    - **Heuristic Audits:** Automated scripts for **Palette Compliance**, **WCAG Contrast**, and **Parallax Depth**.
    - **Aesthetic Verification:** "Glow Scoring" ensures atmospheric fidelity remains consistent across all loops.
- **Graphics Pipeline:** Custom HLSL shaders for bloom and dynamic color grading based on dominant elements.
- **Component Architecture:** ECS-lite manifestation system defined in JSON for rapid mechanical expansion.

## 🚀 Quick Start
1.  **Build & Run:** `dotnet run --project src/IncriElemental.Desktop/IncriElemental.Desktop.csproj`
2.  **Test:** `dotnet test`
3.  **Health Check:** `python scripts/check_health.py`
4.  **Update Screenshots:** `python scripts/update_screenshots.py`

## 📜 License
This project is licensed under the **MIT License**.

---
*Last Updated: Friday, March 13, 2026 (Updated by Agent Gemini)*
