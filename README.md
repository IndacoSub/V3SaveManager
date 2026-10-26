# V3SaveManager

A save editor dedicated to the game "Danganronpa V3: Killing Harmony", now with a Graphical User Interface ("GUI") as well.

Keep in mind that it's not feature-complete, but still a work-in-progress ("WIP"), which may or may not be accurate.

Contributions and bug/issue-reports are welcome and appreciated.

# Compatibility

This program has only been tested using the "legacy" Steam PC version of the game.

IT *MAY OR MAY NOT* WORK WITH V3 "ANNIVERSARY EDITION" SAVEFILES.

We currently don't have any plans to support V3 "Anniversary Edition" savefiles.

Don't edit/use this program, or suite of programs, unless you acknowledge that know exactly what you're doing, as your newly-edited save files may not work as expected, or even corrupt.

**You have been warned.**

We are not responsible and/or liable for any damage, and we won't help you recover your savefiles either.

**Make backups** frequently, possibly on multiple devices and/or storage devices.

# GUI

The GUI provides you with some options to easily edit your savefiles.

It does NOT contain "as many" features as the command-line ("CLI") version.

Please consider using the CLI version instead, if you need to edit data which is not accessible through the GUI version (some backlog data, for example).

# GUI -- How to use

It's very easy to use, you really don't need a guide.

Just make sure to save your changes, and make sure that you're not overwriting any of your backup files.

# CLI

The non-GUI ("CLI") version is NOT USER-FRIENDLY!

You'll need to compile it every time you want to edit something.

If you just run it without changing anything, it won't do anything.

# CLI -- How-to-use

After making the changes you desire in the "Program.cs" file, and compiling the program, run the program with the savefile as the first argument.

A new file called "mysave.dat" will be created, possibly in the same folder as the savefile you used as an argument.

Move and rename the file, if needed.

## Why it's called "save manager" instead of "save editor"?

The name was chosen early on in development (May/June 2022), but there are currently no features planned other than save editing.

## Licensing ##

This is licensed under the ISC License, with exemptions for specific companies or groups noted below.

Exemptions:

    Spike Chunsoft Co., Ltd. may, at their discretion, relicense any code under either the MIT license or the GPLv2 license.
    NIS America Inc. may, at their discretion, relicense any code under either the MIT license or the GPLv2 license.
    Too Kyo Games, LLC may, at their discretion, relicense any code under either the MIT license or the GPLv2 license.

