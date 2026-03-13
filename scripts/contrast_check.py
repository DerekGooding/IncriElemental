import os
import json
import numpy as np
from PIL import Image

def get_luminance(color):
    r, g, b = [c / 255.0 for c in color]
    r = r / 12.92 if r <= 0.03928 else ((r + 0.055) / 1.055) ** 2.4
    g = g / 12.92 if g <= 0.03928 else ((g + 0.055) / 1.055) ** 2.4
    b = b / 12.92 if b <= 0.03928 else ((b + 0.055) / 1.055) ** 2.4
    return 0.2126 * r + 0.7152 * g + 0.0722 * b

def calculate_contrast(c1, c2):
    l1 = get_luminance(c1); l2 = get_luminance(c2)
    if l1 < l2: l1, l2 = l2, l1
    return (l1 + 0.05) / (l2 + 0.05)

def check_contrast(img_path, meta_path):
    if not os.path.exists(img_path) or not os.path.exists(meta_path): return False
    img = Image.open(img_path).convert('RGB'); data = json.load(open(meta_path))
    all_pass = True; print(f"Contrast Audit for {img_path}:")
    for btn in data.get("Buttons", []):
        b = btn.get("Bounds"); x, y, w, h = b['X'], b['Y'], b['Width'], b['Height']
        if x < 0 or y < 0 or x + w > img.width or y + h > img.height: continue
        pixels = np.array(img.crop((x, y, x + w, y + h))).reshape(-1, 3)
        brightest = pixels[np.argmax(np.sum(pixels, axis=1))]
        darkest = pixels[np.argmin(np.sum(pixels, axis=1))]
        contrast = calculate_contrast(brightest, darkest)
        status = "[PASS]" if contrast >= 4.5 else "[FAIL]"
        print(f"  Button '{btn['Text']}': {contrast:.2f}:1 {status}")
        if contrast < 4.5: all_pass = False
    return all_pass

if __name__ == "__main__":
    import sys
    img_path = sys.argv[1] if len(sys.argv) > 1 else "review/screenshot.png"
    if not check_contrast(img_path, img_path.replace(".png", ".json")): sys.exit(1)
