BeforeAll {
    Import-Module "$PSScriptRoot/BeforeAll.ps1" -Force
}

Describe 'New-Item' {
    It 'Given we create a new file, Then file is created' {
        New-Item -Path Sftp:/upload/testfile.txt -ItemType File -Value "hello"

        Test-Path -Path Sftp:/upload/testfile.txt | Should -Be $true
    }
}
