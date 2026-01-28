# Hướng Dẫn Thiết Lập Bootstrap & UI_Common Scenes

## 📋 Bước 1: Tạo BOOTSTRAP_Main Scene

### 1.1 Tạo Scene Mới
```
Menu → File → New Scene
Đặt tên: BOOTSTRAP_Main
Lưu vào: Assets/Scenes/Bootstrap/
```

### 1.2 Hierarchy Setup
```
BOOTSTRAP_Main
├── Bootstrap (GameObject)
│   ├── GameManager (Script)
│   ├── SceneLoadManager (Script)
│   ├── AudioManager (Script + 2x AudioSource)
│   └── SaveManager (Script)
├── Canvas (từ UI prefab)
│   └── LoadingScreen (GameObject)
│       ├─ Panel (Background)
│       └─ ProgressBar (Slider/Image)
└── EventSystem (tự động tạo khi có Canvas)
```

### 1.3 Script Assignment
**GameObject "Bootstrap" - Thêm Components:**
1. **GameManager.cs**
2. **SceneLoadManager.cs**
3. **AudioManager.cs** + AudioSource (BGM) + AudioSource (SFX)
4. **SaveManager.cs**

```csharp
// Trong GameManager.cs - Awake()
void Awake()
{
    if (Instance == null)
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);  // ← KEY!
        Debug.Log("[GameManager] Initialized");
    }
    else
    {
        Destroy(gameObject);
    }
}
```

### 1.4 Đảm bảo DontDestroyOnLoad
Tất cả managers trên GameObject "Bootstrap" phải có:
```csharp
DontDestroyOnLoad(gameObject);
```

## 📋 Bước 2: Tạo UI_Common Scene

### 2.1 Tạo Scene Mới
```
Menu → File → New Scene
Đặt tên: UI_Common
Lưu vào: Assets/Scenes/UI/
```

### 2.2 Hierarchy Setup
```
UI_Common
├── HUD (Canvas)
│   ├── HealthBar
│   │   ├─ Background (Image)
│   │   └─ Fill (Image)
│   ├── Inventory
│   │   ├─ Button_Inventory
│   │   └─ Slot_01, Slot_02... (Grid Layout)
│   ├── Minimap (RawImage)
│   └── QuickSlots
│       ├─ Slot_Q
│       ├─ Slot_E
│       └─ Slot_R
├── PauseMenu (Canvas)
│   ├── Panel (Background)
│   ├── Button_Resume
│   ├── Button_Settings
│   └── Button_MainMenu
├── DialogueUI (Canvas)
│   ├── DialogueBox (Image)
│   ├── SpeakerName (Text)
│   ├── DialogueText (Text)
│   ├── Choices (VerticalLayoutGroup)
│   │   ├─ ChoiceButton_01
│   │   └─ ChoiceButton_02
└── EventSystem
```

### 2.3 Canvas Setup
**Cài đặt Canvas:**
- Render Mode: `Overlay` (nằm trên gameplay)
- Canvas Scaler: 
  - Reference Resolution: 1920x1080
  - Scale Mode: Scale With Screen Size

## 🎮 Bước 3: Setup Scene Trong Build Settings

### 3.1 Add Scenes Vào Build
```
File → Build Settings → Scenes in Build
```

**Đơn hàng phải là:**
```
0 - Assets/Scenes/Bootstrap/BOOTSTRAP_Main
1 - Assets/Scenes/UI/UI_Common
2 - Assets/Scenes/Chapters/Chap01/Chap01_Environment
3 - Assets/Scenes/Chapters/Chap01/Chap01_Gameplay
4 - Assets/Scenes/Chapters/Chap01/Chap01_Lighting
5 - Assets/Scenes/Chapters/Chap02/... (Tương tự)
...
```

### 3.2 Workflow Load
```csharp
// Build Settings order quan trọng:
// Scene 0: BOOTSTRAP_Main (DontDestroyOnLoad)
// Scene 1: UI_Common (DontDestroyOnLoad)
// Scene 2+: Chapter Scenes (Load Additive)
```

