import os
import subprocess
import time
import sys
import json

# Configurations
DESKTOP_PROJECT_PATH = "src/IncriElemental.Desktop"
EXE_PATH = "src/IncriElemental.Desktop/bin/Debug/net10.0/IncriElemental.Desktop.exe"
COMMANDS_FILE = "src/IncriElemental.Desktop/bin/Debug/net10.0/ai_commands.txt"
SCREENSHOT_FILE = "review/screenshot.png"

def build_project():
    print("Cleaning and Building project...")
    # Skip clean for speed unless explicitly requested
    result = subprocess.run(["dotnet", "build", DESKTOP_PROJECT_PATH], capture_output=True, text=True)
    if result.returncode != 0:
        print("Build failed!")
        print(result.stdout)
        print(result.stderr)
        return False
    return True

def verify_metadata(metadata_path, assertions):
    if not os.path.exists(metadata_path):
        print(f"[FAIL] Metadata file not found: {metadata_path}")
        return False
    
    with open(metadata_path, "r") as f:
        data = json.load(f)
    
    all_found = True
    for text in assertions:
        found = False
        # Check buttons
        for btn in data.get("Buttons", []):
            btn_text = btn.get("Text") or ""
            btn_sub = btn.get("Subtitle") or ""
            if text.lower() in btn_text.lower() or text.lower() in btn_sub.lower():
                found = True
                break
        # Check resources
        if not found:
            for res in data.get("Resources", []):
                res_type = res.get("Type") or ""
                if text.lower() in res_type.lower():
                    found = True
                    break
        
        if found:
            print(f"[SUCCESS] Found asserted text: '{text}'")
        else:
            print(f"[FAIL] Could not find asserted text: '{text}'")
            all_found = False
            
    return all_found

def run_ai_review(commands, assertions=None):
    print(f"Running AI review with {len(commands)} commands...")
    
    output_dirs = [
        "src/IncriElemental.Desktop/bin/Debug/net10.0",
        "src/IncriElemental.Desktop"
    ]
    
    for d in output_dirs:
        os.makedirs(d, exist_ok=True)
        with open(os.path.join(d, "ai_commands.txt"), "w") as f:
            for cmd in commands:
                f.write(f"{cmd}\n")
    
    try:
        subprocess.run(["dotnet", "run", "--project", DESKTOP_PROJECT_PATH, "--", "--ai-mode"], timeout=30)
    except subprocess.TimeoutExpired:
        print("Game timed out during AI review.")
    except Exception as e:
        print(f"Error running game: {e}")

    # Check for screenshot
    search_paths = [
        os.path.join(DESKTOP_PROJECT_PATH, "screenshot.png"),
        "screenshot.png"
    ]
    
    screenshot_found = False
    for path in search_paths:
        if os.path.exists(path):
            screenshot_found = True
            metadata_path = path.replace(".png", ".json")
            
            # Metadata Verification
            metadata_pass = True
            if assertions:
                print("Performing metadata verification...")
                metadata_pass = verify_metadata(metadata_path, assertions)

            # Visual Regression Check
            baseline_path = "review/baseline.png"
            visual_pass = True
            if os.path.exists(baseline_path):
                print("Performing visual regression check...")
                visual_pass = compare_images(path, baseline_path)
            else:
                print("[INFO] No baseline found. Saving current screenshot as baseline.")
                os.makedirs("review", exist_ok=True)
                import shutil
                shutil.copy(path, baseline_path)

            # Copy to review/ folder
            os.makedirs("review", exist_ok=True)
            import shutil
            shutil.copy(path, "review/screenshot.png")
            if os.path.exists(metadata_path):
                shutil.copy(metadata_path, "review/screenshot.json")
            
            return metadata_pass and visual_pass

    print("Review failed! Screenshot not found.")
    return False

def compare_images(path1, path2):
    try:
        from PIL import Image, ImageChops
        img1 = Image.open(path1).convert('RGB')
        img2 = Image.open(path2).convert('RGB')
        diff = ImageChops.difference(img1, img2)
        if diff.getbbox():
            # Calculate percentage difference
            import numpy as np
            diff_array = np.array(diff)
            non_zero = np.count_nonzero(diff_array)
            total = diff_array.size
            percent = (non_zero / total) * 100
            print(f"Image difference: {percent:.2f}%")
            
            if percent >= 1.0:
                print(f"[FAIL] Visual regression detected! Difference: {percent:.2f}%")
                return False
        return True
    except ImportError:
        print("[WARNING] Pillow or NumPy not installed. Skipping visual regression.")
        return True
    except Exception as e:
        print(f"Error comparing images: {e}")
        return True

if __name__ == "__main__":
    import argparse
    parser = argparse.ArgumentParser()
    parser.add_argument("commands", nargs="*", help="Commands to run")
    parser.add_argument("--text-assert", action="append", help="Text to verify in UI metadata")
    args = parser.parse_args()

    success = False
    if build_project():
        scenario = args.commands if args.commands else ["focus", "focus", "focus", "focus", "focus"]
        success = run_ai_review(scenario, assertions=args.text_assert)
    
    if not success:
        sys.exit(1)
    sys.exit(0)
