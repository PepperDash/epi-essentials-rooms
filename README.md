# PDT.Plugins.Essentials.Rooms

A PepperDash Essentials plugin that provides standard room types for use with the Essentials v1.x and v2.x runtime.

## License

Provided under MIT license

## Room Types

| Config `type` | Class |
|---|---|
| `huddle` | `EssentialsHuddleSpaceRoom` |
| `huddlevtc1` | `EssentialsHuddleVtc1Room` |
| `dualdisplay` | `EssentialsDualDisplayRoom` |
| `combinedhuddlevtc1` | `EssentialsCombinedHuddleVtc1Room` |
| `techroom` | `EssentialsTechRoom` |

## Compatibility

| Target | .NET | Essentials | Project file | Build output |
|---|---|---|---|---|
| 3-series processors | net3.5 (SimplSharp) | v1.x (≤1.15.x) | `PDT.Plugins.Essentials.Rooms.sln` | `PDT.Plugins.Essentials.Rooms.dll` |
| 4-series processors (CP4, etc.) | net4.7.2 | v2.28.0+ | `PDT.Plugins.Essentials.Rooms.4.72.sln` | `PDT.Plugins.Essentials.Rooms.dll` |

The two project files target the same source files. API differences between v1 and v2 are handled via `#if ESSENTIALS_V2` conditional compilation — the `4.72` project defines this constant, the `3.5` project does not.

---

## Dependencies — net4.7.2 / Essentials v2 (4-series)

### 1. Crestron SDK (SimplSharpPro.exe)

Install the [Crestron Toolbox](https://www.crestron.com/Software-Firmware/Software/Crestron-Toolbox). This places `SimplSharpPro.exe` at:

```
C:\ProgramData\Crestron\SDK\SimplSharpPro.exe
```

The `4.72` project references this path directly.

### 2. Crestron SimplSharp SDK NuGet package

The `4.72` project requires `Crestron.SimplSharp.SDK.Library` v2.21.226 via NuGet. Restore it from the `src` directory:

```bat
cd src
nuget install packages.config -OutputDirectory ..\packages -excludeVersion
```

Or open `PDT.Plugins.Essentials.Rooms.4.72.sln` in Visual Studio 2022 and let NuGet restore automatically on build.

### 3. PepperDash Essentials v2 runtime DLLs

The `lib/` folder in this repository contains the four DLLs required for compilation, extracted from `PepperDashEssentials.2.28.0.net472.cpz`:

| File | Source |
|---|---|
| `lib/PepperDashCore.dll` | PepperDashEssentials CPZ |
| `lib/PepperDash_Essentials_Core.dll` | PepperDashEssentials CPZ |
| `lib/Essentials Devices Common.dll` | PepperDashEssentials CPZ |
| `lib/Serilog.dll` | PepperDashEssentials CPZ |

These are committed to the repository so no additional steps are needed. If you need to update them for a newer Essentials release, extract the new CPZ (it is a ZIP archive) and replace these four files.

> The CPZ itself is **not** included in this repo. Obtain the target version from the [PepperDash Essentials releases](https://github.com/PepperDash/Essentials/releases) page and extract it with any ZIP tool.

---

## Dependencies — net3.5 / Essentials v1 (3-series)

### 1. Crestron SDK

The same Crestron Toolbox installation provides the net3.5 SDK DLLs referenced from `C:\ProgramData\Crestron\SDK\`.

### 2. PepperDash Essentials v1 NuGet package

From the repository root:

```bat
nuget install .\packages.config -OutputDirectory .\packages -excludeVersion
```

Or run `GetPackages.BAT`.

This installs `PepperDashEssentials` v1.15.x to `packages\PepperDashEssentials\lib\net35\`, which is where the `3.5` project's references point.

### 3. Compact Framework build tools

The net3.5 project targets the Crestron SimplSharp platform (Windows CE / Compact Framework). Building it requires the Compact Framework MSBuild targets, which are installed alongside Visual Studio by the Crestron SDK toolchain. Visual Studio 2022 does not include these by default — you may need Visual Studio 2019 or earlier, or the standalone Crestron build tools, to compile the net3.5 target.

---

## Building

### Visual Studio

Open the solution for the target you want to build:

- **net4.7.2 / v2:** `PDT.Plugins.Essentials.Rooms.4.72.sln` — requires Visual Studio 2022
- **net3.5 / v1:** `PDT.Plugins.Essentials.Rooms.sln` — requires Visual Studio 2019 or earlier with Compact Framework tools

Build the solution in **Release** configuration. The output DLL is placed in `src\bin\Release\PDT.Plugins.Essentials.Rooms.dll`.

### MSBuild (command line)

**net4.7.2 / v2:**

```bat
msbuild src\PDT.Plugins.Essentials.Rooms.4.72.csproj /p:Configuration=Release
```

**net3.5 / v1** (requires Compact Framework MSBuild targets):

```bat
msbuild src\PDT.Plugins.Essentials.Rooms.csproj /p:Configuration=Release
```

---

## Deployment

PepperDash Essentials loads plugins from a `.cplz` file — a ZIP archive with a renamed extension. To package and deploy the built DLL:

### Package as .cplz

```powershell
$dll  = "src\bin\Release\PDT.Plugins.Essentials.Rooms.dll"
$zip  = "PDT.Plugins.Essentials.Rooms.cplz.zip"
$cplz = "PDT.Plugins.Essentials.Rooms.cplz"

Compress-Archive -Path $dll -DestinationPath $zip -Force
Rename-Item $zip $cplz -Force
```

### Upload to processor

Copy the `.cplz` file to the processor's plugin directory over SFTP:

- **Path:** `/user/program{slot}/plugins/`  (e.g. `/user/program1/plugins/`)

On next program load, Essentials will unzip the `.cplz` and load `PDT.Plugins.Essentials.Rooms.dll` automatically via reflection.

> The plugin DLL must be compiled against the same Essentials version running on the processor. Mismatched versions will cause the factory to fail to register at startup.

---

## Conditional Compilation Reference

The preprocessor symbol `ESSENTIALS_V2` is defined in the net4.7.2 project only. The following guards are present in the shared source:

| Symbol | Defined in | Effect |
|---|---|---|
| `ESSENTIALS_V2` | `4.72.csproj` | Selects v2 API surface |
| *(not defined)* | `3.5.csproj` | Selects v1 API surface |

Key differences guarded by `#if ESSENTIALS_V2`:

| Location | v2 (`ESSENTIALS_V2`) | v1 |
|---|---|---|
| `DisplayBase` / `TwoWayDisplayBase` namespace | `PepperDash.Essentials.Devices.Common.Displays` | `PepperDash.Essentials.Core` |
| `IHasDefaultDisplay.DefaultDisplay` type | `IRoutingSink` | `IRoutingSinkWithSwitching` |
| `IHasMultipleDisplays.Displays` value type | `IRoutingSink` | `IRoutingSinkWithSwitching` |
| `EssentialsRoomsDeviceFactory` | `TypeNames` + `BuildDevice()` | `LoadTypeFactories()` |
| `EssentialsTechRoom.RunDirectRoute` | 3-param overload present | omitted |
| `EssentialsTechRoomConfig` type alias | present (resolves ambiguity) | omitted |
