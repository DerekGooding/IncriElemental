import os
import numpy as np
from PIL import Image

def audit_aesthetics(path):
    if not os.path.exists(path):
        return False
    
    img = Image.open(path).convert('RGB')
    pixels = np.array(img)
    
    # 20.1 Glow Heuristic: Ratio of Bloom (saturated/bright) to Background (dark)
    brightness = np.sum(pixels, axis=2) / 3.0
    
    # Highly saturated or bright pixels are "Glow"
    glow_mask = (brightness > 180) | (np.max(pixels, axis=2) - np.min(pixels, axis=2) > 100)
    glow_pixels = np.sum(glow_mask)
    total_pixels = brightness.size
    
    glow_score = (glow_pixels / total_pixels) * 100
    
    print(f"Aesthetic Audit for {path}:")
    print(f"  Glow Score: {glow_score:.2f}")
    
    # Heuristic: Early game should have low glow, late game high.
    # For a general check, we just ensure it's not "flat" (0 glow).
    if glow_score > 0.05:
        print(f"  [SUCCESS] Scene has atmospheric glow.")
        return True
    else:
        print(f"  [FAIL] Scene feels visually flat.")
        return False

if __name__ == "__main__":
    import sys
    if len(sys.argv) < 2:
        sys.exit(0)
    if not audit_aesthetics(sys.argv[1]):
        sys.exit(1)
    sys.exit(0)
