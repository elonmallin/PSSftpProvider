BeforeAll {
    Import-Module "$PSScriptRoot/BeforeAll.ps1" -Force
}

Describe 'Test-Path' {
    It 'Given -Path to non existing item, Then return false' {
        $item = Test-Path -Path Sftp:/not-exist
        $item | Should -Be $false
    }
    It 'Given -Path to existing item, Then return true' {
        $item = Test-Path -Path Sftp:/fixture
        $item | Should -Be $true
    }
}
