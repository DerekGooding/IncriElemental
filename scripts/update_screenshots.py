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
        "screenshot:void_main"
    ],
    "spire_flow": [
        "update:3600", # Fast forward an hour to unlock things
        "manifest:aether_unlocked",
        "tab:flow",
        "screenshot:spire_flow"
    ],
    "world_map": [
        "update:7200",
        "manifest:garden_manifested",
        "tab:world",
        "screenshot:world_map"
    ],
    "mixing_table": [
        "update:7200",
        "manifest:forge_constructed",
        "tab:spire",
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
