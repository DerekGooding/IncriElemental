import os
import json

def audit_reaction(metadata_path, buff_id):
    if not os.path.exists(metadata_path):
        return False
    
    with open(metadata_path, "r") as f:
        data = json.load(f)
    
    # Check ActiveBuffs list
    found = False
    for buff in data.get("ActiveBuffs", []):
        if buff_id.lower() in buff.get("Id", "").lower():
            found = True
            break
            
    print(f"Reaction Visual Audit for {buff_id}:")
    if found:
        print(f"[SUCCESS] {buff_id} detected in active buffs.")
        return True
    else:
        print(f"[FAIL] {buff_id} not detected in active buffs.")
        return False

if __name__ == "__main__":
    import sys
    if len(sys.argv) < 3: sys.exit(0)
    if not audit_reaction(sys.argv[1], sys.argv[2]): sys.exit(1)
    sys.exit(0)
