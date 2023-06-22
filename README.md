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