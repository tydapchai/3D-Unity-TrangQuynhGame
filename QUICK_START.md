# 🚀 Quick Start Guide - Tạo Game Narrative-Driven

## 📋 Tóm Tắt Cấu Trúc

**Mục tiêu:** Tạo game cốt truyện với 3 scene mỗi chapter, dễ ghép team, không conflict.

```
BOOTSTRAP_Main (Managers: Game, Scene, Audio, Save)
        ↓
UI_Common (HUD, Pause Menu, Dialogue)
        ↓
Chapter Scenes (Load Additive)
    ├─ Chap01_Environment (Terrain, objects tĩnh)
    ├─ Chap01_Gameplay (Player, enemies, quests)
    └─ Chap01_Lighting (Lights, effects, atmosphere)
```

---

## ⚡ 5 Bước Nhanh Để Bắt Đầu

### Bước 1: Tạo Folders (Đã xong ✓)
```
Assets/
├── Scenes/Bootstrap/
├── Scenes/UI/
├── Scenes/Chapters/Chap01/
├── Scripts/
│   ├── Managers/
│   ├── Gameplay/
│   ├── Player/
│   └── Utils/
├── Prefabs/Managers/
└── ...
```

### Bước 2: Tạo BOOTSTRAP_Main Scene

**File → New Scene → BOOTSTRAP_Main.unity**

**Hierarchy:**
```
BOOTSTRAP_Main
├── Managers (GameObject)
│   ├── GameManager (script)
│   ├── SceneLoadManager (script)
│   ├── AudioManager (script + AudioSource x2)
│   └── SaveManager (script)
└── Canvas (UI)
    └── LoadingScreen
```

**Cài đặt:**
```csharp
// Tất cả managers phải có DontDestroyOnLoad
void Awake() 
{ 
    if (Instance == null) {
        Instance = this;
        DontDestroyOnLoad(gameObject);  ← KEY!
    }
}
```

### Bước 3: Tạo UI_Common Scene

**File → New Scene → UI_Common.unity**

**Hierarchy:**
```
UI_Common
├── HUD (Canvas)
│   ├── HealthBar
│   ├── Inventory
│   └── QuickSlots
├── PauseMenu (Canvas)
│   ├── ResumeButton
│   ├── SettingsButton
│   └── ExitButton
└── DialogueUI (Canvas)
    ├── DialogueBox
    ├── SpeakerName
    └── Choices
```

**Cài đặt:**
- Canvas → Render Mode: **Overlay**
- Canvas Scaler → Reference Resolution: 1920x1080
- Thêm DontDestroyOnLoad nếu có
  
### Bước 4: Build Settings

**File → Build Settings → Scenes in Build**

**Add scenes:**
```
0 - Assets/Scenes/Bootstrap/BOOTSTRAP_Main
1 - Assets/Scenes/UI/UI_Common
2 - Assets/Scenes/Chapters/Chap01/Chap01_Environment
3 - Assets/Scenes/Chapters/Chap01/Chap01_Gameplay
4 - Assets/Scenes/Chapters/Chap01/Chap01_Lighting
```

### Bước 5: Tạo Chapter Scenes

**File → New Scene × 3:**
1. `Chap01_Environment.unity`
2. `Chap01_Gameplay.unity`
3. `Chap01_Lighting.unity`

**Lưu vào:** `Assets/Scenes/Chapters/Chap01/`

---

## 🎮 Các Script Có Sẵn

| Script | Mục đích | Vị trí |
|--------|---------|--------|
| **GameManager.cs** | Quản lý chapters, pause | Scripts/Managers/ |
| **SceneLoadManager.cs** | Load/unload scenes additive | Scripts/Managers/ |
| **AudioManager.cs** | BGM, SFX | Scripts/Managers/ |
| **SaveManager.cs** | Save/load game state | Scripts/Managers/ |
| **Player.cs** | Player movement | Scripts/Player/ |
| **Enemy.cs** | Enemy AI | Scripts/Gameplay/ |
| **NPC.cs** | NPCs (Merchant, Guard) | Scripts/Gameplay/ |
| **Interactable.cs** | Chest, Door, Lever | Scripts/Gameplay/ |
| **SceneDebugger.cs** | Keyboard shortcuts | Scripts/Utils/ |

---

## 🎯 Test Shortcuts

**Khi play game, ấn:**

| Phím | Chức năng |
|------|----------|
| **1-5** | Load Chapter 1-5 |
| **N** | Next chapter |
| **R** | Restart chapter |
| **ESC** | Pause/Resume |
| **S** | Save game |
| **L** | Load game |
| **D** | Debug info |

