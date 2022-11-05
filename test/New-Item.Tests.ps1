BeforeAll {
    Import-Module "$PSScriptRoot/BeforeAll.ps1" -Force
}

Describe 'New-Item' {
    It 'Given we create a new file, Then file is created' {
        New-Item -Path Sftp:/fixture/testfile.txt -ItemType File -Value "hello"

        Test-Path -Path Sftp:/fixture/testfile.txt | Should -Be $true
    }
    It 'Given -Path to existing item, Then return true' {
        $item = Test-Path -Path Sftp:/fixture
        $item | Should -Be $true
    }
}
