import os
import subprocess
import sys

DESKTOP_PROJECT_PATH = "src/IncriElemental.Desktop"
COMMANDS_FILE = "src/IncriElemental.Desktop/bin/Debug/net10.0/ai_commands.txt"
REVIEW_DIR = "review"

# Define scenarios: Name -> List of commands
SCENARIOS = {
    "void_main": [
        "resource:Aether:100",
        "discovery:automation_unlocked",
        "manifestation:rune_of_attraction:1",
        "screenshot:void_main"
    ],
    "spire_flow": [
        "resource:Earth:1000",
        "resource:Fire:1000",
        "discovery:altar_constructed",
        "discovery:forge_constructed",
        "discovery:spire_foundation_ready",
        "tab:Spire",
        "screenshot:spire_flow"
    ],
    "world_map": [
        "resource:Life:100",
        "discovery:garden_manifested",
        "manifestation:familiar:1",
        "tab:World",
        "explore:5:5",
        "explore:5:6",
        "explore:6:5",
        "screenshot:world_map"
    ],
    "mixing_table": [
        "resource:Fire:500",
        "resource:Earth:500",
        "discovery:forge_constructed",
        "discovery:fire_unlocked",
        "discovery:earth_unlocked",
        "tab:Spire",
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
