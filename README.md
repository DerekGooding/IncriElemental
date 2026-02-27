# IncriElemental - An Unfolding Incremental Game

> **Note:** This project is being developed with **heavy agentic support**. All code, documentation, and design decisions are a collaborative effort between the user and an AI agent (Gemini CLI), following the mandates defined in `GEMINI.md`.

**IncriElemental** is an unfolding incremental game built with **MonoGame** and **.NET 10.0**, focusing on elemental magic and world construction.

### Development Environment
- **SDK:** .NET 10.0
- **Framework:** MonoGame (DesktopGL)
- **Editor:** Visual Studio / VS Code (any C# editor)

### Project Structure
- `src/IncriElemental.Core/`: Pure C# logic, state, and math (Shared).
- `src/IncriElemental.Desktop/`: Platform-specific MonoGame runner and rendering.
- `tests/IncriElemental.Tests/`: Unit tests for the core logic.
- `docs/`: Design documents, architecture, and roadmap.
- `prompts/`: Templates for agentic interaction.
- `scripts/`: Development and validation automation.

### Quick Start
1.  **Build:** `dotnet build`
2.  **Run:** `dotnet run --project src/IncriElemental.Desktop/IncriElemental.Desktop.csproj`
3.  **Test:** `dotnet test`

### Agentic Support
This project is designed for "heavy agentic support." See `GEMINI.md` for the mandates I follow.

---
*Created on Friday, February 27, 2026.*
