import os
import numpy as np
from PIL import Image

def compare_button_states(idle_path, hover_path):
    if not os.path.exists(idle_path) or not os.path.exists(hover_path):
        print("Missing screenshots for comparison.")
        return False
    
    idle_meta = idle_path.replace(".png", ".json")
    hover_meta = hover_path.replace(".png", ".json")
    
    if not os.path.exists(idle_meta) or not os.path.exists(hover_meta):
        print("Missing metadata for comparison.")
        return False

    with open(idle_meta, "r") as f: m1 = json.load(f)
    with open(hover_meta, "r") as f: m2 = json.load(f)
    
    img1 = Image.open(idle_path).convert('RGB')
    img2 = Image.open(hover_path).convert('RGB')
    
    # Identify the button that changed (the one being hovered)
    # For now, we compare all buttons and find the one with the most change
    # or look for a specific button if provided.
    
    print(f"Button State Audit (Metadata-Aware):")
    
    max_diff = -1
    best_btn = None
    
    for b in m1.get("Buttons", []):
        bounds = b['Bounds']
        box = (bounds['X'], bounds['Y'], bounds['X'] + bounds['Width'], bounds['Y'] + bounds['Height'])
        
        # Crop button area
        crop1 = img1.crop(box)
        crop2 = img2.crop(box)
        
        avg1 = np.mean(np.array(crop1))
        avg2 = np.mean(np.array(crop2))
        diff = abs(avg2 - avg1)
        
        if diff > max_diff:
            max_diff = diff
            best_btn = (b['Text'], avg1, avg2)
            
    if best_btn and max_diff > 5: # Threshold for hover effect
        print(f"  Detected hover effect on '{best_btn[0]}':")
        print(f"    Idle Brightness: {best_btn[1]:.2f}")
        print(f"    Hover Brightness: {best_btn[2]:.2f}")
        
        if best_btn[2] > best_btn[1]:
            print(f"    [SUCCESS] Hover state detected (Brightness increased by {max_diff:.2f})")
            return True
        else:
            print(f"    [FAIL] Hover state did not increase brightness.")
            return False
    else:
        print(f"  [FAIL] No significant visual change detected in any button bounding box.")
        return False

if __name__ == "__main__":
    import sys
    # For now, this is a manual verify or used in a sequence
    if len(sys.argv) < 3:
        print("Usage: python button_state_audit.py <idle.png> <hover.png>")
        sys.exit(0)
    
    if not compare_button_states(sys.argv[1], sys.argv[2]):
        sys.exit(1)
    sys.exit(0)
