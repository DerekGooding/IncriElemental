import os
import json
import numpy as np
from PIL import Image

def detect_collisions(buttons):
    collisions = []
    for i in range(len(buttons)):
        for j in range(i + 1, len(buttons)):
            b1 = buttons[i]['Bounds']
            b2 = buttons[j]['Bounds']
            
            # Simple rectangle intersection
            if (b1['X'] < b2['X'] + b2['Width'] and
                b1['X'] + b1['Width'] > b2['X'] and
                b1['Y'] < b2['Y'] + b2['Height'] and
                b1['Y'] + b1['Height'] > b2['Y']):
                collisions.append((buttons[i]['Text'], buttons[j]['Text']))
    return collisions

def semantic_review(metadata_path):
    if not os.path.exists(metadata_path):
        return False
    
    with open(metadata_path, "r") as f:
        metadata = json.load(f)
    
    buttons = metadata.get("Buttons", [])
    print(f"Semantic Review for {metadata_path}:")
    
    # 6.3 Detect Visual Collisions
    collisions = detect_collisions(buttons)
    if collisions:
        print(f"  [FAIL] UI Overlaps detected:")
        for c in collisions:
            print(f"    - {c[0]} overlaps with {c[1]}")
        return False
    else:
        print(f"  [SUCCESS] No UI collisions detected.")
        
    return True

if __name__ == "__main__":
    import sys
    meta_path = sys.argv[1] if len(sys.argv) > 1 else "review/screenshot.json"
    if not semantic_review(meta_path):
        sys.exit(1)
    sys.exit(0)
