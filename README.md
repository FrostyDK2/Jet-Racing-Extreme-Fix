# Jet Racing Extreme - Offline Fix & Store Unlocker

This project is an unofficial patch for **Jet Racing Extreme: The First Encounter**.

## The Story
I played the demo of this game when I was a kid and had completely forgotten about it. A long time later, when I remembered it and went looking to play the full version, I discovered that it was no longer available on official stores, the game had been abandoned, and the server was shut down.

Doing a bit of research, I found out it was still possible to buy a key on itch.io by accessing the old demo page. I bought the key with all the hope in the world, but when I tried to redeem it, the game didn't work! It would go to the menu screen and get stuck forever on **"Activation in progress"** since the authentication server was dead.

Since it's a game made in the Unity engine, I decided to get my hands dirty and try to make it work locally. I know probably no one else plays this nowadays or will even give the game a chance, but I think it's a really cool game. If someone in the future finds this project useful or wants to relive the nostalgia, I'll be very happy to have helped!

## What does this mod do?
Due to the death of the original server, the game broke in three different ways. This patch fixes all of them:
1. **DRM/Activation Bypass:** Removes the infinite "Activation in progress" loop and forces the game to recognize the version as fully activated.
2. **Explosive Engine Fix (OVERHEATED):** Without the server, the game failed to decrypt the car's physics data, resulting in absurd and infinite values for engine power and heat. This made the car explode as soon as you accelerated. The patch restores the multiplier values to 1.0f so that the original Unity physics work perfectly.
3. **Local Store:** Since the server processed transactions, the in-game store became unusable. The code was modified so that all buy and sell transactions are accepted locally, allowing you to buy any item without needing to be connected to anything.

## How to use (Release)
It is extremely simple, the folder structure is already set up for the game!
1. Download the JReX_Offline_Fix_Release.zip file from this repository and extract it.
2. Copy all the extracted files (the jrex_Data folder, etc.).
3. Paste them directly into the root folder where the game is installed (e.g., C:\Program Files (x86)\Steam\steamapps\common\JReX).
4. When Windows asks if you want to replace the files, click **Yes / Replace**.
5. Start the game and have fun!

## Open Source (How to compile the patcher)
If you want to see the modifications for yourself or apply the patch to a vanilla version of the DLL, I have provided the JReX_Patcher.cs script.
It uses the **Mono.Cecil** library to edit the game's DLL at runtime.

To run it:
csc.exe /r:Mono.Cecil.dll JReX_Patcher.cs
.\JReX_Patcher.exe Assembly-CSharp.dll Assembly-CSharp.patched.dll
