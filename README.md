# Qube2 Trainer

This is a trainer meant for speedrunners of Q.U.B.E. 2 to for example practice sections more easily, get to spots faster or glitch-hunt more efficiently.
But it can of course be used by anyone to simply explore the game more with the cheats such as Moonjump, Fly Mode and position customizer. 

![screenshot of trainer](https://github.com/daniel-noordzij/Qube2-Trainer/blob/2.0/Screenshot1.jpg)
![screenshot of trainer](https://github.com/daniel-noordzij/Qube2-Trainer/blob/2.0/Screenshot2.jpg)

### How does it work?

This program hooks into the memory of Qube 2 at specific offsets to get values like the current position and speed. These addresses and offsets were searched for using [Cheat Engine](cheatengine.org). Having found the base address and offset of the Y-Position, it was just a matter of messing around with the offsets to get more interesting values that can be modified and read. We already found more of those values than implemented in this trainer, the Cheat Engine file of all those values can be downloaded [here](https://cdn.discordapp.com/attachments/425395827893075973/685243872506675264/QUBE-All-Values-109.CT).


## Prerequisites

### Running the trainer

To run the trainer, you just have to be on Windows and have the newest version of Q.U.B.E. 2 installed. This is currently the version 1.0.9 (as displayed in the home menu of QUBE, date of writing: 21/05/2022)

### Compiling and modifying the source code

This program was made using Microsoft Visual Studio (not Code), together with the the Visual C#-pack.


## Running

You can either start Q.U.B.E. 2 or the trainer first, the order is not important in the first part. Wait for the main menu in QUBE to appear, then press "Connect" in the trainer to hook into QUBE's memory. If an error appears saying "Could not find an open QUBE 2 process!", try restarting your game and then press "Connect" again.
When connected in the home menu, all values should display a 0. When entering a level, the values should automatically set to the right values. If it doesn't then give it a minute to sync, otherwise try going to a different chapter or restarting the game.

### Hotkeys

These are the default hotkeys, but are customizable including a bunch of other actions.

Key | Action
--- | ------
F1  | Toggle Moon Jump
F2  | Toggle Super Speed
F3  | Toggle Low Gravity
F4  | Store current position
F5  | Restore saved position
F6  | Teleport to the marker
F7  | Lock the X coordinate
F8  | Lock the Y coordinate
F9  | Lock the Z coordinate
F10 | Toggle Fly Mode
F11 | RESERVED for fullscreen toggle
F12 | Toggle arm visibility

When Super Speed or Fly Mode is enabled, QUBE's in-game WASD-controls are disabled and the trainer controls the position instead. For Fly Mode, Space is used to go up and Left Shift to go down.
