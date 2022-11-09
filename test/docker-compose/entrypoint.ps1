& dotnet build
& dotnet publish

Start-Sleep -Seconds 2

Set-Location -Path /app/test

Import-Module Pester

Invoke-Pester
