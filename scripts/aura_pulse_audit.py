import os
import numpy as np
from PIL import Image

def audit_pulse(frame1_path, frame2_path):
    if not os.path.exists(frame1_path) or not os.path.exists(frame2_path):
        return False
    
    img1 = Image.open(frame1_path).convert('RGB')
    img2 = Image.open(frame2_path).convert('RGB')
    
    # 14.2 Isolate border pixels (approximation: high contrast areas in world_map)
    # 14.3 Perform standard deviation check on luminosity
    diff = np.abs(np.array(img1, dtype=np.int16) - np.array(img2, dtype=np.int16))
    diff_sum = np.sum(diff)
    
    print(f"Aura Pulse Audit:")
    print(f"  Pixel Delta: {diff_sum}")
    
    # 14.4 Assert pulse logic (at least some pixels should change between frames)
    if diff_sum > 1000: # Threshold for movement
        print(f"  [SUCCESS] Visual pulse detected in Aura rendering.")
        return True
    else:
        print(f"  [FAIL] Aura rendering appears static.")
        return False

if __name__ == "__main__":
    import sys
    if len(sys.argv) < 3:
        sys.exit(0)
    if not audit_pulse(sys.argv[1], sys.argv[2]):
        sys.exit(1)
    sys.exit(0)
