$publishPath = "$PSScriptRoot/src/PSSftpProvider/bin/Debug/netstandard2.0/publish"

Import-Module "$publishPath/SshNet.Security.Cryptography.dll"
Import-Module "$publishPath/Renci.SshNet.dll"
Import-Module "$publishPath/Renci.SshNet.Async.dll"
Import-Module "$publishPath/PSSftpProvider.dll"

$cred = New-Object System.Management.Automation.PSCredential ("foo", ("pass" | ConvertTo-SecureString -AsPlainText -Force))
    
New-PSDrive -PSProvider Sftp -Root sftp://localhost:2222/ -Name sftp -Credential $cred
