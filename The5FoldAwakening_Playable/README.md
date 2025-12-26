# 🎮 The 5 Fold Awakening - Complete Playable Version

A fully self-contained, third-person adventure game blending magic and technology. This version combines all game systems into a single, comprehensive Unity project that's immediately playable.

## 🚀 Quick Start

### 1. Open in Unity
- Open Unity Hub
- Click "Add" → "Add project from disk"
- Select the `The5FoldAwakening_Playable` folder
- Open the project

### 2. Run the Game
- Open the `Assets/Scenes/MainScene` (or create a new scene)
- Add the `The5FoldAwakening` script to an empty GameObject
- Press Play!

### 3. Test Features
- **WASD**: Move character
- **Mouse**: Look around (third-person camera)
- **E**: Interact with characters
- **Tab**: Toggle story panel
- **Esc**: Pause game

## 📁 Project Structure

```
The5FoldAwakening_Playable/
├── Assets/
│   ├── Scripts/           # All game logic in C#
│   │   ├── The5FoldAwakening.cs    # Main game controller
│   │   ├── [Other scripts...]      # Individual system scripts
│   ├── Audio/             # Audio asset folders
│   │   ├── Music/         # Background music
│   │   ├── SFX/           # Sound effects
│   │   └── Voices/        # Character voices
│   ├── Prefabs/           # Reusable game objects
│   └── Scenes/            # Unity scenes
├── README.md              # This file
└── [Unity project files]
```

## 🎯 Game Features

### Core Gameplay
- **Third-Person Movement**: Smooth character controls with camera follow
- **Interactive World**: Characters, objects, and environments to explore
- **Story-Driven**: Chapter-based narrative with objectives
- **Dynamic Audio**: Music and sound effects that respond to gameplay

### Character System
- **NPC Interactions**: Talk to characters with dialogue trees
- **Voice Acting**: Character voices synchronized with text
- **Multiple Characters**: Lyra Vale, Eldrin, and more

### Audio System
- **Dynamic Music**: Peaceful exploration → Adventurous missions
- **Sound Effects**: Interactions, objectives, story progression
- **Voice Acting**: Character and narrator dialogue
- **Volume Controls**: Master, music, SFX, and voice controls

### UI System
- **Main Menu**: Start game, settings, quit
- **Dialogue Panels**: Character conversations
- **Story Display**: Objectives and narrative
- **Settings Menu**: Audio, graphics, controls

## 🔧 Setup Instructions

### Basic Setup
1. **Create New Scene**: File → New Scene
2. **Add GameObject**: GameObject → Create Empty
3. **Add Script**: Add Component → The5FoldAwakening
4. **Configure Audio** (Optional):
   - Drag music clips to the Audio Clips section
   - Add voice clips for characters and narrator
   - Add SFX clips for interactions

### Advanced Setup
1. **UI Setup**: The script auto-creates basic UI, or assign your own panels
2. **Character Setup**: Add character GameObjects and configure dialogues
3. **Environment**: Add terrain, buildings, and interactive objects
4. **Testing**: Use the built-in test methods to verify functionality

## 🎮 Controls

| Key | Action |
|-----|--------|
| WASD | Move character |
| Mouse | Look around / Camera control |
| E | Interact with objects/characters |
| Tab | Toggle story panel |
| Esc | Pause / Resume game |
| F11 | Toggle fullscreen (in build) |

## 🔊 Audio Configuration

### Music Tracks
- **Menu Music**: Atmospheric music for menus
- **Peaceful Music**: Calm exploration music
- **Adventure Music**: High-energy mission music

### Voice Clips
- **Character Voices**: One per dialogue line
- **Narrator Voices**: One per story segment

### Sound Effects
- **Interaction**: When talking to characters
- **Objective Complete**: When finishing objectives
- **Story Advance**: When progressing story

## 🐛 Debugging & Testing

### Built-in Debug Tools
```csharp
// Test all systems
FindObjectOfType<The5FoldAwakening>().TestAllSystems();

// Test individual components
FindObjectOfType<The5FoldAwakening>().DebugLog("Custom message");
```

### Common Issues & Fixes

#### Game Won't Start
- **Issue**: No main camera or player
- **Fix**: Ensure scene has Camera tagged "Main Camera"

#### No Audio
- **Issue**: Audio clips not assigned
- **Fix**: Drag audio files to script inspector

#### No UI
- **Issue**: UI panels not created
- **Fix**: Script auto-creates UI, or assign manually

#### Character Not Responding
- **Issue**: Character objects not configured
- **Fix**: Ensure characters array is populated in inspector

## 🎨 Customization

### Adding New Characters
1. Create character GameObject
2. Add to `characters` array in inspector
3. Configure name, dialogues, and interaction settings

### Adding New Story Segments
1. Expand `storySegments` array in inspector
2. Add title, content, and objectives
3. Voice clips will auto-map by index

### Custom Audio
1. Add audio clips to `Assets/Audio/` folders
2. Drag to appropriate slots in inspector
3. Adjust volume levels as needed

## 📦 Building the Game

### For Windows
1. File → Build Settings
2. Select "PC, Mac & Linux Standalone"
3. Target Platform: Windows
4. Click "Build"
5. Choose output folder

### For WebGL
1. File → Build Settings
2. Select "WebGL"
3. Click "Build"
4. Upload to web server

## 🔄 Version History

- **v1.0**: Complete playable version with all core systems
- **Audio System**: Dynamic music, voice acting, sound effects
- **UI System**: Complete menu, dialogue, and story interfaces
- **Character System**: Interactive NPCs with dialogue trees
- **Story System**: Chapter-based narrative with objectives

## 🎯 Future Enhancements

- [ ] Advanced AI for NPC behaviors
- [ ] Inventory and item system
- [ ] Combat mechanics
- [ ] Multiple levels and environments
- [ ] Particle effects for magic
- [ ] Save/load game system

## 📞 Support

If you encounter issues:
1. Check the Unity Console for error messages
2. Use the built-in test methods
3. Verify all required components are assigned
4. Check that audio clips are in the correct format

---

**Ready to play? Press Play and begin your journey in "The 5 Fold Awakening"!** 🚀