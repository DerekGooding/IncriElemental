# validate.ps1 - Standard Validation Script
# This script runs the entire solution tests and checks for errors.

Write-Host "Validating IncriElemental..." -ForegroundColor Green
dotnet test tests/IncriElemental.Tests/IncriElemental.Tests.csproj
if ($LASTEXITCODE -ne 0) {
    Write-Host "Tests failed." -ForegroundColor Red
    exit $LASTEXITCODE
}
Write-Host "All tests passed!" -ForegroundColor Green
