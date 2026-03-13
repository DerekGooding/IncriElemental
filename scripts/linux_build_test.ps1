# PowerShell script to run local Linux build test via Docker

$IMAGE_NAME = "incrielemental-linux-test"

Write-Host "--- Building Docker Image: $IMAGE_NAME ---"
docker build -f Dockerfile.linux_test -t $IMAGE_NAME .

if ($LASTEXITCODE -ne 0) {
    Write-Host "[FAIL] Docker build failed." -ForegroundColor Red
    exit 1
}

Write-Host "--- Running Linux Build Test ---"
docker run --rm $IMAGE_NAME

if ($LASTEXITCODE -ne 0) {
    Write-Host "[FAIL] Linux build test inside Docker failed." -ForegroundColor Red
    exit 1
}

Write-Host "[SUCCESS] Linux build test passed!" -ForegroundColor Green
exit 0
