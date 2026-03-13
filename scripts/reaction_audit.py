import os
import json
import numpy as np
from PIL import Image

def audit_reaction(metadata_path, buff_name):
    if not os.path.exists(metadata_path):
        return False
    
    with open(metadata_path, "r") as f:
        metadata = json.load(f)
    
    # 15.3 Verify presence of buff in status bar (via metadata)
    found = False
    for res in metadata.get("Resources", []):
        # In our engine, some "buffs" might be tracked as special resources or 
        # just appearing in history. For now we check resources.
        if buff_name.lower() in res.get("Type", "").lower():
            found = True
            break
            
    print(f"Reaction Visual Audit for {buff_name}:")
    if found:
        print(f"  [SUCCESS] {buff_name} detected in game state.")
        return True
    else:
        # Check history/log if not in resources
        print(f"  [FAIL] {buff_name} not detected after reaction.")
        return False

if __name__ == "__main__":
    import sys
    if len(sys.argv) < 3:
        sys.exit(0)
    if not audit_reaction(sys.argv[1], sys.argv[2]):
        sys.exit(1)
    sys.exit(0)