---

## 📐 Cấu Trúc Mỗi Chapter

### Environment Scene
```
✓ Terrain (painted/imported)
✓ Trees, buildings (static)
✓ Props, rocks
✓ NavMesh (baked)
```

### Gameplay Scene
```
✓ PlayerSpawn (tag: "PlayerSpawn")
✓ Enemies (prefabs)
✓ NPCs (prefabs)
✓ Interactables (chest, doors)
✓ Puzzles, events
```

### Lighting Scene
```
✓ Directional light (sun)
✓ Point lights (torches)
✓ Post-processing (bloom, AO)
✓ Particle effects
```

---

## 🔧 Thêm Scripts Vào Scene

**BOOTSTRAP_Main → Managers GameObject:**

1. Thêm component **GameManager.cs**
2. Thêm component **SceneLoadManager.cs**
3. Thêm component **AudioManager.cs** (2x AudioSource)
4. Thêm component **SaveManager.cs**

**Chap01_Gameplay scene:**

1. GameObject "GameplayManager" + script ChapGameplay.cs
2. Thêm component **SceneDebugger.cs** (any GameObject)

---

## 📝 Checklist Bắt Đầu

- [ ] Tạo BOOTSTRAP_Main scene với 4 managers
- [ ] Tạo UI_Common scene với HUD & menus
- [ ] Add 5 scenes vào Build Settings (đúng thứ tự)
- [ ] Tạo 3 chapter scenes (Environment, Gameplay, Lighting)
- [ ] Đặt "PlayerSpawn" tag trong Chap01_Gameplay
- [ ] Add Player prefab vào Chap01_Gameplay
- [ ] Thêm SceneDebugger vào BOOTSTRAP_Main
- [ ] Test: Play → Ấn 1 → Chapter load được không?
- [ ] Test: Ấn ESC → Game pause không?
- [ ] Test: Ấn N → Chapter 2 load không? (chưa có scene thì error)

---

## 🚨 Common Issues

| Lỗi | Nguyên Nhân | Cách Fix |
|-----|-----------|---------|
| Player lost references | Manager unload khi load scene | Dùng DontDestroyOnLoad |
| Scene không load | Build Settings thiếu scene | Add scene vào Build Settings |
| PlayerSpawn không tìm được | Chưa set tag | Select PlayerSpawn → Tag: "PlayerSpawn" |
| Manager duplicate | Awake không check Instance | Thêm singleton pattern |
| Audio lag | AudioSource không setup | Drag AudioClip vào bgmSource |

---

## 📚 Tài Liệu Chi Tiết

Xem thêm:
- [GAME_STRUCTURE.md](GAME_STRUCTURE.md) - Kiến trúc tổng quát
- [SETUP_BOOTSTRAP_UI.md](SETUP_BOOTSTRAP_UI.md) - Chi tiết bootstrap & UI setup
- [SETUP_CHAPTERS.md](SETUP_CHAPTERS.md) - Chi tiết chapter scenes

---

## 💡 Tips Pro

1. **Copy-Paste Chapters**: Để tạo Chap02, copy Chap01 folder → Rename → Edit
2. **Use Prefabs**: Enemy, NPC, Interactables phải là prefabs
3. **Bake Lighting**: Bake shadows trước build final
4. **Load Optimization**: Unload chapter cũ trước load chapter mới
5. **Save Data**: Luôn gọi `SaveManager.Instance.SaveGame()` sau mỗi event quan trọng

---

## 🎬 Workflow Phát Triển

```
Day 1: Tạo Bootstrap + UI scenes
Day 2: Tạo Chap01_Environment (terrain, props)
Day 3: Tạo Chap01_Gameplay (player, enemies, puzzles)
Day 4: Tạo Chap01_Lighting (lights, effects)
Day 5: Test & polish
Day 6+: Dupe cho chapters 2, 3, 4, 5
```

---

## 🤝 Collaborative Workflow

**Khi làm team:**

```
Person A: Làm Environment scenes
Person B: Làm Gameplay scenes
Person C: Làm UI & Managers
Person D: Làm Lighting & Effects
```

Mỗi người làm scene riêng → Merge dễ vì ít conflict!

---

## 🚀 Kế Tiếp

1. Tạo BOOTSTRAP_Main scene ngay
2. Tạo UI_Common scene
3. Add managers vào BOOTSTRAP_Main
4. Setup Build Settings
5. Play game & test shortcuts
6. Bắt đầu làm Chap01

**Chúc bạn phát triển game thành công! 🎮**
