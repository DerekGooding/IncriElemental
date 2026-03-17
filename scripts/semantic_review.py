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
        tb = t['Bounds']
        # Skip empty text
        if tb['Width'] == 0 or tb['Height'] == 0: continue
        
        found_container = False
        potential_overflow = None

        # Check buttons
        for b in buttons:
            bb = b['Bounds']
            # If they overlap at all
            if (tb['X'] < bb['X'] + bb['Width'] and
                tb['X'] + tb['Width'] > bb['X'] and
                tb['Y'] < bb['Y'] + bb['Height'] and
                tb['Y'] + tb['Height'] > bb['Y']):
                
                if is_contained(tb, bb):
                    found_container = True
                    break
                else:
                    # Overlaps but not contained -> potential overflow
                    potential_overflow = f"Text '{t['Text']}' overflows Button '{b.get('Text', 'Unknown')}'"
                    found_container = True # Mark as found so we don't treat as floating
                    break
        
        if not found_container:
            # Check panels
            # We want the SMALLEST panel that contains it or overlaps it most
            candidate_panels = []
            for p in panels:
                pb = p['Bounds']
                if (tb['X'] < pb['X'] + pb['Width'] and
                    tb['X'] + tb['Width'] > pb['X'] and
                    tb['Y'] < pb['Y'] + pb['Height'] and
                    tb['Y'] + tb['Height'] > pb['Y']):
                    candidate_panels.append(p)
            
            if candidate_panels:
                # Sort by area ascending to find the most specific container
                candidate_panels.sort(key=lambda x: x['Bounds']['Width'] * x['Bounds']['Height'])
                p = candidate_panels[0]
                if not is_contained(tb, p['Bounds']):
                    potential_overflow = f"Text '{t['Text']}' overflows Panel"
                found_container = True

        if potential_overflow:
            collisions.append(potential_overflow)
        
        # If still no container found, it might be floating text
        # Check if it overlaps with any button it shouldn't be in
        elif not found_container:
            for b in buttons:
                bb = b['Bounds']
                if (tb['X'] < bb['X'] + bb['Width'] and
                    tb['X'] + tb['Width'] > bb['X'] and
                    tb['Y'] < bb['Y'] + bb['Height'] and
                    tb['Y'] + tb['Height'] > bb['Y']):
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
