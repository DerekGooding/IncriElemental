import json
import os
import sys

def check_visual_sanity(current_path, golden_path):
    if not os.path.exists(current_path):
        print(f"Error: Current metadata not found at {current_path}")
        return False
    
    with open(current_path, 'r', encoding='utf-8') as f:
        current = json.load(f)

    # If golden path provided and exists, do golden comparison
    if golden_path and os.path.exists(golden_path):
        with open(golden_path, 'r', encoding='utf-8') as f:
            golden = json.load(f)
        # Golden checks (omitted for brevity here, focusing on collision)
    
    errors = []
    
    cur_tab = current.get('CurrentTab', 'None')
    
    # Check for illegal overlaps
    all_elements = []
    for b in current.get('Buttons', []):
        all_elements.append({
            'Name': f"Button '{b.get('Text')}'",
            'Type': 'Button',
            'Bounds': b['Bounds']
        })
    for e in current.get('Elements', []):
        all_elements.append({
            'Name': f"{e.get('Type')} '{e.get('Text', '')}'",
            'Type': e.get('Type'),
            'Bounds': e['Bounds']
        })

    for i, e1 in enumerate(all_elements):
        for j, e2 in enumerate(all_elements):
            if i >= j: continue
            
            b1 = e1['Bounds']
            b2 = e2['Bounds']
            
            # Intersection check
            if (b1['X'] < b2['X'] + b2['Width'] and
                b1['X'] + b1['Width'] > b2['X'] and
                b1['Y'] < b2['Y'] + b2['Height'] and
                b1['Y'] + b1['Height'] > b2['Y']):
                
                # Check for containment (Nest)
                e1_inside_e2 = (b1['X'] >= b2['X'] and b1['Y'] >= b2['Y'] and 
                                b1['X'] + b1['Width'] <= b2['X'] + b2['Width'] and 
                                b1['Y'] + b1['Height'] <= b2['Y'] + b2['Height'])
                
                e2_inside_e1 = (b2['X'] >= b1['X'] and b2['Y'] >= b1['Y'] and 
                                b2['X'] + b2['Width'] <= b1['X'] + b1['Width'] and 
                                b2['Y'] + b2['Height'] <= b1['Y'] + b1['Height'])
                
                if e1_inside_e2 or e2_inside_e1:
                    continue # Valid nest
                
                # If they intersect but neither contains the other, it's an illegal collision
                errors.append(f"Visual Collision [{cur_tab}]: {e1['Name']} and {e2['Name']} overlap illegally.")

    # High-Priority Check: World Tab sanity
    if cur_tab == "World":
        has_map = any(e['Type'] == 'WorldMap' for e in current.get('Elements', []))
        if not has_map:
            errors.append("Visual Deficiency [World]: WorldMap element missing from metadata.")

    if errors:
        print("Visual Sanity Check FAILED:")
        for err in errors:
            print(f" - {err}")
        return False

    print(f"Visual Sanity Check PASSED for tab: {cur_tab}")
    return True

if __name__ == "__main__":
    cur = "screenshot.json"
    gold = None
    if len(sys.argv) > 1 and not sys.argv[1].startswith("--"):
        cur = sys.argv[1]
    if len(sys.argv) > 2 and not sys.argv[2].startswith("--"):
        gold = sys.argv[2]
    
    success = check_visual_sanity(cur, gold)
    sys.exit(0 if success else 1)
