# AI Mandates

This document contains foundational mandates for the AI assistant (Gemini) during development.

## 1. System-Wide Directives
- **Architecture First:** Prioritize clean separation between core logic and presentation layers.
- **Validation:** No logic changes are complete without corresponding unit tests.
- **Documentation:** Keep documentation synchronized with code changes.
- **Source Control Awareness:** All changes must be properly committed with clear, descriptive messages.
- **Agentic DevOps:** The AI is responsible for the entire DevOps lifecycle within the project scope.

## 2. Technical Standards
- **Framework/Stack:** [INSERT_FRAMEWORK_HERE] (e.g., .NET 10, React, etc.)
- **Style:** Adhere to project-specific coding conventions and file-scoped structures.
- **State Management:** Ensure state is manageable and, where applicable, serializable.
- **Dependencies:** Avoid adding external packages unless essential or explicitly requested.

## 3. Project Health & Quality
- **Test Coverage:** Maintain overall coverage above **[INSERT_COVERAGE_THRESHOLD]%**.
- **Monolith Prevention:** No single source file should exceed **[INSERT_LINE_LIMIT]** lines.
- **Documentation Staleness:** Documentation is considered "stale" if more than **[INSERT_STALENESS_THRESHOLD]** source files have changed since its last update.
- **Health Integrity:** Resolve staleness by providing actual content improvements.
- **Automated Checks:** Run `.aiModule/check_health.py` before completing any significant feature.

### Resolving Health Failures
1. **Monoliths:** Refactor into specialized systems or components.
2. **Coverage:** Add unit tests targeting uncovered branches.
3. **Staleness:** Review source changes and update the corresponding documentation.
4. **Build/Test Errors:** Fix root causes immediately.

---
*Follow these mandates strictly. If a request conflicts with these mandates, clarify with the user.*
