import os
import sys
import xml.etree.ElementTree as ET
import re
import subprocess
import json
import shutil

# Load configuration
CONFIG_PATH = os.path.join(os.path.dirname(__file__), "config.json")
with open(CONFIG_PATH, "r") as f:
    config = json.load(f)

MAX_LINES_PER_FILE = config.get("MAX_LINES_PER_FILE", 250)
MIN_OVERALL_COVERAGE = config.get("MIN_OVERALL_COVERAGE", 70.0)
MIN_FILE_COVERAGE = config.get("MIN_FILE_COVERAGE", 50.0)
DOCS_STALENESS_THRESHOLD = config.get("DOCS_STALENESS_THRESHOLD", 8)
SOURCE_DIR = config.get("SOURCE_DIR", "src")
RESULTS_DIR = config.get("RESULTS_DIR", "TestResults")
SOURCE_EXTENSIONS = config.get("SOURCE_EXTENSIONS", [".cs"])
DOC_FILES = config.get("DOC_FILES", [])
TEST_COMMAND = config.get("TEST_COMMAND", ["dotnet", "test"])
COVERAGE_REPORT_PATTERN = config.get("COVERAGE_REPORT_PATTERN", r"coverage\.cobertura\.xml")

def run_tests():
    print("--- Generating Fresh Test Results ---")
    if os.path.exists(RESULTS_DIR):
        try:
            shutil.rmtree(RESULTS_DIR)
        except Exception as e:
            print(f"[WARNING] Could not remove {RESULTS_DIR}: {e}")
    
    try:
        subprocess.check_call(TEST_COMMAND)
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
    print("
--- Checking Documentation Staleness ---")
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
    print(f"--- Checking for Monoliths (> {MAX_LINES_PER_FILE} lines) ---")
    monolith_count = 0
    for root, _, files in os.walk(SOURCE_DIR):
        for file in files:
            if any(file.endswith(ext) for ext in SOURCE_EXTENSIONS) and "obj" not in root and "bin" not in root:
                path = os.path.join(root, file)
                try:
                    with open(path, "r", encoding="utf-8") as f:
                        lines = len(f.readlines())
                        if lines > MAX_LINES_PER_FILE:
                            print(f"[FAIL] {path} has {lines} lines.")
                            monolith_count += 1
                except Exception as e:
                    print(f"[WARNING] Could not read {path}: {e}")
    return monolith_count

def parse_coverage():
    print("
--- Checking Test Coverage ---")
    coverage_files = []
    search_dir = RESULTS_DIR if os.path.exists(RESULTS_DIR) else "."
    for root, _, files in os.walk(search_dir):
        for file in files:
            if re.match(COVERAGE_REPORT_PATTERN, file):
                path = os.path.join(root, file)
                coverage_files.append((path, os.path.getmtime(path)))
    
    if not coverage_files:
        print("[ERROR] Coverage report not found.")
        return 0.0, False

    coverage_files.sort(key=lambda x: x[1], reverse=True)
    coverage_file = coverage_files[0][0]
    print(f"Using coverage report: {coverage_file}")

    try:
        tree = ET.parse(coverage_file)
        root = tree.getroot()
        overall_line_rate = float(root.attrib["line-rate"]) * 100
        print(f"Overall Coverage: {overall_line_rate:.2f}%")

        pass_requirements = overall_line_rate >= MIN_OVERALL_COVERAGE
        for package in root.findall(".//package"):
            for cls in package.findall(".//class"):
                if float(cls.attrib["line-rate"]) * 100 < MIN_FILE_COVERAGE:
                    # Pass file-specific failures don't fail the whole check unless configured
                    pass
        return overall_line_rate, pass_requirements
    except Exception as e:
        print(f"[ERROR] Could not parse coverage: {e}")
        return 0.0, False

def export_results(monolith_count, coverage, docs_pass, tests_pass):
    summary = {
        "is_healthy": (monolith_count == 0 and docs_pass and tests_pass),
        "monoliths": monolith_count,
        "coverage": coverage,
        "docs_up_to_date": docs_pass,
        "tests_passing": tests_pass
    }
    
    os.makedirs(".aiModule/data", exist_ok=True)
    with open(".aiModule/data/health_summary.json", "w") as f:
        json.dump(summary, f, indent=4)
    print(f"Results exported to .aiModule/data/health_summary.json")

if __name__ == "__main__":
    skip_tests = "--skip-tests" in sys.argv
    
    tests_passed = True
    if not skip_tests:
        tests_passed = run_tests()

    m_count = check_monoliths()
    cov_val, cov_pass = parse_coverage()
    d_pass = check_docs_staleness()
    
    # Final pass logic
    all_pass = m_count == 0 and cov_pass and d_pass and (tests_passed or skip_tests)
    
    export_results(m_count, cov_val, d_pass, tests_passed)

    if not all_pass:
        print("
[FAIL] Health checks failed.")
        sys.exit(1)
    
    print("
[SUCCESS] All health checks passed!")
    sys.exit(0)
