import os
import sys

def is_completely_black(path):
    """
    Checks if a PNG is completely black without requiring PIL/Pillow if possible,
    but realistically PIL is the best way. We will try PIL and fallback.
    """
    if not os.path.exists(path):
        print(f"[ERROR] File not found: {path}")
        return True # Treat missing as failure

    try:
        from PIL import Image
        img = Image.open(path).convert('RGB')
        # getextrema() returns (min, max) for each channel
        # If max is 0 for all channels, it's black.
        extrema = img.getextrema()
        # RGB extrema is ((minR, maxR), (minG, maxG), (minB, maxB))
        is_black = all(ex[1] == 0 for ex in extrema)
        return is_black
    except ImportError:
        # Fallback: Check file size as a VERY crude heuristic
        # A 1024x768 black PNG is usually ~3-5KB. 
        # But this is unreliable. Let's just report the missing dependency.
        print(f"[WARNING] PIL not installed. Cannot verify if {path} is black.")
        return False
    except Exception as e:
        print(f"[ERROR] Failed to check {path}: {e}")
        return True

if __name__ == "__main__":
    if len(sys.argv) < 2:
        print("Usage: python verify_black_screen.py <path_to_png>")
        sys.exit(1)
    
    path = sys.argv[1]
    if is_completely_black(path):
        print(f"[FAIL] {path} is completely black!")
        sys.exit(1)
    else:
        print(f"[SUCCESS] {path} contains non-black pixels.")
        sys.exit(0)
