# TAM - 2D Platformer Game Documentation

## Presentation File For The Game: https://docs.google.com/presentation/d/1-OmbO7h5lfAwxp699MOJ1wx5m1_1lz6UeceoMFlxkhg/edit?usp=sharing

## GamePlay Video: https://www.youtube.com/watch?v=2o161uFWWC4    
	Blurry Video Due to Screen Recorder  

## Overview

This 2D platformer game features pixelated graphics and places the player in a vibrant, dynamic fantasy world. The primary character is a melee fighter, with plans to introduce ranged and tank characters in future updates. The game involves navigating through levels filled with platforms, enemies, and various obstacles. The gameplay experience is enhanced by a parallax background that creates a sense of depth and a moving camera that tracks the player's movement.

## Game Mechanics

- **2D Platformer**: The game is a side-scrolling platformer where the player controls a character to jump between platforms, avoid obstacles, and defeat enemies.
- **Character Types**: The primary character is a melee fighter. Enemies include both melee orcs and ranged orcs who use magic projectiles.
- **Parallax Background**: The background consists of multiple layers that move at different speeds, creating a parallax effect that adds visual depth to the game.
- **Moving Camera**: The camera follows the player's movement, ensuring the player is always centered in the view and enhancing the immersive experience.
- **Platform Movement**: Platforms in the game move left and right, requiring the player to time their jumps and movements precisely.
- **Health Management**: The `statHandler` script is used to manage the health of all characters, ensuring consistent health mechanics across the game.
- **Projectile Attacks**: Enemy characters use the `ProjectileSpawner` script to launch magic projectiles at the player, adding an additional layer of challenge.

## Major Scripts

- **CharacterProfile**
  - **Purpose**: Defines the attributes of characters, including speed, health, jump force, and attack range. These attributes are stored in a `ScriptableObject` for easy configuration and reuse.
  - **Usage**: This script is attached to the player and enemy characters to set their stats.

- **PlatformMover**
  - **Purpose**: Controls the movement of platforms, making them move left and right.
  - **Details**: Includes speed, delay, and distance parameters to define how the platform moves.

- **ProjectileSpawner**
  - **Purpose**: Spawns projectiles from enemies during specific animations.
  - **Details**: Monitors animation states and spawns projectiles towards the player, adding a ranged attack mechanic to the game.

- **StatHandler**
  - **Purpose**: Manages health and damage for all characters.
  - **Details**: Initializes health from the `CharacterProfile`, handles taking damage, healing, and character death. Provides feedback via animations and sprite color changes.

- **MagicProjectile**
  - **Purpose**: Controls the behavior of magic projectiles launched by enemies.
  - **Details**: Handles projectile movement, collision detection, and damage application. Ensures projectiles move in the correct direction and disappear after hitting a target or traveling a certain distance.

- **ParallaxLayer**
  - **Purpose**: Implements the parallax effect for background layers.
  - **Details**: Moves background layers based on the camera's movement to create a sense of depth.

- **ParallaxCamera**
  - **Purpose**: Tracks camera movement and updates parallax layers.
  - **Details**: Detects camera movement and notifies parallax layers to adjust their positions accordingly.

- **ParallaxBackground**
  - **Purpose**: Manages multiple parallax layers.
  - **Details**: Collects and updates all parallax layers, ensuring they move correctly in relation to the camera's movement.

## Singleton Objects

- **LevelManager**: Manages level transitions and game state. Utilizes the Singleton pattern to ensure only one instance exists and persists across scenes.
- **SoundManager**: Manages all game sounds and music. Uses the Singleton pattern to provide consistent sound management throughout the game.

## Scriptable Objects

- **Player Stats**: `ScriptableObject` is used to store player and enemy stats, such as speed, health, jump force, and attack range. This allows for easy adjustment and reuse of these attributes across multiple characters and levels.

## Credits

**Development Team**
- Made by Biraj Tiwari and Bishal Tiwari
- LinkedIn: https://www.linkedin.com/in/birajtiwari/ https://www.linkedin.com/in/bishaltwr/  

**Assets**
- Graphics:
  - Icons and graphics from Flaticon
    - https://www.flaticon.com/
  - Sprites and additional graphics from CraftPix.net
  - Some assets from Unity Store

**Tools and Software**
- Game Engine: Unity
- Programming Language: C#
- Graphics Editor: GIMP
- Code Editor: Visual Studio, Visual Studio Code
  - https://craftpix.net/
  - https://assetstore.unity.com/

