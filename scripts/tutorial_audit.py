import os
import numpy as np
from PIL import Image

def audit_tutorial(path):
    if not os.path.exists(path):
        return False
    
    img = Image.open(path).convert('RGB')
    pixels = np.array(img)
    brightness = np.sum(pixels, axis=2) / 3.0
    
    # In Tutorial mode, most of the screen is dimmed (< 30% brightness)
    # The highlighted area should be significantly brighter.
    dim_area = np.sum(brightness < 80)
    bright_area = np.sum(brightness > 150)
    total = brightness.size
    
    dim_percent = (dim_area / total) * 100
    bright_percent = (bright_area / total) * 100
    
    print(f"Tutorial Highlight Audit for {path}:")
    print(f"  Dimmed Area: {dim_percent:.2f}%")
    print(f"  Highlighted Area: {bright_percent:.2f}%")
    
    # 13.3 Assert highlight logic
    if dim_percent > 50.0 and bright_percent > 0.1:
        print(f"  [SUCCESS] Tutorial highlight mask detected.")
        return True
    else:
        print(f"  [FAIL] Screen does not appear to be correctly dimmed for tutorial.")
        return False

if __name__ == "__main__":
    import sys
    # This would be run on a specific tutorial screenshot
    if len(sys.argv) < 2:
        sys.exit(0)
    if not audit_tutorial(sys.argv[1]):
        sys.exit(1)
    sys.exit(0)
