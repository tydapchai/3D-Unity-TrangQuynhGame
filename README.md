# 📚 Game Development Documentation Index

## 🎮 Project Overview

**Tên Game:** Trạng Quỳnh (3D Narrative-Driven Adventure)

**Cấu Trúc:**
- **Bootstrap** scene: Managers (Game, Scene, Audio, Save)
- **UI_Common** scene: Shared HUD, menus, dialogue
- **Chapter Scenes**: Additive loading (Environment, Gameplay, Lighting)
- **Support**: 5 chapters with full story progression

---

## 📖 Documentation Files

### 1. **QUICK_START.md** ⭐ START HERE
   - 5 bước nhanh để bắt đầu
   - Shortcuts để test
   - Checklist
   - **Đọc file này trước tiên!**

### 2. **GAME_STRUCTURE.md**
   - Kiến trúc tổng quát
   - Lợi ích cấu trúc
   - Workflow loading
   - Manager persistence

### 3. **SETUP_BOOTSTRAP_UI.md**
   - Chi tiết tạo BOOTSTRAP_Main scene
   - Chi tiết tạo UI_Common scene
   - Build Settings setup
   - Test loading

### 4. **SETUP_CHAPTERS.md**
   - Cách tạo chapter scenes
   - Environment layer
   - Gameplay layer
   - Lighting layer
   - Ví dụ prefabs (Enemy, NPC, Interactable)

---

## 🛠️ Scripts Available

### Managers (Assets/Scripts/Managers/)

**GameManager.cs**
- Quản lý chapters hiện tại
- Toggle pause
- Load/restart chapter
- Events: OnChapterLoaded, OnGamePaused, OnGameResumed

**SceneLoadManager.cs**
- Load/unload scenes additive
- 3-scene chapter loading
- Player spawn setup
- IChapterSetup interface

**AudioManager.cs**
- BGM playback + fade
- SFX one-shot
- Volume control
- Persistent across scenes

**SaveManager.cs**
- Save/load game state
- Track completed chapters
- Player progress
- JSON serialization

### Gameplay (Assets/Scripts/Gameplay/)

**Enemy.cs**
- Basic AI (chase, attack)
- Health system
- Die on 0 health

**NPC.cs** + Merchant, Guard
- Dialogue system
- Quest tracking
- Merchant shop
- Guard patrol

**Interactable.cs** + Chest, Door, Lever
- Base interaction class
- Chest (items drop)
- Door (locked/unlocked)
- Lever (trigger events)

### Player (Assets/Scripts/Player/)

**Player.cs**
- Movement (WASD)
- Jump (Space)
- Ground detection

### Utils (Assets/Scripts/Utils/)

**SceneDebugger.cs**
- Keyboard shortcuts for testing
- Load/restart chapters
- Save/load game
- Debug info

---

## 🎮 Keyboard Shortcuts (During Play)

| Key | Action | Purpose |
|-----|--------|---------|
| 1-5 | Load Chapter 1-5 | Test chapter loading |
| N | Next Chapter | Progress story |
| R | Restart Chapter | Test restart |
| ESC | Pause/Resume | Test pause system |
| S | Save Game | Test save |
| L | Load Game | Test load |
| D | Debug Info | Show current state |

---

## 📁 Folder Structure

```
Assets/
├── Scenes/
│   ├── Bootstrap/
│   │   └── BOOTSTRAP_Main.unity
│   ├── UI/
│   │   └── UI_Common.unity
│   └── Chapters/
│       ├── Chap01/
│       │   ├── Chap01_Environment.unity
│       │   ├── Chap01_Gameplay.unity
│       │   └── Chap01_Lighting.unity
│       └── Chap02/ ... Chap05/
├── Scripts/
│   ├── Managers/
│   │   ├── GameManager.cs
│   │   ├── SceneLoadManager.cs
│   │   ├── AudioManager.cs
│   │   └── SaveManager.cs
│   ├── Gameplay/
│   │   ├── Enemy.cs
│   │   ├── NPC.cs
│   │   └── Interactable.cs
│   ├── Player/
│   │   └── Player.cs
│   └── Utils/
│       └── SceneDebugger.cs
├── Prefabs/
│   ├── Managers/
│   │   └── ManagersBundle.prefab
│   ├── Enemies/
│   │   ├── Enemy_Goblin.prefab
│   │   └── Enemy_Orc.prefab
│   ├── NPCs/
│   │   ├── NPC_Merchant.prefab
│   │   └── NPC_Guard.prefab
│   └── Interactables/
│       ├── Chest.prefab
│       ├── Door.prefab
│       └── Lever.prefab
├── Resources/
│   ├── Audio/
│   │   ├── BGM/
│   │   │   ├── Chap01_BGM.mp3
│   │   │   └── Chap02_BGM.mp3
│   │   └── SFX/
│   │       ├── UI_Click.mp3
│   │       └── Damage.mp3
│   └── Addressables/
├── Art/
├── Materials/
├── Animations/
└── UI/
```

