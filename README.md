# 2D Fighter Game - Unity Project

## Project Overview
A local multiplayer 2D fighting game built in Unity where two medieval themed fighter battle it out till their health reaches exhaust or victory.

## Features
- **Two Playable Characters**: Soldier and Orc with unique animations
- **Local Multiplayer**: Simultaneous two-player gameplay
- **Health System**: Dynamic health bars with color-coded status
- **Combat Mechanics**: Attack system with hit detection
- **Victory Conditions**: Game over screen with winner announcement
- **Restart Functionality**: Quick game reset after matches

## Characters & Controls

### Soldier
- **Move Left**: A
- **Move Right**: D  
- **Jump**: W
- **Attack**: Space

### Orc
- **Move Left**: Left Arrow
- **Move Right**: Right Arrow
- **Jump**: Up Arrow
- **Attack**: Down Arrow

## Technical Implementation

### Core Systems

#### PlayerController.cs
- Handles character movement, jumping, and attacking
- Manages health system and damage taking
- Controls animation states and character flipping
- Implements ground detection and collision handling

#### GameManager.cs  
- Manages game state and victory conditions
- Implements singleton pattern for global access
- Handles scene management and restart functionality
- Controls UI elements and victory screen

### Key Components
- **Rigidbody2D**: Physics-based movement
- **Animator**: Character animations and state transitions
- **BoxCollider2D**: Collision detection
- **UI Slider**: Health bar visualization
- **LayerMask**: Selective collision detection for attacks

## Setup Instructions

### Prerequisites
- Unity 2022.3 LTS or later
- Basic knowledge of Unity Editor

### Installation
1. Create new Unity 2D project
2. Import provided script files into Assets/Scripts/
3. Set up two character GameObjects with:
   - SpriteRenderer
   - Rigidbody2D
   - BoxCollider2D  
   - Animator with controller
4. Assign PlayerController script to both characters
5. Configure input keys in Inspector
6. Create GameManager GameObject and assign references
7. Set up UI Canvas with health bars and victory screen

### Scene Structure

Assets/
|───Scripts/
|          └──PlayerController.cs
|          └──GameManager.cs
|───Animations/
|	  └──Soldier/
|	  └──Orc/
|───Prefabs/
|───Scenes/ 
|______Main.unity

---
*Project developed as a demonstration of Unity 2D game development skills*


