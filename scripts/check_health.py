import os
import sys
import xml.etree.ElementTree as ET
import re
import subprocess
import json

# Configurations
MAX_LINES_PER_FILE = 250
MIN_OVERALL_COVERAGE = 70.0
MIN_FILE_COVERAGE = 50.0
DOCS_STALENESS_THRESHOLD = 8
SOURCE_DIR = "src"
COVERAGE_REPORT_PATTERN = r"coverage\.cobertura\.xml"
DOC_FILES = ["README.md", "docs/architecture.md", "docs/game_design.md", "docs/roadmap.md", "docs/devops.md", "GEMINI.md"]

def get_git_changes_since_file(file_path):
    try:
        last_commit = subprocess.check_output(["git", "log", "-1", "--format=%H", "--", file_path]).decode().strip()
        if not last_commit: return 0
        changes = subprocess.check_output(["git", "diff", "--name-only", last_commit, "HEAD", "--", SOURCE_DIR]).decode().strip()
        return len(set(changes.splitlines())) if changes else 0
    except:
        return 0

def check_docs_staleness():
    print("\n--- Checking Documentation Staleness ---")
    stale_docs = []
    all_up_to_date = True
    for doc in DOC_FILES:
        if os.path.exists(doc):
            changes = get_git_changes_since_file(doc)
            print(f"{doc}: {changes} source files changed.")
            if changes > DOCS_STALENESS_THRESHOLD:
                print(f"[FAIL] {doc} is stale!")
                all_up_to_date = False
    return all_up_to_date

def check_monoliths():
    print("--- Checking for Monoliths (> 250 lines) ---")
    monolith_count = 0
    for root, _, files in os.walk(SOURCE_DIR):
        for file in files:
            if file.endswith(".cs") and "obj" not in root and "bin" not in root:
                path = os.path.join(root, file)
                with open(path, "r", encoding="utf-8") as f:
                    lines = len(f.readlines())
                    if lines > MAX_LINES_PER_FILE:
                        print(f"[FAIL] {path} has {lines} lines.")
                        monolith_count += 1
    return monolith_count

import os
import sys
import xml.etree.ElementTree as ET
import re
import subprocess
import json
import shutil

# Configurations
MAX_LINES_PER_FILE = 250
MIN_OVERALL_COVERAGE = 70.0
MIN_FILE_COVERAGE = 50.0
DOCS_STALENESS_THRESHOLD = 8
SOURCE_DIR = "src"
RESULTS_DIR = "TestResults"
COVERAGE_REPORT_PATTERN = r"coverage\.cobertura\.xml"
DOC_FILES = ["README.md", "docs/architecture.md", "docs/game_design.md", "docs/roadmap.md", "docs/devops.md", "GEMINI.md"]

def run_tests():
    print("--- Generating Fresh Test Results ---")
    if os.path.exists(RESULTS_DIR):
        shutil.rmtree(RESULTS_DIR)
    
    try:
        # Run tests with coverage
        subprocess.check_call([
            "dotnet", "test", 
            "--collect:XPlat Code Coverage", 
            "--results-directory", RESULTS_DIR,
            "--verbosity", "quiet"
        ])
        print("[SUCCESS] Tests completed.")
        return True
    except subprocess.CalledProcessError:
        print("[ERROR] Tests failed during health check.")
        return False

def get_git_changes_since_file(file_path):
    try:
        last_commit = subprocess.check_output(["git", "log", "-1", "--format=%H", "--", file_path]).decode().strip()
        if not last_commit: return 0
        changes = subprocess.check_output(["git", "diff", "--name-only", last_commit, "HEAD", "--", SOURCE_DIR]).decode().strip()
        return len(set(changes.splitlines())) if changes else 0
    except:
        return 0

