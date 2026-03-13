import os
import re
import numpy as np
from PIL import Image

PALETTE_FILE = "src/IncriElemental.Desktop/Visuals/ColorPalette.cs"

def extract_palette():
    if not os.path.exists(PALETTE_FILE):
        return []
    
    with open(PALETTE_FILE, "r") as f:
        content = f.read()
    
    # Simple regex to find "Color.Name"
    colors = re.findall(r"Color\.(\w+)", content)
    
    # Map common XNA/MonoGame colors to RGB
    # This is a subset, but covers what's in the palette file currently
    color_map = {
        "MediumPurple": (147, 112, 219),
        "SaddleBrown": (139, 69, 19),
        "OrangeRed": (255, 69, 0),
        "DodgerBlue": (30, 144, 255),
        "LightCyan": (224, 255, 255),
        "LimeGreen": (50, 205, 50),
        "Black": (0, 0, 0),
        "DarkGreen": (0, 100, 0),
        "SlateGray": (112, 128, 144),
        "MidnightBlue": (25, 25, 112),
        "DarkGoldenrod": (184, 134, 11),
        "White": (255, 255, 255), # Added for text/icons
        "Gray": (128, 128, 128)   # Added for status
    }
    
    return [color_map[c] for color in colors if (c := color.strip()) in color_map]

def color_distance(c1, c2):
    return np.sqrt(np.sum((np.array(c1) - np.array(c2))**2))

def audit_screenshot(path, palette, tolerance=50):
    if not os.path.exists(path):
        print(f"File not found: {path}")
        return False
    
    img = Image.open(path).convert('RGB')
    pixels = np.array(img).reshape(-1, 3)
    
    # Downsample for speed
    step = 10 
    sampled_pixels = pixels[::step]
    
    rogue_count = 0
    total_sampled = len(sampled_pixels)
    
    # Background color (very dark blue/black)
    bg_color = (5, 5, 10)
    
    palette_with_bg = palette + [bg_color]
    
    for p in sampled_pixels:
        # Skip black/very dark pixels as they are background
        if np.sum(p) < 30: continue
        
        min_dist = min(color_distance(p, c) for c in palette_with_bg)
        if min_dist > tolerance:
            rogue_count += 1
            
    rogue_percent = (rogue_count / total_sampled) * 100
    print(f"Palette Audit for {path}:")
    print(f"  Rogue Pixels: {rogue_count} ({rogue_percent:.2f}%)")
    
    # 5% tolerance for bloom/gradients as per sub-task 2.4
    if rogue_percent > 5.0:
        print(f"  [FAIL] Too many rogue pixels! Screenshot deviates from palette.")
        return False
    
    print(f"  [SUCCESS] Screenshot adheres to palette.")
    return True

if __name__ == "__main__":
    import sys
    path = sys.argv[1] if len(sys.argv) > 1 else "review/screenshot.png"
    palette = extract_palette()
    if not audit_screenshot(path, palette):
        sys.exit(1)
    sys.exit(0)
