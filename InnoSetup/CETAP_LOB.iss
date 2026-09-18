; ---------------------------------------------------------------------------
; CETAP LOB - Inno Setup script
;
; Packaging
;   1. Build the application in Release (see build-setup.ps1 or README.md).
;   2. Compile this script with Inno Setup 6:
;          ISCC.exe CETAP_LOB.iss
;      or open it in the Inno Setup IDE and press F9.
;
; The application folder and the installer output folder can be overridden
; without editing this file, for example to package an obfuscated build:
;      ISCC.exe /DSourceDir="..\Obfuscator_Output" /DOutputDir="Output" CETAP_LOB.iss
; ---------------------------------------------------------------------------

#define MyAppName      "CETAP LOB"
#define MyAppVersion   "1.5"
#define MyAppPublisher "CETAP"
#define MyAppExeName   "CETAP_LOB.exe"
#define MyAppId        "{{8E2C4C93-3B3D-4A16-9C2B-7E5A1D4F6B21}"

; Folder holding the built application (CETAP_LOB.exe and its libraries).
#ifndef SourceDir
  #define SourceDir "..\CETAP_LOB\bin\Release"
#endif

; Folder the finished installer is written to.
#ifndef OutputDir
  #define OutputDir "Output"
#endif

[Setup]
AppId={#MyAppId}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
AppVerName={#MyAppName} {#MyAppVersion}
AppPublisher={#MyAppPublisher}
VersionInfoVersion=1.5.0.0
DefaultDirName={autopf}\CETAP LOB
DefaultGroupName={#MyAppName}
DisableProgramGroupPage=yes
OutputDir={#OutputDir}
OutputBaseFilename=CETAP_LOB_Setup_{#MyAppVersion}
SetupIconFile=..\CETAP_LOB\c_logo.ico
UninstallDisplayIcon={app}\{#MyAppExeName}
Compression=lzma2/max
SolidCompression=yes
WizardStyle=modern
PrivilegesRequired=admin
ArchitecturesAllowed=x86compatible x64compatible
MinVersion=6.1sp1

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: checkedonce

[Files]
; Application, libraries and data files. The data files below are opened by
; relative name (CetapDat.dat, PackingList.xlsx, nbt.png, cea.png), which is why
; every shortcut sets WorkingDir to {app}.
; Only the top level is taken, so the ClickOnce publish folder (app.publish) that
; a Release build leaves behind is not packaged.
Source: "{#SourceDir}\*"; DestDir: "{app}"; Flags: ignoreversion; Excludes: "*.pdb,*.xml,*.cs,*.application,*.vshost.exe,*.vshost.exe.manifest,PackingList.xlsx,PackingList.xlsm,PackingList.xltx"

; Satellite resource assemblies and the native SQL Server spatial libraries.
Source: "{#SourceDir}\de\*"; DestDir: "{app}\de"; Flags: ignoreversion
Source: "{#SourceDir}\es\*"; DestDir: "{app}\es"; Flags: ignoreversion
Source: "{#SourceDir}\fr\*"; DestDir: "{app}\fr"; Flags: ignoreversion
Source: "{#SourceDir}\it\*"; DestDir: "{app}\it"; Flags: ignoreversion
Source: "{#SourceDir}\ja\*"; DestDir: "{app}\ja"; Flags: ignoreversion
Source: "{#SourceDir}\ko\*"; DestDir: "{app}\ko"; Flags: ignoreversion
Source: "{#SourceDir}\pt\*"; DestDir: "{app}\pt"; Flags: ignoreversion
Source: "{#SourceDir}\ru\*"; DestDir: "{app}\ru"; Flags: ignoreversion
Source: "{#SourceDir}\zh-Hans\*"; DestDir: "{app}\zh-Hans"; Flags: ignoreversion
Source: "{#SourceDir}\zh-Hant\*"; DestDir: "{app}\zh-Hant"; Flags: ignoreversion
Source: "{#SourceDir}\x86\*"; DestDir: "{app}\x86"; Flags: ignoreversion
Source: "{#SourceDir}\x64\*"; DestDir: "{app}\x64"; Flags: ignoreversion

; Operator templates - installed once and never overwritten by an upgrade, so a
; customised copy survives.
Source: "{#SourceDir}\PackingList.xlsx"; DestDir: "{app}"; Flags: onlyifdoesntexist
Source: "{#SourceDir}\PackingList.xlsm"; DestDir: "{app}"; Flags: onlyifdoesntexist
Source: "{#SourceDir}\PackingList.xltx"; DestDir: "{app}"; Flags: onlyifdoesntexist

; Reference material shipped with the product.
Source: "..\Required Templates\*"; DestDir: "{app}\Required Templates"; Flags: ignoreversion recursesubdirs createallsubdirs

[Icons]
Name: "{autoprograms}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; WorkingDir: "{app}"
Name: "{autodesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; WorkingDir: "{app}"; Tasks: desktopicon

[Run]
Filename: "{app}\{#MyAppExeName}"; Description: "{cm:LaunchProgram,{#StringChange(MyAppName, '&', '&&')}}"; WorkingDir: "{app}"; Flags: nowait postinstall skipifsilent

[Code]
// .NET Framework 4.8 is required by the application (TargetFrameworkVersion v4.8).
const
  Net48Release = 528040;

function IsDotNet48Installed: Boolean;
var
  Release: Cardinal;
begin
  Result := RegQueryDWordValue(HKLM,
              'SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full',
              'Release', Release) and (Release >= Net48Release);
end;

function InitializeSetup: Boolean;
var
  ErrorCode: Integer;
begin
  Result := IsDotNet48Installed;
  if not Result then
  begin
    if MsgBox('.NET Framework 4.8 is required but was not detected on this computer.' + #13#10 + #13#10 +
              'Open the Microsoft download page?', mbConfirmation, MB_YESNO) = IDYES then
      ShellExec('open', 'https://dotnet.microsoft.com/download/dotnet-framework/net48',
                '', '', SW_SHOWNORMAL, ewNoWait, ErrorCode);
  end;
end;
