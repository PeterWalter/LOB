# CETAP LOB — Installer (Inno Setup)

This folder builds the Windows installer for the CETAP LOB desktop application.

| File | Purpose |
|---|---|
| `CETAP_LOB.iss` | Inno Setup script — what is installed, shortcuts, prerequisites |
| `build-setup.ps1` | Builds the Release application and compiles the installer in one step |

---

## Prerequisites

1. **Visual Studio 2022** (or MSBuild) to build `CETAP_LOB.sln`.
2. **Inno Setup 6** — https://jrsoftware.org/isdl.php (the script uses `{autopf}` and
   `WizardStyle`, which need version 6).
3. On the **target** machine: **.NET Framework 4.8**. The installer checks for it and
   offers the Microsoft download page if it is missing.

---

## Build the installer

### One step

```powershell
cd InnoSetup
.\build-setup.ps1
```

This builds `CETAP_LOB.sln` in **Release** and then compiles the script. The finished
installer is written to `InnoSetup\Output\CETAP_LOB_Setup_1.5.exe`.

Useful switches:

```powershell
.\build-setup.ps1 -SkipBuild                       # compile only, using the existing output
.\build-setup.ps1 -SourceDir '..\Obfuscator_Output' # package an obfuscated build
.\build-setup.ps1 -InnoSetupPath 'D:\Inno Setup 6\ISCC.exe'
```

### Manually

```powershell
# 1. build the application
msbuild CETAP_LOB.sln /t:Build /p:Configuration=Release

# 2. compile the installer
%ProgramFiles(x86)%\Inno Setup 6\ISCC.exe InnoSetup\CETAP_LOB.iss
```

Or open `CETAP_LOB.iss` in the Inno Setup IDE and press **F9**.

The application folder and output folder can be overridden from the command line
without editing the script:

```
ISCC.exe /DSourceDir="..\CETAP_LOB\bin\Release" /DOutputDir="Output" CETAP_LOB.iss
```

---

## What is installed

* The application and its libraries from the **Release** output folder
  (`CETAP_LOB.exe`, `CETAP_LOB.exe.config`, `CETAP_LOB.exe.manifest` and the dependent
  assemblies). Debug symbols (`*.pdb`), XML documentation (`*.xml`), stray source files
  and the ClickOnce manifest (`*.application`) are excluded.
* Runtime data files the application opens **by relative name** — `CetapDat.dat`,
  `PackingList.xlsx`, `nbt.png`, `cea.png`.
* `Required Templates\NBT RESULTS.docx` (reference material).
* A **Start menu** entry and an optional **desktop** shortcut, and an uninstaller.

### Why `WorkingDir` matters

`PackingList.xlsx`, `CetapDat.dat`, `nbt.png` and `cea.png` are opened by bare file name
(`new XLWorkbook("PackingList.xlsx")`, `new StreamReader("CetapDat.dat")`, …). Relative
names resolve against the process **working directory**, so every shortcut created by
the installer sets `WorkingDir: "{app}"`. Always launch the application from its
shortcut, not by double-clicking the exe from another folder.

---

## Notes

* **Install location** — `{autopf}\CETAP LOB`, which on 64-bit Windows is
  `C:\Program Files (x86)\CETAP LOB` (the application is `AnyCPU` and installs in
  32-bit mode, matching the existing deployment).
* **User settings** — `ApplicationSettings` are per user, stored under the user's
  `%LOCALAPPDATA%` profile, so they are untouched by install, upgrade and uninstall.
* **Operator templates** — `PackingList.xlsx`, `PackingList.xlsm` and `PackingList.xltx`
  are installed only if absent, so a customised copy is never overwritten by an upgrade.
* **Upgrades** — a new version installs over the old one (the same `AppId`), keeping the
  install folder and the uninstall entry.
* **Obfuscated builds** — the production pipeline can obfuscate first (see `obfuscar.xml`
  at the repository root); point `-SourceDir` at that output to package it.
* **Code signing** — the script does not sign the installer or the executable. Add a
  `SignTool` directive in `[Setup]` if signed output is required.
