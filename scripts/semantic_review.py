import os
import json

def detect_collisions(buttons, screen_width=1024, screen_height=768):
    violations = []
    
    # Define Panels based on Game1.cs Draw() logic
    # Left Panel (Log): Rectangle(5, 50, 200, UiLayout.Height - 60)
    # Right Panel (Status/Resources): Rectangle(UiLayout.Width - 210, 50, 205, UiLayout.Height - 60)
    
    # We define "Forbidden Zones" for center-area buttons.
    # Top buttons should also avoid these X-ranges to not overlap panel headers.
    panels = [
        {'Name': 'LogPanel', 'X': 5, 'Y': 0, 'Width': 200, 'Height': screen_height}, # Full height forbidden for center elements
        {'Name': 'StatusPanel', 'X': screen_width - 210, 'Y': 0, 'Width': 205, 'Height': screen_height}
    ]

    for i in range(len(buttons)):
        b1 = buttons[i]['Bounds']
        b1_name = buttons[i].get('Text', f'Button_{i}')
        b1_tab = buttons[i].get('Tab', 'None')

        # 1. Check if off-screen
        if (b1['X'] < 0 or b1['Y'] < 0 or 
            b1['X'] + b1['Width'] > screen_width or 
            b1['Y'] + b1['Height'] > screen_height):
            violations.append((b1_name, "SCREEN_BOUNDS"))

        # 2. Check collisions with panels
        # Logic: If a button is NOT a tab button (Tab != None), it must not hit any panel.
        # If it IS a tab button (Tab == None), it still must not overlap with panel X-ranges.
        for p in panels:
            if (b1['X'] < p['X'] + p['Width'] and
                b1['X'] + b1['Width'] > p['X'] and
                b1['Y'] < p['Y'] + p['Height'] and
                b1['Y'] + b1['Height'] > p['Y']):
                violations.append((b1_name, p['Name']))

        # 3. Check collisions with other buttons (only if they are on the same tab or one is global)
        for j in range(i + 1, len(buttons)):
            b2 = buttons[j]['Bounds']
            b2_name = buttons[j].get('Text', f'Button_{j}')
            b2_tab = buttons[j].get('Tab', 'None')
            
            # Only collide if they could be visible at the same time
            if b1_tab == b2_tab or b1_tab == "None" or b2_tab == "None":
                # Simple rectangle intersection (Bounding Box)
                if (b1['X'] < b2['X'] + b2['Width'] and
                    b1['X'] + b1['Width'] > b2['X'] and
                    b1['Y'] < b2['Y'] + b2['Height'] and
                    b1['Y'] + b1['Height'] > b2['Y']):
                    violations.append((b1_name, b2_name))
                
    return violations

def semantic_review(metadata_path):
    if not os.path.exists(metadata_path):
        print(f"[FAIL] Metadata file not found: {metadata_path}")
        return False
    
    with open(metadata_path, "r") as f:
        metadata = json.load(f)
    
    buttons = metadata.get("Buttons", [])
    print(f"Semantic Review for {metadata_path}:")
    
    # Use screen size from metadata if available, else default
    width = metadata.get("ScreenWidth", 1024)
    height = metadata.get("ScreenHeight", 768)
    
    collisions = detect_collisions(buttons, width, height)
    if collisions:
        print(f"  [FAIL] UI Violations detected:")
        for c in collisions:
            if c[1] == "SCREEN_BOUNDS":
                print(f"    - {c[0]} is out of screen bounds.")
            else:
                print(f"    - {c[0]} overlaps with {c[1]}")
        return False
    else:
        print(f"  [SUCCESS] No UI collisions or bounds violations detected.")
        
    return True

if __name__ == "__main__":
    import sys
    meta_path = sys.argv[1] if len(sys.argv) > 1 else "review/screenshot.json"
    if not semantic_review(meta_path):
        sys.exit(1)
    sys.exit(0)
