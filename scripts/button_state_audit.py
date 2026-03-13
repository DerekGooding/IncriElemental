import os
import numpy as np
from PIL import Image

def compare_button_states(idle_path, hover_path):
    if not os.path.exists(idle_path) or not os.path.exists(hover_path):
        print("Missing screenshots for comparison.")
        return False
    
    img1 = Image.open(idle_path).convert('RGB')
    img2 = Image.open(hover_path).convert('RGB')
    
    # Calculate average brightness
    avg1 = np.mean(np.array(img1))
    avg2 = np.mean(np.array(img2))
    
    print(f"Button State Audit:")
    print(f"  Idle Brightness: {avg1:.2f}")
    print(f"  Hover Brightness: {avg2:.2f}")
    
    # Hover should generally be brighter in our theme
    if avg2 > avg1:
        print(f"  [SUCCESS] Hover state detected (Brightness increased by {avg2-avg1:.2f})")
        return True
    else:
        print(f"  [FAIL] No visual change detected between Idle and Hover states.")
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
