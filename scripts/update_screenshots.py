import os
import subprocess
import sys

DESKTOP_PROJECT_PATH = "src/IncriElemental.Desktop"
COMMANDS_FILE = "src/IncriElemental.Desktop/bin/Debug/net10.0/ai_commands.txt"
REVIEW_DIR = "review"

# Define scenarios: Name -> List of commands
SCENARIOS = {
    "void_main": [
        "focus", "focus", "focus", "focus", "focus",
        "update:1.0",
        "manifest:speck_of_matter",
        "update:1.0",
        "screenshot:void_main"
    ],
    "spire_flow": [
        "focus", "focus", "focus", "focus", "focus",
        "focus", "focus", "focus", "focus", "focus",
        "manifest:pickaxe",
        "focus", "focus", "focus", "focus", "focus",
        "manifest:speck_of_matter",
        "update:10.0",
        "tab:Spire",
        "update:1.0",
        "screenshot:spire_flow"
    ],
    "world_map": [
        "focus", "focus", "focus", "focus", "focus",
        "focus", "focus", "focus", "focus", "focus",
        "manifest:familiar",
        "update:5.0",
        "tab:World",
        "update:1.0",
        "screenshot:world_map"
    ],
    "mixing_table": [
        "focus", "focus", "focus", "focus", "focus",
        "focus", "focus", "focus", "focus", "focus",
        "manifest:forge",
        "update:5.0",
        "tab:Spire",
        "update:1.0",
        "screenshot:mixing_table"
    ]
}

def run_scenario(name, commands):
    print(f"--- Capturing Scenario: {name} ---")
    with open(COMMANDS_FILE, "w") as f:
        for cmd in commands:
            f.write(f"{cmd}\n")
    
    try:
        result = subprocess.run(["dotnet", "run", "--project", DESKTOP_PROJECT_PATH, "--", "--ai-mode"], timeout=30, capture_output=True, text=True)
        print(result.stdout)
        print(result.stderr)
    except Exception as e:
        print(f"Error running scenario {name}: {e}")

if __name__ == "__main__":
    os.makedirs(REVIEW_DIR, exist_ok=True)
    
    # We could run them one by one, but AiModeSystem currently exits after one run.
    # To be efficient, we run the game once for each scenario.
    for name, commands in SCENARIOS.items():
        run_scenario(name, commands)
    
    print("\n[SUCCESS] Screenshots updated in 'review/' folder.")
