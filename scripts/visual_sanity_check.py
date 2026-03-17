import json
import os
import sys

def check_visual_sanity(current_path, golden_path):
    if not os.path.exists(current_path):
        print(f"Error: Current metadata not found at {current_path}")
        return False
    
    if not os.path.exists(golden_path):
        print(f"Golden reference not found at {golden_path}. Creating one from current.")
        with open(current_path, 'r') as f:
            data = json.load(f)
        with open(golden_path, 'w') as f:
            json.dump(data, f, indent=2)
        return True

    with open(current_path, 'r') as f:
        current = json.load(f)
    with open(golden_path, 'r') as f:
        golden = json.load(f)

    errors = []
    
    # Check buttons
    gold_btns = {b['Text']: b for b in golden.get('Buttons', [])}
    curr_btns = {b['Text']: b for b in current.get('Buttons', [])}
    
    for text, g_btn in gold_btns.items():
        if text not in curr_btns:
            errors.append(f"Missing button: {text}")
            continue
        
        c_btn = curr_btns[text]
        # Check if intent changed
        if g_btn.get('Intent') != c_btn.get('Intent'):
            errors.append(f"Intent mismatch for {text}: expected {g_btn.get('Intent')}, got {c_btn.get('Intent')}")
            
        # Check bounds (small threshold for layout jitter)
        gb = g_btn['Bounds']
        cb = c_btn['Bounds']
        dx = abs(gb['X'] - cb['X'])
        dy = abs(gb['Y'] - cb['Y'])
        if dx > 10 or dy > 10:
            errors.append(f"Button {text} shifted significantly: delta({dx}, {dy})")

    # Check for illegal overlaps
    all_elements = current.get('Buttons', []) + current.get('Elements', [])
    for i, e1 in enumerate(all_elements):
        for j, e2 in enumerate(all_elements):
            if i >= j: continue
            b1 = e1['Bounds']
            b2 = e2['Bounds']
            
            # Check overlap
            if (b1['X'] < b2['X'] + b2['Width'] and
                b1['X'] + b1['Width'] > b2['X'] and
                b1['Y'] < b2['Y'] + b2['Height'] and
                b1['Y'] + b1['Height'] > b2['Y']):
                
                # Filter out intentional overlaps (like text inside panels)
                if e1.get('Type') == 'Panel' or e2.get('Type') == 'Panel':
                    continue
                
                # Truncated names for readability
                n1 = e1.get('Text') or e1.get('Type')
                n2 = e2.get('Text') or e2.get('Type')
                errors.append(f"Visual Collision: {n1} overlaps with {n2}")

    if errors:
        print("Visual Sanity Check FAILED:")
        for err in errors:
            print(f" - {err}")
        return False

    print("Visual Sanity Check PASSED.")
    return True

if __name__ == "__main__":
    cur = "screenshot.json"
    gold = "docs/golden_reference.json"
    if len(sys.argv) > 2:
        cur = sys.argv[1]
        gold = sys.argv[2]
    
    success = check_visual_sanity(cur, gold)
    sys.exit(0 if success else 1)
