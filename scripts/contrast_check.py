import os
import json
import numpy as np
from PIL import Image

def get_luminance(color):
    # sRGB luminance formula
    r, g, b = [c / 255.0 for c in color]
    r = r / 12.92 if r <= 0.03928 else ((r + 0.055) / 1.055) ** 2.4
    g = g / 12.92 if g <= 0.03928 else ((g + 0.055) / 1.055) ** 2.4
    b = b / 12.92 if b <= 0.03928 else ((b + 0.055) / 1.055) ** 2.4
    return 0.2126 * r + 0.7152 * g + 0.0722 * b

def calculate_contrast(c1, c2):
    l1 = get_luminance(c1)
    l2 = get_luminance(c2)
    if l1 < l2: l1, l2 = l2, l1
    return (l1 + 0.05) / (l2 + 0.05)

def check_contrast(screenshot_path, metadata_path):
    if not os.path.exists(screenshot_path) or not os.path.exists(metadata_path):
        print("Required files missing.")
        return False
    
    img = Image.open(screenshot_path).convert('RGB')
    with open(metadata_path, "r") as f:
        metadata = json.load(f)
    
    all_pass = True
    print(f"Contrast Audit for {screenshot_path}:")
    
    for btn in metadata.get("Buttons", []):
        bounds = btn.get("Bounds")
        # Sample center of button for text color (heuristic)
        # In reality, we'd need to know the specific color used, 
        # but we can try to find the brightest pixel in the button area for text
        # and darkest for background.
        
        x, y, w, h = bounds['X'], bounds['Y'], bounds['Width'], bounds['Height']
        # Ensure we are within image bounds
        if x < 0 or y < 0 or x + w > img.width or y + h > img.height: continue
        
        region = img.crop((x, y, x + w, y + h))
        pixels = np.array(region).reshape(-1, 3)
        
        # Heuristic: brightest pixel is text, darkest is background
        # (This is an approximation)
        brightest = pixels[np.argmax(np.sum(pixels, axis=1))]
        darkest = pixels[np.argmin(np.sum(pixels, axis=1))]
        
        contrast = calculate_contrast(brightest, darkest)
        status = "[PASS]" if contrast >= 4.5 else "[FAIL]"
        print(f"  Button '{btn['Text']}': {contrast:.2f}:1 {status}")
        
        if contrast < 4.5:
            all_pass = False
            
    return all_pass

if __name__ == "__main__":
    import sys
    img_path = sys.argv[1] if len(sys.argv) > 1 else "review/screenshot.png"
    meta_path = img_path.replace(".png", ".json")
    if not check_contrast(img_path, meta_path):
        sys.exit(1)
    sys.exit(0)
