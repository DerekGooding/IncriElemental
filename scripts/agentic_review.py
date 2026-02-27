import os
import subprocess
import time
import sys

# Configurations
DESKTOP_PROJECT_PATH = "src/IncriElemental.Desktop"
EXE_PATH = "src/IncriElemental.Desktop/bin/Debug/net10.0/IncriElemental.Desktop.exe"
COMMANDS_FILE = "src/IncriElemental.Desktop/bin/Debug/net10.0/ai_commands.txt"
SCREENSHOT_FILE = "src/IncriElemental.Desktop/bin/Debug/net10.0/review/screenshot.png"

def build_project():
    print("Building project...")
    result = subprocess.run(["dotnet", "build", DESKTOP_PROJECT_PATH], capture_output=True, text=True)
    if result.returncode != 0:
        print("Build failed!")
        print(result.stdout)
        print(result.stderr)
        return False
    return True

def run_ai_review(commands):
    print(f"Running AI review with {len(commands)} commands...")
    
    # Write commands to file in the output directory
    os.makedirs(os.path.dirname(COMMANDS_FILE), exist_ok=True)
    with open(COMMANDS_FILE, "w") as f:
        for cmd in commands:
            f.write(f"{cmd}\n")
    
    # Run the game in AI mode
    try:
        # We run the exe directly to ensure the working directory is correct for content loading
        base_dir = os.path.dirname(EXE_PATH)
        subprocess.run([EXE_PATH, "--ai-mode"], cwd=base_dir, timeout=30)
    except subprocess.TimeoutExpired:
        print("Game timed out during AI review.")
    except Exception as e:
        print(f"Error running game: {e}")

    if os.path.exists(os.path.join(base_dir, "review/screenshot.png")):
        print(f"Review completed. Screenshot saved to {os.path.join(base_dir, 'review/screenshot.png')}")
        return True
    else:
        print("Review failed! Screenshot not found.")
        return False

if __name__ == "__main__":
    if build_project():
        # Default scenario: Wake up and focus a few times
        scenario = ["focus", "focus", "focus", "focus", "focus"]
        if len(sys.argv) > 1:
            scenario = sys.argv[1:]
            
        run_ai_review(scenario)