## 🔧 Bước 4: Tạo Managers Container Prefab

### 4.1 Tạo Prefab từ Bootstrap
```
Drag "Bootstrap" GameObject → Assets/Prefabs/Managers/
Đặt tên: "ManagersBundle"
```

### 4.2 Nếu muốn tạo mới lần sau
```
Đơn giản drag prefab này vào scene mới
```

## ✅ Bước 5: Test Loading

### 5.1 Script Test (Optional)
Tạo `Assets/Scripts/Utils/SceneDebugger.cs`:

```csharp
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneDebugger : MonoBehaviour
{
    private void Update()
    {
        // Phím 1-5 để load chapters
        for (int i = 1; i <= 5; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha0 + i))
            {
                GameManager.Instance.LoadChapter(i);
            }
        }
        
        // Phím N để chapter tiếp theo
        if (Input.GetKeyDown(KeyCode.N))
        {
            GameManager.Instance.NextChapter();
        }
        
        // Phím R để restart
        if (Input.GetKeyDown(KeyCode.R))
        {
            GameManager.Instance.RestartChapter();
        }
        
        // Phím ESC để pause
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameManager.Instance.TogglePause();
        }
    }
}
```

### 5.2 Test Steps
1. Play từ BOOTSTRAP_Main scene
2. Kiểm tra Console có errors?
3. Ấn phím 1 → Chap01 có load không?
4. Ấn phím N → Chap02 có load không?
5. Ấn phím ESC → Game có pause không?

## 🎯 Bước 6: Tạo Chapter Scene Template

### 6.1 Chap01_Environment.unity
```
Hierarchy:
- Terrain
  ├─ Terrain (Asset)
  └─ TerrainData
- StaticObjects
  ├─ Trees
  ├─ Rocks
  ├─ Buildings
  └─ Props
- NavMeshes (baked)
```

### 6.2 Chap01_Gameplay.unity
```
Hierarchy:
- SpawnPoints
  ├─ PlayerSpawn (Tag: "PlayerSpawn")
  └─ EnemySpawns
- GameplayObjects
  ├─ Enemies
  ├─ NPCs
  ├─ Interactables
  └─ Puzzles
- EventManager
```

### 6.3 Chap01_Lighting.unity
```
Hierarchy:
- Lighting
  ├─ DirectionalLight
  ├─ Lights (PointLights, SpotLights)
  └─ VolumeProfile (PostProcessing)
- Effects
  ├─ Particle Systems
  └─ Weather
```

## 💡 Important Notes

1. **DontDestroyOnLoad** - Quan trọng nhất! Không có nó = Lost references
2. **Scene Order** - Build Settings phải đúng thứ tự
3. **Tags** - Tạo Tags "PlayerSpawn", "Ground", "Environment"
4. **Player** - Phải có Player prefab trong Chap01_Gameplay
5. **Audio** - Load BGM khi chapter load bằng `AudioManager.PlayBGM()`

## 🔗 Liên Kết Scripts

```csharp
// Ví dụ: Chuyển chapter khi hoàn thành
public void CompleteChapter()
{
    SaveManager.Instance.CompleteChapter(GameManager.Instance.GetCurrentChapter());
    GameManager.Instance.NextChapter();
}

// Ví dụ: Phát nhạc chapter
private void Start()
{
    AudioClip chap1Bgm = Resources.Load<AudioClip>("Audio/BGM/Chap01_BGM");
    AudioManager.Instance.PlayBGM(chap1Bgm);
}
```

## 🚀 Kế Tiếp
- [ ] Tạo BOOTSTRAP_Main scene + assign scripts
- [ ] Tạo UI_Common scene + HUD UI
- [ ] Add scenes vào Build Settings
- [ ] Create Chap01_Environment scene
- [ ] Create Chap01_Gameplay scene
- [ ] Create Chap01_Lighting scene
- [ ] Test loading chapters (Phím 1-5)
- [ ] Test pause (Phím ESC)
- [ ] Tạo SceneDebugger script để test
