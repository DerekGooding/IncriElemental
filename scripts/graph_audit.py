import os
import numpy as np
from PIL import Image

def audit_graph(path):
    if not os.path.exists(path):
        return False
    
    img = Image.open(path).convert('RGB')
    pixels = np.array(img)
    
    # Heuristic: Nodes and lines should be bright against the void.
    # Calculate percentage of "Bright" pixels (nodes/edges)
    brightness = np.sum(pixels, axis=2) / 3.0
    bright_pixels = np.sum(brightness > 100)
    total_pixels = brightness.size
    density = (bright_pixels / total_pixels) * 100
    
    print(f"Graph Readability Audit for {path}:")
    print(f"  Visual Density: {density:.2f}%")
    
    # 12.3 Detect Node Clumping (placeholder)
    # 12.4 Verify contrast
    if density > 0.5 and density < 20.0:
        print(f"  [SUCCESS] Graph visual density is within expected range.")
        return True
    else:
        print(f"  [FAIL] Graph is either too cluttered or empty.")
        return False

if __name__ == "__main__":
    import sys
    path = sys.argv[1] if len(sys.argv) > 1 else "review/spire_flow.png"
    if not audit_graph(path):
        sys.exit(1)
    sys.exit(0)
