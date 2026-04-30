param(
    [int]$FrontendPort = 5173,
    [int]$BackendPort = 61471,
    [int]$DbPort = 1433
)

$ErrorActionPreference = "Stop"

$root = Split-Path -Parent $PSScriptRoot
$configPath = Join-Path $root "app-config.json"

$config = [ordered]@{
    Backend = [ordered]@{
        Port = $BackendPort
        Url = "http://localhost:$BackendPort"
    }
    Frontend = [ordered]@{
        Port = $FrontendPort
        Url = "http://localhost:$FrontendPort"
    }
}

$configJson = $config | ConvertTo-Json -Depth 4
$utf8NoBom = New-Object System.Text.UTF8Encoding($false)
[System.IO.File]::WriteAllText($configPath, $configJson, $utf8NoBom)

Write-Host "Ports updated:"
Write-Host "  Frontend: http://localhost:$FrontendPort"
Write-Host "  Backend:  http://localhost:$BackendPort"
Write-Host "  Database: localhost,$DbPort"
