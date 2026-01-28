# Cấu Trúc Game - Narrative-Driven Adventure

## 🎯 Tổng Quan Kiến Trúc

```
┌─────────────────────────────────────────────────────┐
│         BOOTSTRAP_Main (DontDestroyOnLoad)         │
│  ├─ GameManager                                     │
│  ├─ AudioManager                                    │
│  ├─ SaveManager                                     │
│  └─ InputManager                                    │
└─────────────────────────────────────────────────────┘
                          ↓
┌─────────────────────────────────────────────────────┐
│           UI_Common (DontDestroyOnLoad)             │
│  ├─ HUD (HP, Inventory, etc.)                       │
│  ├─ Menu (Pause, Settings)                          │
│  └─ DialogueUI                                      │
└─────────────────────────────────────────────────────┘
                          ↓
┌─────────────────────────────────────────────────────┐
│         CHAP01_Environment (Scene Layer)            │
│  ├─ Terrain & Static Objects                        │
│  ├─ Buildings & Props                               │
│  └─ Waypoints & NavMesh                             │
└─────────────────────────────────────────────────────┘
                          ↓
┌─────────────────────────────────────────────────────┐
│          CHAP01_Gameplay (Scene Layer)              │
│  ├─ Player Spawn Point                              │
│  ├─ Enemies & NPCs                                  │
│  ├─ Interactables                                   │
│  └─ Puzzles & Events                                │
└─────────────────────────────────────────────────────┘
                          ↓
┌─────────────────────────────────────────────────────┐
│          CHAP01_Lighting (Scene Layer)              │
│  ├─ Directional Light                               │
│  ├─ Point Lights                                    │
│  ├─ Post Processing                                 │
│  └─ Fog & Effects                                   │
└─────────────────────────────────────────────────────┘
```

## 📁 Cấu Trúc Thư Mục Scenes

```
Assets/Scenes/
├── Bootstrap/
│   └── BOOTSTRAP_Main.unity
├── UI/
│   └── UI_Common.unity
└── Chapters/
    ├── Chap01/
    │   ├── Chap01_Environment.unity
    │   ├── Chap01_Gameplay.unity
    │   └── Chap01_Lighting.unity
    ├── Chap02/
    │   ├── Chap02_Environment.unity
    │   ├── Chap02_Gameplay.unity
    │   └── Chap02_Lighting.unity
    └── ...
```

## ✅ Lợi Ích Của Cấu Trúc Này

| Lợi Ích | Chi Tiết |
|---------|----------|
| **Giảm Conflict** | Mỗi layer riêng biệt → dễ merge khi làm team |
| **Tối Ưu Hóa** | Load environment trước → gameplay → lighting |
| **Tái Sử Dụng** | Common managers được dùng cho tất cả chapter |
| **Dễ Bảo Trì** | Thay đổi 1 chapter không ảnh hưởng chapter khác |
| **Performance** | Unload scene cũ khi chuyển chapter |
| **Collaborate** | Texture artist làm Environment, Level designer làm Gameplay |

## 🔧 Workflow Loading Scene

```
1. Application Start
   ↓
2. Load BOOTSTRAP_Main (DontDestroyOnLoad)
   • Khởi tạo GameManager, AudioManager, SaveManager
   ↓
3. Load UI_Common (DontDestroyOnLoad)
   • Hiển thị Main Menu hoặc HUD
   ↓
4. Load Chapter Scenes (Additive)
   • Load Chap01_Environment (base layer)
   • Load Chap01_Gameplay (gameplay logic)
   • Load Chap01_Lighting (visual effects)
   ↓
5. Player Tương Tác
   • Chơi game, giải puzzle, quests
   ↓
6. Kết Thúc Chapter → Load Chapter Tiếp Theo
   • Unload Chap01 (all 3 scenes)
   • Load Chap02 (environment, gameplay, lighting)
```

## 🎬 Setup Mỗi Layer Scene

### CHAP01_Environment
```
Hierarchy:
- TerrainParent
  ├─ Terrain
  ├─ Trees (LOD Groups)
  └─ Rocks
- BuildingsParent
  ├─ House_01
  ├─ House_02
  └─ Bridge
- WayPoints
  ├─ Waypoint_01
  ├─ Waypoint_02
  └─ NavMeshes (baked)
- Tags: "Environment" (để dễ unload)
```

### CHAP01_Gameplay
```
Hierarchy:
- SpawnPoints
  ├─ PlayerSpawn
  └─ EnemySpawns
- Enemies
  ├─ Enemy_01 (Prefab instance)
  └─ Enemy_02
- NPCs
  ├─ NPC_Merchant
  └─ NPC_Guard
- Interactables
  ├─ Chest
  ├─ Door
  └─ Lever
- Puzzles
  ├─ PuzzleManager
  └─ PuzzleElements
- Events
  ├─ EventTrigger_01
  └─ EventTrigger_02
```

### CHAP01_Lighting
```
Hierarchy:
- Lights
  ├─ DirectionalLight (Sun)
  ├─ PointLights (Torches)
  └─ SpotLights
- PostProcessing
  ├─ Volume (Bloom, AO, etc.)
  └─ Fog Settings
- Effects
  ├─ Particle Systems
  └─ Weather Effects
```

## 💾 Manager Persistence

Các manager này phải có **DontDestroyOnLoad**:

```csharp
void Awake()
{
    if (Instance == null)
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    else
    {
        Destroy(gameObject);
    }
}
```

**Managers cần tạo:**
- `GameManager` - Quản lý game state, chapters
- `AudioManager` - BGM, SFX
- `SaveManager` - Save/Load game
- `InputManager` - Input handling
- `UIManager` - Common UI elements

## 🎯 Khi Chuyển Chapter

```csharp
// Trong SceneManager hoặc ChapterManager
async Task LoadChapter(int chapterNumber)
{
    // Unload chapter cũ
    await UnloadAllChapters();
    
    // Load scenes mới (additive)
    string chap = $"Chap{chapterNumber:D2}";
    await LoadSceneAdditive($"Chap{chapterNumber:D2}_Environment");
    await LoadSceneAdditive($"Chap{chapterNumber:D2}_Gameplay");
    await LoadSceneAdditive($"Chap{chapterNumber:D2}_Lighting");
    
    // Trigger events
    GameManager.OnChapterLoaded?.Invoke(chapterNumber);
}
```

## 📝 Checklist Mỗi Chapter

- [ ] Environment scene hoàn thành + NavMesh baked
- [ ] Gameplay scene có spawn points & interactables
- [ ] Lighting scene optimize & post-processing setup
- [ ] Tất cả 3 scenes added vào Build Settings
- [ ] SceneManager có hỗ trợ loading chapter này
- [ ] Test loading/unloading không có lỗi

## 🔗 Kế Tiếp

1. Xem `SceneManager.cs` để hiểu cách load scenes
2. Setup `BOOTSTRAP_Main` scene với managers
3. Tạo `UI_Common` scene với HUD & menus
4. Dùng `ChapterManager.cs` để control chapter flow
