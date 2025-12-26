# The 5 Fold Awakening - Unity Version

A third-person adventure game blending magic and technology in a mystical world.

## Overview

This Unity project is a complete rewrite of the original JavaScript/Three.js version, featuring:
- Third-person perspective with smooth camera controls
- Enhanced character rendering and interactions
- Immersive story-driven gameplay
- Modern Unity architecture with C# scripting

## Project Structure

```
Assets/
├── Scripts/
│   ├── PlayerController.cs      # Third-person player movement and camera
│   ├── CharacterManager.cs      # NPC interactions and dialogue system
│   ├── StorySystem.cs          # Story progression and objectives
│   ├── GameManager.cs          # Overall game state and settings
│   ├── SceneSetup.cs           # Scene initialization and world setup
│   ├── AudioManager.cs         # Audio management and playback system
│   ├── TestSceneSetup.cs       # Automatic test scene creation
│   ├── GameTester.cs           # Runtime testing and validation
│   └── AudioSetupHelper.cs     # Helper script for testing audio setup
├── Audio/
│   ├── Music/                  # Background music tracks
│   ├── SFX/                    # Sound effects
│   └── Voices/                 # Character and narrator voice clips
```

## Key Features

### PlayerController
- Smooth third-person movement
- Dynamic camera that follows the player
- Character rotation based on movement direction
- Gravity and ground detection

### CharacterManager
- Interactive NPCs with dialogue systems
- Proximity-based interaction (press E to talk)
- Sequential dialogue progression
- UI-based dialogue display

### StorySystem
- Chapter-based story progression
- Objective tracking system
- Story panel with content display
- Tab key to toggle story view

### GameManager
- Main menu and pause functionality
- Settings management (audio, particles, sensitivity)
- Game state handling
- Persistent settings with PlayerPrefs

### SceneSetup
- Procedural scene initialization
- Player, environment, and character spawning
- Lighting configuration
- Camera positioning

### AudioManager
- Dynamic music system that changes based on game state
- Voice acting for characters and narrator
- Sound effects for interactions and story progression
- Volume controls for different audio types
- Smooth music transitions with fade effects

### TestSceneSetup
- Automatic creation of test scene with all necessary components
- Basic UI panels for dialogue, story, and menus
- Sample environment with interactable objects
- Ready-to-play test environment

### GameTester
- Comprehensive runtime testing system
- Component validation and error checking
- Interactive test methods for all game systems
- Debug logging for troubleshooting

## Quick Start Testing

### Automatic Test Scene
1. **Create New Scene**: File → New Scene
2. **Add TestSceneSetup**: Create empty GameObject → Add Component → TestSceneSetup
3. **Run**: Press Play - the system will automatically create a complete test environment
4. **Test Features**:
   - **Movement**: WASD to move, mouse to look
   - **Interaction**: Move near Lyra and press E to talk
   - **Story**: Press Tab to view story and objectives
   - **Menu**: Click "START GAME" to begin

### Manual Testing
1. **Add GameTester Component**: To any GameObject in your scene
2. **Run Tests**: The system will automatically validate all components on start
3. **Manual Tests**: Use the public methods to test specific features:
   - `TestMovement()` - Test player controls
   - `TestInteraction()` - Test character dialogue
   - `TestStory()` - Test story system
   - `TestAudio()` - Test audio playback

1. **Open in Unity**: Open this project folder in Unity (recommended version: 2021.3+)
2. **Create Scene**: Create a new scene and add the SceneSetup script to an empty GameObject
3. **Configure Components**:
   - Assign prefabs in the SceneSetup inspector
   - Set up UI panels in GameManager
   - Configure character dialogues in CharacterManager
   - Add story content in StorySystem
4. **Add UI Elements**:
   - Create Canvas with dialogue panel, story panel, settings panel
   - Add buttons, toggles, and sliders as referenced in scripts
4. **Setup Audio System**:
   - Create an AudioManager GameObject and attach the AudioManager script
   - Assign audio clips to the AudioManager inspector:
     - Music: Add adventure, peaceful, and menu music tracks
     - Voices: Add character voice clips and narrator voice clips
     - SFX: Add interaction, objective complete, and story advance sounds
   - (Optional) Add AudioSetupHelper script to test audio without full implementation
   - Update UI settings panel to include volume sliders for master, music, SFX, and voice
5. **Test Audio**: Run the scene and verify music transitions, voice playback, and sound effects work correctly

## Story Content

The game follows Kael Ardin, a young mage discovering his powers in a world where magic and technology coexist. Key characters include:
- **Lyra Vale**: Mysterious mentor figure
- **Eldrin**: Ancient guardian spirit
- **Various NPCs**: Townsfolk and magical beings

## Technical Notes

- Uses Unity's CharacterController for movement
- Implements event-driven dialogue system
- Persistent settings with PlayerPrefs
- Modular script architecture for easy expansion

## Audio Features

### Dynamic Music System
- **Adventure Music**: Plays during missions and high-stakes moments
- **Peaceful Music**: Plays during exploration and regular gameplay
- **Menu Music**: Plays in main menu and settings
- Smooth crossfading between different music states

### Voice Acting
- **Character Voices**: Each NPC has unique voice clips for their dialogues
- **Narrator Voice**: Story segments are narrated with dedicated voice clips
- Voice playback is synchronized with text display

### Sound Effects
- Interaction sounds when talking to characters
- Objective completion sounds
- Story advancement sounds
- All SFX are configurable and can be toggled

### Audio Settings
- Master volume control
- Individual volume controls for music, SFX, and voices
- Settings are saved and persist between sessions
- Audio can be completely disabled if desired

## Future Enhancements

- Advanced AI for NPC behaviors
- Inventory and item system
- Combat mechanics
- Multiple levels and environments
- Audio integration
- Particle effects for magic

## Original JavaScript Version

This Unity version is based on the original JavaScript implementation found in the parent directory. The core story, characters, and gameplay concepts remain the same, but with enhanced visuals and third-person gameplay.