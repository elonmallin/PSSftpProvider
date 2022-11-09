# $isRunning = -not [string]::IsNullOrWhiteSpace(((& docker-compose ps --services --filter "status=running") | Select-String sftp))
# $isListening = -not [string]::IsNullOrWhiteSpace(((& docker-compose logs sftp) | Select-String "Server listening on :: port 22"))
# $isReady = $isRunning -and $isListening

# Write-Output "IsSftpReady $isReady,IsSftpRunning $isRunning, IsSftpListening $isListening"

Import-Module "$PSScriptRoot/../PSSftpProvider.psm1" -Force

if (-not ((Get-PSDrive).Provider.Name -contains "Sftp")) {
    $cred = New-Object System.Management.Automation.PSCredential ("foo", ("pass" | ConvertTo-SecureString -AsPlainText -Force))
    
    New-PSDrive -PSProvider Sftp -Root sftp://localhost:22 -Name Sftp -Credential $cred
}