def check_docs_staleness():
    print("\n--- Checking Documentation Staleness ---")
    all_up_to_date = True
    for doc in DOC_FILES:
        if os.path.exists(doc):
            changes = get_git_changes_since_file(doc)
            print(f"{doc}: {changes} source files changed.")
            if changes > DOCS_STALENESS_THRESHOLD:
                print(f"[FAIL] {doc} is stale!")
                all_up_to_date = False
    return all_up_to_date

def check_monoliths():
    print("--- Checking for Monoliths (> 250 lines) ---")
    monolith_count = 0
    for root, _, files in os.walk(SOURCE_DIR):
        for file in files:
            if file.endswith(".cs") and "obj" not in root and "bin" not in root:
                path = os.path.join(root, file)
                with open(path, "r", encoding="utf-8") as f:
                    lines = len(f.readlines())
                    if lines > MAX_LINES_PER_FILE:
                        print(f"[FAIL] {path} has {lines} lines.")
                        monolith_count += 1
    return monolith_count

def parse_coverage():
    print("\n--- Checking Test Coverage ---")
    coverage_files = []
    # Search in RESULTS_DIR specifically for freshness
    search_dir = RESULTS_DIR if os.path.exists(RESULTS_DIR) else "."
    for root, _, files in os.walk(search_dir):
        for file in files:
            if re.match(COVERAGE_REPORT_PATTERN, file):
                path = os.path.join(root, file)
                coverage_files.append((path, os.path.getmtime(path)))
    
    if not coverage_files:
        print("[ERROR] Coverage report not found.")
        return 0.0, False

    # Sort by modification time descending to get the newest one
    coverage_files.sort(key=lambda x: x[1], reverse=True)
    coverage_file = coverage_files[0][0]
    print(f"Using coverage report: {coverage_file}")

    tree = ET.parse(coverage_file)
    root = tree.getroot()
    overall_line_rate = float(root.attrib["line-rate"]) * 100
    print(f"Overall Coverage: {overall_line_rate:.2f}%")

    pass_requirements = overall_line_rate >= MIN_OVERALL_COVERAGE
    for package in root.findall(".//package"):
        for cls in package.findall(".//class"):
            if float(cls.attrib["line-rate"]) * 100 < MIN_FILE_COVERAGE:
                pass_requirements = False
    return overall_line_rate, pass_requirements

def export_shields_data(monolith_count, coverage, docs_pass, tests_pass):
    data = {
        "schemaVersion": 1,
        "label": "health",
        "message": "Project Healthy" if (monolith_count == 0 and docs_pass and tests_pass) else "Project Unhealthy",
        "color": "brightgreen" if (monolith_count == 0 and docs_pass and tests_pass) else "red",
        "monoliths": "None" if monolith_count == 0 else f"{monolith_count} Found",
        "monoliths_color": "brightgreen" if monolith_count == 0 else "red",
        "coverage": f"{coverage:.0f}%",
        "coverage_color": "brightgreen" if coverage >= 70 else "yellow",
        "docs": "Up to Date" if docs_pass else "Stale",
        "docs_color": "brightgreen" if docs_pass else "red",
        "tests": "Passing" if tests_pass else "Failing",
        "tests_color": "brightgreen" if tests_pass else "red"
    }
    with open("health_data.json", "w") as f:
        json.dump(data, f, indent=4)

if __name__ == "__main__":
    skip_tests = "--skip-tests" in sys.argv
    
    tests_passed = True
    if not skip_tests:
        tests_passed = run_tests()

    m_count = check_monoliths()
    cov_val, cov_pass = parse_coverage()
    d_pass = check_docs_staleness()
    
    t_pass = cov_val > 0 and cov_pass and tests_passed
    
    export_shields_data(m_count, cov_val, d_pass, t_pass)

    if m_count > 0 or not cov_pass or not d_pass or not tests_passed:
        print("\n[FAIL] Health checks failed.")
        sys.exit(1)
    
    print("\n[SUCCESS] All health checks passed!")
    sys.exit(0)