---

## 🚀 Development Roadmap

### Phase 1: Setup (Week 1)
- [x] Create folder structure
- [ ] Create BOOTSTRAP_Main scene
- [ ] Create UI_Common scene
- [ ] Setup managers
- [ ] Test shortcuts

### Phase 2: Chapter 01 (Week 2-3)
- [ ] Create Environment scene
  - Terrain, props, buildings
  - NavMesh baked
- [ ] Create Gameplay scene
  - Player spawn, enemies, NPCs
  - Interactables, puzzles
- [ ] Create Lighting scene
  - Main light, point lights
  - Post-processing, effects

### Phase 3: Story (Week 4-5)
- [ ] Write dialogue/quests
- [ ] Create cutscenes
- [ ] Test chapter flow
- [ ] Implement save/load

### Phase 4: Chapters 2-5 (Week 6-8)
- [ ] Copy Chapter 01 template
- [ ] Customize each chapter
- [ ] Implement chapter-specific mechanics
- [ ] Polish & balance

### Phase 5: Polish (Week 9-10)
- [ ] Optimize performance
- [ ] Fix bugs
- [ ] Add sounds/music
- [ ] Final testing

---

## 🎯 Key Concepts

### DontDestroyOnLoad
**Quan trọng nhất!** Managers phải persist across scenes:
```csharp
void Awake() {
    if (Instance == null) {
        DontDestroyOnLoad(gameObject);
    }
}
```

### Additive Scene Loading
Chapter được load thêm vào, không replace:
```csharp
SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
```

### IChapterSetup Interface
Objects implement này sẽ được setup khi chapter load:
```csharp
public class ChapGameplay : IChapterSetup {
    public void OnChapterSetup(int chapterNumber) { ... }
}
```

### Save Data Format
SaveManager tự động JSON serialize:
```csharp
[System.Serializable]
public class SaveData {
    public int currentChapter;
    public int playedTime;
    public bool[] completedChapters;
}
```

---

## 🐛 Troubleshooting

### Problem: Manager not persisting
**Solution:** Add DontDestroyOnLoad, check singleton pattern

### Problem: Scene not loading
**Solution:** Add scene to Build Settings, check scene name spelling

### Problem: Player spawning at wrong location
**Solution:** Create PlayerSpawn GameObject, tag it "PlayerSpawn"

### Problem: Memory leak when loading chapters
**Solution:** Properly unload old chapter scenes before loading new ones

### Problem: Audio not playing
**Solution:** Check AudioSource volume, check audio clip path in Resources/

---

## 📞 Getting Help

If you encounter issues:

1. Check the **QUICK_START.md** checklist
2. Look at **SETUP_BOOTSTRAP_UI.md** for detailed steps
3. Review **SETUP_CHAPTERS.md** for chapter setup
4. Check script comments for usage examples
5. Use **SceneDebugger.cs** to test functionality

---

## 🎓 Learning Resources

**Within this project:**
- Read QUICK_START.md for overview
- Check script comments for implementation details
- Study SETUP_*.md files for best practices

**External Resources:**
- [Unity Documentation](https://docs.unity3d.com)
- [Scene Management](https://docs.unity3d.com/ScriptReference/SceneManagement.html)
- [Game State Management](https://learn.unity.com)

---

## 📊 Project Stats

| Metric | Value |
|--------|-------|
| Scenes | 8 (Bootstrap + UI + 3×Chapter×2) |
| Scripts | 10+ managers, gameplay, utils |
| Managers | 4 (Game, Scene, Audio, Save) |
| Max Chapters | 5 |
| Save System | JSON-based |

---

## ✅ Quality Checklist

Before submitting each chapter:

- [ ] All 3 scenes load without errors
- [ ] Player spawns at correct position
- [ ] Enemies AI works
- [ ] NPCs have dialogue
- [ ] Interactables function
- [ ] Lights baked properly
- [ ] Post-processing looks good
- [ ] BGM plays
- [ ] No memory leaks
- [ ] Save/load works

---

**Last Updated:** January 28, 2026
**Version:** 1.0
**Status:** Ready for development ✅
