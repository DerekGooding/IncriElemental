import os
import subprocess
import time
import sys

# Configurations
DESKTOP_PROJECT_PATH = "src/IncriElemental.Desktop"
EXE_PATH = "src/IncriElemental.Desktop/bin/Debug/net10.0/IncriElemental.Desktop.exe"
COMMANDS_FILE = "src/IncriElemental.Desktop/bin/Debug/net10.0/ai_commands.txt"
SCREENSHOT_FILE = "review/screenshot.png"

def build_project():
    print("Cleaning and Building project...")
    subprocess.run(["dotnet", "clean", DESKTOP_PROJECT_PATH], capture_output=True)
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
    # For 'dotnet run', the working directory is the project folder, 
    # but the binary looks for ai_commands.txt in its own folder.
    # We'll put it in both to be safe.
    output_dirs = [
        "src/IncriElemental.Desktop/bin/Debug/net10.0",
        "src/IncriElemental.Desktop"
    ]
    
    for d in output_dirs:
        os.makedirs(d, exist_ok=True)
        with open(os.path.join(d, "ai_commands.txt"), "w") as f:
            for cmd in commands:
                f.write(f"{cmd}\n")
    
    # Run the game in AI mode
    try:
        subprocess.run(["dotnet", "run", "--project", DESKTOP_PROJECT_PATH, "--", "--ai-mode"], timeout=30)
    except subprocess.TimeoutExpired:
        print("Game timed out during AI review.")
    except Exception as e:
        print(f"Error running game: {e}")

    # Check for screenshot in common locations
    search_paths = [
        os.path.join(DESKTOP_PROJECT_PATH, "screenshot.png"),
        "screenshot.png"
    ]
    
    for path in search_paths:
        if os.path.exists(path):
            print(f"Review completed. Screenshot found at {path}")
            # Ensure it's in the root review folder for the user
            os.makedirs("review", exist_ok=True)
            import shutil
            shutil.copy(path, "review/screenshot.png")
            return True

    print("Review failed! Screenshot not found.")
    return False

if __name__ == "__main__":
    if build_project():
        # Default scenario: Wake up and focus a few times
        scenario = ["focus", "focus", "focus", "focus", "focus"]
        if len(sys.argv) > 1:
            scenario = sys.argv[1:]
            
        run_ai_review(scenario)