## Credits ##

     “DANGANRONPA” is a registered trademark of Spike Chunsoft Co., Ltd., Too Kyo Games, LLC and NIS America Inc.
     We are not in any way affiliated or associated with them.
	 
	 Thanks to the members of the "SPIRAL" Discord server and the Spiral Framework project (https://spiralframework.info/ -- source code here: https://github.com/SpiralFramework/Spiral).
	 Special thanks to Ziella.
	 
	 Thanks to all of our collaborators and reviewers.
## Rewritten applications and shared library

The applications now share `V3SaveManager.Core`, a single binary schema and editing engine. The instructions below supersede the original CLI instructions above: editing `Program.cs` and recompiling for each change is no longer necessary.

The schema includes **all 144 members** declared by the original save implementation, including `Unk*`, `ToDo*`, backlog data, flags, and every voice-line slot. Its fixed region is 279,528 bytes (`0x443E8`). Offsets follow the original binary read/write order. Additional trailing bytes are preserved unchanged. Files shorter than this region are rejected. This is a structural check, not proof that a file is a compatible game save; the original compatibility notes still apply.

### Build and checks

Use the .NET 6 SDK (6.0.428 was used for validation):

```sh
dotnet build V3SaveManager.sln -c Release -p:EnableWindowsTargeting=true
dotnet run --project V3SaveManager.Tests -c Release
```

The Windows-targeting option allows cross-compilation on Linux. The GUI itself requires Windows and the .NET 6 Windows Desktop runtime. The CLI and regression checks run on Linux and Windows. .NET 6 is out of support; the target is retained for compatibility with the existing project.

The regression runner uses no test-framework packages and exits nonzero on failure. It checks binary layout, byte preservation, typed edits, invalid input, atomic batch editing, file replacement, CLI behavior, and spreadsheet clipboard operations. Synthetic fixtures do not establish compatibility with real game saves.

### Command-line editor

The executable remains `V3SaveManager`. From a source checkout, run it with `dotnet run --project V3SaveManager -c Release --` followed by a command:

```sh
# Discover every member and its binary location.
dotnet run --project V3SaveManager -c Release -- fields

# Inspect a save or read one member.
dotnet run --project V3SaveManager -c Release -- inspect backup.dat --filter Coins
dotnet run --project V3SaveManager -c Release -- get backup.dat Monocoins

# Apply multiple changes without changing the input file.
dotnet run --project V3SaveManager -c Release -- set backup.dat Monocoins=999 CurrentLevel=50 CurrentLevelShort=50 --output edited.dat

# Choose a different interpretation, including raw bytes.
dotnet run --project V3SaveManager -c Release -- set backup.dat Unk1=01020304 --format Hex --output edited.dat

# Export every member as lossless hexadecimal JSON and import all or part of it.
dotnet run --project V3SaveManager -c Release -- export backup.dat --output fields.json
dotnet run --project V3SaveManager -c Release -- import backup.dat fields.json --output edited.dat
```

Use `--help` for the complete grammar and supported formats. `--force` explicitly permits replacing an existing output. Numeric input uses invariant culture (a period for decimals) and little-endian binary representation. Hex input must contain exactly the member's byte count. Text is fixed-width and cannot be silently truncated. Batch edits are validated before being applied. Related members such as `CurrentLevel` / `CurrentLevelShort` and `CurrentMapID` / `CurrentMapID_Again` remain independent: update both when appropriate.

JSON uses the shape `{"version":1,"fields":{"Monocoins":"E703"}}`. A full export contains every member; a partial import changes only its listed members. Import retains trailing bytes from the input save.

Exit codes are 0 for success/help, 2 for invalid arguments or data, and 3 for file/access errors. Errors are written to standard error.

### Graphical editor and expert mode

On Windows, run `V3SaveManagerGUI`. Open a copy of a save, edit values, and use Save As to create an edited file. The normal view exposes recognized fields. Enable **Expert mode** to expose every declared member, including unknown and reserved data. Filter the grid to locate fields by name or category.

The field grid shows member names, offsets, fixed byte lengths, interpretation, and editable values. Choose a compatible interpretation to edit numbers, text, or exact hexadecimal bytes. Expert mode also provides a 16-column raw-byte grid for the selected member, so large arrays can be edited byte by byte. Unknown data remains editable without guessing its meaning.

The grids support rectangular selection, copy, and tab/newline-separated paste for spreadsheet workflows. Paste validates the whole edit before changing the document. Invalid widths, out-of-range numbers, and malformed bytes are rejected. Unsaved changes are tracked, and opening another file or closing the window prompts to save or discard them.

Saving uses a temporary file in the destination directory followed by replacement, rather than deleting the destination first. Bytes outside the explicitly edited member remain unchanged. No semantic repairs, automatic checksums, or game-validity guarantees are inferred for unknown data. Keep an independent original backup.

### Dedicated normal-mode editors

Normal mode now opens an editor dashboard, with **eight dedicated dialogs**, rather than a field grid. Expert mode is an additional workspace and can be toggled without losing the current document or undo history.

- **Coins:** separate inputs for Monocoins and casino coins.
- **Playtime:** total frames and an hours/minutes/seconds/frames converter at 60 frames per second.
- **Level and EXP:** level and total EXP inputs, plus the original level-to-EXP lookup for levels 1–99. Applying a level edit synchronizes the two stored level fields.
- **Flags:** a checkbox for the game-clear flag.
- **Map and position:** the original named map catalog and X/Y coordinates. The original nine restricted maps are excluded from normal map selection. Selecting a map updates both stored map IDs; editing only coordinates leaves both IDs unchanged.
- **Character statuses:** the original 16 named characters, each with Alive, Dead, and Unknown choices. Edits preserve reserved bits and the raw bits of unchanged characters.
- **Files and script:** archive names, WRD/script references, and current line.
- **Audio:** BGM index, music/sound references, and volume settings.

Each dialog has Apply and Cancel. Apply stages the changes as one undoable operation; the file is written only through Save or Save As. Cancel leaves the document unchanged. Unchanged text retains its original bytes and padding. The normal editors and expert grids use the same shared save document, so edits are visible immediately when switching between them. Expert mode retains access to all 144 declared members and arbitrary raw-byte edits.

The shared library also provides `GameCatalog` and `GuidedEdits` for these operations. The executable regression suite checks the catalogs against independently captured legacy data, synchronized edits, status-mask preservation, and playtime conversion boundaries. Windows Forms interaction still requires validation on Windows.

### Current runtime and file-drop support (.NET 10 LTS)

This section supersedes the earlier .NET 6 build/runtime requirements. All four projects now target **.NET 10 LTS**: `net10.0` for the shared library, CLI, and tests; `net10.0-windows` for the Windows Forms GUI. `global.json` selects SDK **10.0.403**, permits newer patches in the same SDK feature band, and excludes prerelease SDKs. Install that SDK to build; the Windows GUI requires the .NET 10 Desktop runtime to run.

```sh
dotnet --version
dotnet build V3SaveManager.sln -c Release -p:EnableWindowsTargeting=true
dotnet run --project V3SaveManager.Tests -c Release
```

To open a save in the GUI, drag **one existing `.dat` file** from File Explorer onto the window, including the editor dashboard or expert grids. Uppercase `.DAT` is accepted. Dropping a file uses the same opening workflow as Open, with Save / Discard / Cancel handling for unsaved changes. Invalid save contents report an error and retain the current document. Dropping multiple files, directories, or other extensions is rejected. Hovering a file over the window never loads or changes the save.

The guided editors remain the default view, and Expert mode remains available separately. Cross-compilation and automated tests can run on Linux; visual rendering and operating-system drag/drop interaction require Windows validation.

### SDK selection on developer machines

The minimum SDK is now **10.0.100**. `global.json` uses `latestFeature` to accept any installed stable .NET 10.0 SDK feature band, rather than requiring 10.0.403. Prerelease SDKs and other major/minor SDK versions are not selected. The cloud environment's installed SDK remains 10.0.403, but developer machines do not need that specific version.

If Visual Studio reports that no compatible SDK is installed, check `dotnet --list-sdks` and install the .NET 10 **SDK** (the runtime alone cannot build the solution). Use a Visual Studio version supporting .NET 10, then reload the solution after installing the SDK.
