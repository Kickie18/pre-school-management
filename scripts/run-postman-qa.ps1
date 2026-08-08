param(
    [string]$BaseUrl = "https://localhost:7147",
    [string]$Collection = "postman/PreschoolManagement.postman_collection.json",
    [string]$Environment = "postman/PreschoolManagement.postman_environment.json",
    [string]$ReportOut = "postman/newman-report.json"
)

$ErrorActionPreference = "Stop"

$workspaceRoot = Split-Path -Parent $PSScriptRoot
Set-Location $workspaceRoot

if (-not (Test-Path $Collection)) {
    throw "Collection file not found: $Collection"
}

if (-not (Test-Path $Environment)) {
    throw "Environment file not found: $Environment"
}

Write-Host "Running Postman QA collection against $BaseUrl"

$newman = Get-Command newman -ErrorAction SilentlyContinue
if ($newman) {
    & newman run $Collection `
        -e $Environment `
        --env-var "baseUrl=$BaseUrl" `
        --insecure `
        --reporters cli,json `
        --reporter-json-export $ReportOut
}
else {
    Write-Host "newman not found globally. Using npx newman..."
    & npx newman run $Collection `
        -e $Environment `
        --env-var "baseUrl=$BaseUrl" `
        --insecure `
        --reporters cli,json `
        --reporter-json-export $ReportOut
}

if ($LASTEXITCODE -ne 0) {
    throw "Newman run failed. See report: $ReportOut"
}

Write-Host "Postman QA run succeeded. Report: $ReportOut"
