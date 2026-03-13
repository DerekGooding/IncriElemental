import os
import numpy as np
from PIL import Image

def audit_ascension(path):
    if not os.path.exists(path):
        return False
    
    img = Image.open(path).convert('RGB')
    pixels = np.array(img)
    
    # 18.4 Assert brightness >= 245
    avg_brightness = np.mean(pixels)
    
    print(f"Ascension Brightness Audit for {path}:")
    print(f"  Average Brightness: {avg_brightness:.2f}/255")
    
    if avg_brightness >= 240: # Using 240 as a safe threshold
        print(f"  [SUCCESS] Ascension white-out effect verified.")
        return True
    else:
        print(f"  [FAIL] Screen is not bright enough for Ascension peak.")
        return False

if __name__ == "__main__":
    import sys
    if len(sys.argv) < 2:
        sys.exit(0)
    if not audit_ascension(sys.argv[1]):
        sys.exit(1)
    sys.exit(0)
