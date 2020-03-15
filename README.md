# Qube2 Trainer

This trainer is meant to help Speedrunners practice their speed in sections by replaying them using the teleport function, but can be used by everyone who wants to mess around and look into the Out of Bounds in Q.U.B.E. 2.

![screenshot of trainer](https://github.com/MonsterDruide1/Qube2-Trainer/Screenshot.jpg)

### How does it work?

This program hooks into the memory of Qube 2 at specific offsets to get values like the current position and speed. These addresses and offsets were searched for using [Cheat Engine](cheatengine.org). Having found the base address and offset of the Y-Position, it was just a matter of messing around with the offsets to get more interesting values that can be modified and read. We already found more of those values than implemented in this trainer, the Cheat Engine file of all those values can be downloaded [here](https://cdn.discordapp.com/attachments/425395827893075973/685243872506675264/QUBE-All-Values-109.CT).


## Prerequisites

### Running the trainer

To run the trainer, you just have to be on Windows and have the newest version of Q.U.B.E. 2 installed. This is currently the version 1.0.9 (as displayed in the home menu of Qube, Date of writing: 15.03.2020)

### Compiling and modifying the source code

This program was made using Microsoft Visual Studio (not Code), together with the the Visual C#-pack.


## Running

You can either start Q.U.B.E. 2 or the trainer first, the order is not important in the first part. Wait for the main menu in Qube to appear, then press "Connect" in the trainer to hook into Qube's memory. If an error appears saying "Could not find an open QUBE 2 process!", try restarting your game and then press "Connect" again.
When connected in the home menu, all values should display a 0. When entering a level, the values should automatically set to the right values.

### Hotkeys

Key | Action
--- | ------
F1  | toggle MoonJump
F2  | toggle SuperSpeed
F3  | toggle LowGravity
F4  | store current position
F5  | restore saved position
F6  | teleport to the marker
F7  | lock the X coordinate
F8  | lock the Y coordinate
F9  | lock the Z coordinate
F10 | toggle FlyMode
F11 | RESERVED for Qube's fullscreen toggle
F12 | toggle arms visibility

When SuperSpeed or FlyMode are enabled, the Qube's ingame WASD-controls are disabled and the only the trainer controls the position using the same controls. For FlyMode, LShift (the left shift button) is used for getting down and space for flying upwards.