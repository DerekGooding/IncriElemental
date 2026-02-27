# build.ps1 - Standard Build Script
# This script builds the entire solution.

Write-Host "Building IncriElemental..." -ForegroundColor Green
dotnet build IncriElemental.sln
if ($LASTEXITCODE -ne 0) {
    Write-Host "Build failed." -ForegroundColor Red
    exit $LASTEXITCODE
}
Write-Host "Build successful!" -ForegroundColor Green
