import os
import json

def is_contained(inner, outer):
    return (inner['X'] >= outer['X'] and
            inner['Y'] >= outer['Y'] and
            inner['X'] + inner['Width'] <= outer['X'] + outer['Width'] and
            inner['Y'] + inner['Height'] <= outer['Y'] + outer['Height'])

def detect_collisions(buttons, elements):
    collisions = []
    
    text_elements = [e for e in elements if e['Type'] == 'Text']
    panels = [e for e in elements if e['Type'] == 'Panel']
    
    # 1. Check if Text overflows its likely container (Button or Panel)
    for t in text_elements:
        found_container = False
        # Check buttons first
        for b in buttons:
            if is_contained(t['Bounds'], b['Bounds']):
                found_container = True
                break
            # If it overlaps but isn't contained, it might be an overflow
            if (t['Bounds']['X'] < b['Bounds']['X'] + b['Bounds']['Width'] and
                t['Bounds']['X'] + t['Bounds']['Width'] > b['Bounds']['X'] and
                t['Bounds']['Y'] < b['Bounds']['Y'] + b['Bounds']['Height'] and
                t['Bounds']['Y'] + t['Bounds']['Height'] > b['Bounds']['Y']):
                # It overlaps! Is it too large?
                if t['Bounds']['Width'] > b['Bounds']['Width'] or t['Bounds']['Height'] > b['Bounds']['Height']:
                    collisions.append(f"Text '{t['Text']}' overflows Button '{b.get('Text', 'Unknown')}'")
                found_container = True # We consider this its container even if it overflows
                break
        
        if not found_container:
            # Check panels
            for p in panels:
                if is_contained(t['Bounds'], p['Bounds']):
                    found_container = True
                    break
                if (t['Bounds']['X'] < p['Bounds']['X'] + p['Bounds']['Width'] and
                    t['Bounds']['X'] + t['Bounds']['Width'] > p['Bounds']['X'] and
                    t['Bounds']['Y'] < p['Bounds']['Y'] + p['Bounds']['Height'] and
                    t['Bounds']['Y'] + t['Bounds']['Height'] > p['Bounds']['Y']):
                    if t['Bounds']['Width'] > p['Bounds']['Width'] or t['Bounds']['Height'] > p['Bounds']['Height']:
                        collisions.append(f"Text '{t['Text']}' overflows Panel")
                    found_container = True
                    break
        
        # If still no container found, it might be floating text
        # Check if it overlaps with any button it shouldn't be in
        if not found_container:
            for b in buttons:
                if (t['Bounds']['X'] < b['Bounds']['X'] + b['Bounds']['Width'] and
                    t['Bounds']['X'] + t['Bounds']['Width'] > b['Bounds']['X'] and
                    t['Bounds']['Y'] < b['Bounds']['Y'] + b['Bounds']['Height'] and
                    t['Bounds']['Y'] + t['Bounds']['Height'] > b['Bounds']['Y']):
                    collisions.append(f"Floating Text '{t['Text']}' overlaps with Button '{b.get('Text', 'Unknown')}'")

    # 2. Check for Button-Button collisions (unrelated to text)
    for i in range(len(buttons)):
        for j in range(i + 1, len(buttons)):
            b1 = buttons[i]['Bounds']
            b2 = buttons[j]['Bounds']
            if (b1['X'] < b2['X'] + b2['Width'] and
                b1['X'] + b1['Width'] > b2['X'] and
                b1['Y'] < b2['Y'] + b2['Height'] and
                b1['Y'] + b1['Height'] > b2['Y']):
                collisions.append(f"Button '{buttons[i].get('Text')}' overlaps with Button '{buttons[j].get('Text')}'")

    return collisions

def semantic_review(metadata_path):
    if not os.path.exists(metadata_path):
        return False
    
    with open(metadata_path, "r") as f:
        metadata = json.load(f)
    
    buttons = metadata.get("Buttons", [])
    elements = metadata.get("Elements", [])
    print(f"Semantic Review for {metadata_path}:")
    
    # 6.3 Detect Visual Collisions
    collisions = detect_collisions(buttons, elements)
    if collisions:
        print(f"  [FAIL] UI Overlaps detected:")
        for c in collisions:
            print(f"    - {c}")
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
