# ARCHITECTURE.md - IncriElemental Technical Design

This document defines the high-level architecture and system responsibilities for **IncriElemental**.

## 1. Project Structure
- **IncriElemental.Core:** Pure C# logic. Contains the `GameEngine`, `GameState`, and all mechanical systems.
- **IncriElemental.Desktop:** MonoGame/XNA implementation. Handles input, rendering, and asset management.
- **IncriElemental.Tests:** XUnit project ensuring logic integrity and progression balance.

## 2. Rendering Pipeline (`VisualManager.cs`)
The game uses a specialized rendering coordinator to maintain the "Aetherial Glow":
- **RenderTarget Management:** All game elements are rendered to a dedicated buffer to allow for full-screen post-processing.
- **HLSL Bloom:** A custom shader (`Bloom.fx`) applies a multi-tap blur and threshold to bright pixels, scaling intensity based on total resource production.
- **Color Grading:** A dynamic tint is applied to the final scene, interpolating between neutral white and the color of the dominant resource.
- **Kinetic Systems:** Centralized screen-shake and tab transition logic ensure consistent visual feedback.

## 3. UI Framework
The UI is built on a custom, resolution-independent layout system:
- **`UiLayout.cs`:** Provides relative positioning and dynamic anchoring for consistent rendering on 16:9 and ultra-wide displays.
- **`Button.cs`:** Implements glassmorphism panels with runic highlights and pulsing borders.
- **`RichTextSystem.cs`:** A tag-based parser supporting inline icons (`[i:icon]`) and colors (`[c:gold]`).
- **`StatusSystem.cs`:** Tracks 20-second historical data to render dynamic resource sparklines.

## 4. Background Systems (`BackgroundManager.cs`)
"The Void" is rendered as a deep, multi-layered environment:
- **Nebula Vistas:** Procedural soft-blob textures move at very slow speeds in the farthest layer.
- **Parallax Layers:** Three distinct star/cloud layers move at varying vectors to provide a sense of vast depth.
- **Atmospheric Pulse:** Global brightness pulses based on `_totalTime` to sync with the background audio.

## 5. Agentic Infrastructure
The project is built for autonomous maintenance:
- **`AiModeSystem.cs`:** Processes sequential command files (`ai_commands.txt`) and exports **UI Metadata** (JSON) for state verification.
- **Audit Suite:** Python-based tools verify visual integrity (Palette, Contrast, Parallax, Aura Pulse).

---
*Last Updated: Friday, March 13, 2026 (Updated by Agent Gemini)*
