# Import-Module C:\Users\atodw\.nuget\packages\renci.sshnet.async\1.4.0\lib\netstandard1.3\Renci.SshNet.Async.dll
# Import-Module C:\Users\atodw\.nuget\packages\ssh.net\2016.1.0\lib\netstandard1.3\Renci.SshNet.dll
# Import-Module C:\Users\atodw\.nuget\packages\sshnet.security.cryptography\1.2.0\lib\netstandard1.3\SshNet.Security.Cryptography.dll
# Import-Module "$PSScriptRoot/src/PSSftpProvider/bin/Debug/netstandard2.0/PSSftpProvider.dll"

$publishPath = "$PSScriptRoot/src/PSSftpProvider/bin/Debug/netstandard2.0/publish"
Import-Module "$publishPath/SshNet.Security.Cryptography.dll"
Import-Module "$publishPath/Renci.SshNet.dll"
Import-Module "$publishPath/Renci.SshNet.Async.dll"
Import-Module "$publishPath/PSSftpProvider.dll"
