# .aiModule - Generalized AI Integration Module

This module provides a reusable framework for integrating an AI assistant into a project, including mandates, health check scripts, and prompt templates.

## Structure
- `MANDATES.md`: Foundational directives for the AI.
- `config.json`: Configuration for the health check script.
- `check_health.py`: A Python script to verify project health (monoliths, coverage, doc staleness).
- `prompts/`: A collection of standardized prompt templates.

## Setup Instructions

1.  **Copy the `.aiModule/` directory** to your new project's root.
2.  **Configure `config.json`**:
    - `SOURCE_DIR`: Path to your source code (e.g., "src", "app").
    - `DOC_FILES`: List of documentation files to track for staleness.
    - `TEST_COMMAND`: The command to run your tests and generate coverage reports.
    - `SOURCE_EXTENSIONS`: The file extensions of your source code (e.g., [".js", ".py"]).
3.  **Update `MANDATES.md`**:
    - Replace the bracketed placeholders (e.g., `[INSERT_FRAMEWORK_HERE]`) with your project's technical details.
4.  **Integration**:
    - Ask the AI to read `.aiModule/MANDATES.md` at the start of a session.
    - Use the templates in `.aiModule/prompts/` for consistent communication.
    - Run `python .aiModule/check_health.py` to ensure project quality.

## Requirements
- Python 3.x
- Git (for documentation staleness checks)
- Your project's test runner (e.g., dotnet, npm, pytest)
