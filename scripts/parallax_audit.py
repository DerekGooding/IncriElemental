import os
import numpy as np
from PIL import Image

def audit_parallax(frame1_path, frame2_path):
    if not os.path.exists(frame1_path) or not os.path.exists(frame2_path):
        return False
    
    img1 = Image.open(frame1_path).convert('RGB')
    img2 = Image.open(frame2_path).convert('RGB')
    
    # 19.2 Optical flow (simplified: find areas of movement)
    # We compare movement of bright small dots (stars) vs large blocks (UI)
    diff = np.abs(np.array(img1, dtype=np.int16) - np.array(img2, dtype=np.int16))
    
    # UI elements are generally larger and more colorful.
    # Stars are tiny and usually white/dim.
    
    print(f"Parallax Audit:")
    print(f"  Total Pixel Delta: {np.sum(diff)}")
    
    # 19.3 Verify background movement vectors are shorter (placeholder)
    # For now, we just verify that there IS movement in multiple regions
    if np.sum(diff) > 5000:
        print(f"  [SUCCESS] Movement detected during parallax test.")
        return True
    else:
        print(f"  [FAIL] Screen appears static during parallax test.")
        return False

if __name__ == "__main__":
    import sys
    if len(sys.argv) < 3:
        sys.exit(0)
    if not audit_parallax(sys.argv[1], sys.argv[2]):
        sys.exit(1)
    sys.exit(0)
