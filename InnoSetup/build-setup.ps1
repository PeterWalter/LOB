<#
.SYNOPSIS
    Builds the CETAP LOB Release application and compiles the Inno Setup installer.

.EXAMPLE
    .\build-setup.ps1
    Builds CETAP_LOB.sln in Release, then compiles CETAP_LOB.iss.

.EXAMPLE
    .\build-setup.ps1 -SkipBuild -SourceDir '..\Obfuscator_Output'
    Compiles only, packaging an already obfuscated build.
#>
[CmdletBinding()]
param(
    [string]$Configuration = 'Release',
    [string]$SourceDir,
    [string]$OutputDir = 'Output',
    [string]$InnoSetupPath,
    [switch]$SkipBuild
)

$ErrorActionPreference = 'Stop'
$here = Split-Path -Parent $MyInvocation.MyCommand.Path
$repoRoot = Split-Path -Parent $here
$solution = Join-Path $repoRoot 'CETAP_LOB.sln'

function Find-MSBuild {
    $vswhere = Join-Path ${env:ProgramFiles(x86)} 'Microsoft Visual Studio\Installer\vswhere.exe'
    if (Test-Path $vswhere) {
        $found = & $vswhere -latest -products * -requires Microsoft.Component.MSBuild -find 'MSBuild\**\Bin\MSBuild.exe' |
                 Select-Object -First 1
        if ($found) { return $found }
    }
    foreach ($candidate in @(
        'C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe',
        'C:\Program Files\Microsoft Visual Studio\2022\Professional\MSBuild\Current\Bin\MSBuild.exe',
        'C:\Program Files\Microsoft Visual Studio\2022\Enterprise\MSBuild\Current\Bin\MSBuild.exe')) {
        if (Test-Path $candidate) { return $candidate }
    }
    $onPath = Get-Command msbuild -ErrorAction SilentlyContinue
    if ($onPath) { return $onPath.Source }
    return $null
}

function Find-ISCC {
    if ($InnoSetupPath) {
        if (Test-Path $InnoSetupPath) { return $InnoSetupPath }
        throw "ISCC.exe not found at '$InnoSetupPath'."
    }
    foreach ($candidate in @(
        (Join-Path ${env:ProgramFiles(x86)} 'Inno Setup 6\ISCC.exe'),
        (Join-Path $env:ProgramFiles 'Inno Setup 6\ISCC.exe'),
        (Join-Path $env:LOCALAPPDATA 'Programs\Inno Setup 6\ISCC.exe'))) {
        if ($candidate -and (Test-Path $candidate)) { return $candidate }
    }
    $onPath = Get-Command iscc -ErrorAction SilentlyContinue
    if ($onPath) { return $onPath.Source }
    return $null
}

if (-not $SkipBuild) {
    $msbuild = Find-MSBuild
    if (-not $msbuild) { throw 'MSBuild was not found. Install Visual Studio 2022 or the Build Tools.' }
    Write-Host "Building $solution ($Configuration) with $msbuild"
    & $msbuild $solution /t:Build "/p:Configuration=$Configuration" /m /nologo /v:m
    if ($LASTEXITCODE -ne 0) { throw "The application build failed (exit $LASTEXITCODE)." }
    Write-Host 'Application build succeeded.'
}

if (-not $SourceDir) { $SourceDir = Join-Path $repoRoot 'CETAP_LOB\bin\Release' }
if (-not (Test-Path (Join-Path $SourceDir 'CETAP_LOB.exe'))) {
    throw "No CETAP_LOB.exe in '$SourceDir'. Build the $Configuration configuration first."
}

$iscc = Find-ISCC
if (-not $iscc) {
    Write-Host ''
    Write-Host 'Inno Setup 6 was not found, so the installer could not be compiled.' -ForegroundColor Yellow
    Write-Host 'Install it from https://jrsoftware.org/isdl.php and run this script again,'
    Write-Host 'or pass -InnoSetupPath to point at your ISCC.exe.'
    exit 1
}

$iss = Join-Path $here 'CETAP_LOB.iss'
$outputPath = Join-Path $here $OutputDir
Write-Host "Compiling $iss"
& $iscc "/DSourceDir=$SourceDir" "/DOutputDir=$outputPath" $iss
if ($LASTEXITCODE -ne 0) { throw "The installer build failed (exit $LASTEXITCODE)." }

$installer = Get-ChildItem $outputPath -Filter 'CETAP_LOB_Setup_*.exe' | Sort-Object LastWriteTime | Select-Object -Last 1
if ($installer) { Write-Host "Installer created: $($installer.FullName)" -ForegroundColor Green }
