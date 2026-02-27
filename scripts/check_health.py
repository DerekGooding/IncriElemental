import os
import sys
import xml.etree.ElementTree as ET
import re

# Configurations
MAX_LINES_PER_FILE = 250
MIN_OVERALL_COVERAGE = 70.0
MIN_FILE_COVERAGE = 50.0
SOURCE_DIR = "src"
COVERAGE_REPORT_PATTERN = r"coverage\.cobertura\.xml"

def check_monoliths():
    print("--- Checking for Monoliths (> 250 lines) ---")
    monoliths = []
    for root, _, files in os.walk(SOURCE_DIR):
        for file in files:
            if file.endswith(".cs") and "obj" not in root and "bin" not in root:
                path = os.path.join(root, file)
                with open(path, "r", encoding="utf-8") as f:
                    lines = len(f.readlines())
                    if lines > MAX_LINES_PER_FILE:
                        monoliths.append((path, lines))
    
    if monoliths:
        for path, lines in monoliths:
            print(f"[FAIL] {path} has {lines} lines.")
        return False
    print("[PASS] No monoliths found.")
    return True

def parse_coverage():
    print("\n--- Checking Test Coverage ---")
    # Find the latest coverage report
    coverage_file = None
    for root, _, files in os.walk("."):
        for file in files:
            if re.match(COVERAGE_REPORT_PATTERN, file):
                coverage_file = os.path.join(root, file)
                break
    
    if not coverage_file:
        print("[ERROR] Coverage report not found. Run 'dotnet test --collect:\"XPlat Code Coverage\"' first.")
        return False

    tree = ET.parse(coverage_file)
    root = tree.getroot()

    overall_line_rate = float(root.attrib["line-rate"]) * 100
    print(f"Overall Coverage: {overall_line_rate:.2f}%")

    if overall_line_rate < MIN_OVERALL_COVERAGE:
        print(f"[FAIL] Overall coverage below {MIN_OVERALL_COVERAGE}%")
        return False

    file_failures = []
    for package in root.findall(".//package"):
        for cls in package.findall(".//class"):
            name = cls.attrib["name"]
            line_rate = float(cls.attrib["line-rate"]) * 100
            if line_rate < MIN_FILE_COVERAGE:
                file_failures.append((name, line_rate))

    if file_failures:
        for name, rate in file_failures:
            print(f"[FAIL] {name} has {rate:.2f}% coverage (Minimum {MIN_FILE_COVERAGE}%)")
        return False

    print("[PASS] Coverage requirements met.")
    return True

if __name__ == "__main__":
    monolith_pass = check_monoliths()
    coverage_pass = parse_coverage()

    if not monolith_pass or not coverage_pass:
        sys.exit(1)
    
    print("\n[SUCCESS] All health checks passed!")
    sys.exit(0)
