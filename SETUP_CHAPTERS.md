# Hướng Dẫn Tạo Chapter Scenes (Chap01 Example)

## 📁 Cấu Trúc Thư Mục

```
Assets/
├── Scenes/Chapters/Chap01/
│   ├── Chap01_Environment.unity
│   ├── Chap01_Gameplay.unity
│   └── Chap01_Lighting.unity
├── Scripts/
│   ├── Gameplay/
│   │   ├── Enemy.cs
│   │   ├── Interactable.cs
│   │   └── Puzzle.cs
│   └── Chapter/
│       ├── ChapEnvironment.cs
│       └── ChapGameplay.cs
├── Prefabs/
│   ├── Enemies/
│   │   ├── Enemy_Goblin.prefab
│   │   └── Enemy_Orc.prefab
│   ├── NPCs/
│   │   ├── NPC_Merchant.prefab
│   │   └── NPC_Guard.prefab
│   └── Interactables/
│       ├── Chest.prefab
│       └── Door.prefab
└── Resources/
    └── Audio/BGM/
        ├── Chap01_BGM.mp3
        └── Chap02_BGM.mp3
```

## 🎬 Step 1: Tạo Chap01_Environment Scene

### 1.1 Tạo Scene
```
File → New Scene
Đặt tên: Chap01_Environment
Lưu vào: Assets/Scenes/Chapters/Chap01/
```

### 1.2 Hierarchy
```
Chap01_Environment
├── Terrain
│   ├─ Terrain (Component)
│   └─ TerrainData
├── StaticObjects
│   ├─ Trees
│   │  ├─ Tree_01
│   │  ├─ Tree_02
│   │  └─ Tree_03 (với LOD Groups)
│   ├─ Buildings
│   │  ├─ House_01
│   │  ├─ House_02
│   │  └─ Bridge
│   └─ Props
│      ├─ Barrel
│      ├─ Crate
│      └─ Torch
├── NavMeshes
│   └─ NavMesh (baked)
└── WayPoints (Empty)
   ├─ WayPoint_01
   ├─ WayPoint_02
   └─ WayPoint_03
```

### 1.3 Cài Đặt
- **Terrain**: Có thể import từ heightmap hoặc paint trong Unity
- **Static Objects**: Mark as "Static" → Window → Rendering → Lightning → Bake (untuk shadows)
- **NavMesh**: Window → AI → Navigation → Bake NavMesh

**Code ví dụ:**
```csharp
// ChapEnvironment.cs
public class ChapEnvironment : MonoBehaviour, IChapterSetup
{
    public void OnChapterSetup(int chapterNumber)
    {
        Debug.Log($"[ChapEnvironment] Setup chapter {chapterNumber}");
        // Setup weather, fog, etc.
    }
}
```

---

## 🎮 Step 2: Tạo Chap01_Gameplay Scene

### 2.1 Tạo Scene
```
File → New Scene
Đặt tên: Chap01_Gameplay
Lưu vào: Assets/Scenes/Chapters/Chap01/
```

### 2.2 Hierarchy
```
Chap01_Gameplay
├── SpawnPoints
│   ├─ PlayerSpawn (Tag: "PlayerSpawn")
│   │  └─ Position: (0, 1, 0)
│   └─ EnemySpawns
│      ├─ EnemySpawn_01
│      └─ EnemySpawn_02
├── Player (Prefab)
│   └─ Character
├── Enemies
│   ├─ Enemy_Goblin (Prefab instance)
│   │  ├─ Health: 30
│   │  └─ Loot: Gold x5
│   ├─ Enemy_Orc (Prefab instance)
│   │  └─ Health: 50
│   └─ Enemy_Boss (Prefab instance)
│      └─ Health: 100
├── NPCs
│   ├─ NPC_Merchant (Prefab)
│   │  └─ Position: (10, 1, 5)
│   └─ NPC_Guard (Prefab)
│      └─ Position: (15, 1, 0)
├── Interactables
│   ├─ Chest_01 (Prefab)
│   │  ├─ Items: Potion x3
│   │  └─ IsLocked: false
│   ├─ Door_01 (Prefab)
│   │  └─ IsLocked: true (key required)
│   ├─ Lever_01 (Prefab)
│   │  └─ LinkedDoor: Door_01
│   └─ Torch_01 (Prefab)
│      └─ Light: Enabled
├── Puzzles
│   ├─ PuzzleManager
│   └─ PuzzleElement_01 (ví dụ: 4 levers để mở cửa)
├── Events
│   ├─ EventTrigger_Intro
│   │  └─ OnTrigger: Play dialogue
│   ├─ EventTrigger_BossArena
│   │  └─ OnTrigger: Spawn boss + play music
│   └─ EventTrigger_Ending
│      └─ OnTrigger: Complete chapter
└── GameplayManager
    └─ Script: ChapGameplay.cs
```

### 2.3 Enemy Prefab Example

**File: Assets/Prefabs/Enemies/Enemy_Goblin.prefab**
```csharp
// Enemy.cs
public class Enemy : MonoBehaviour
{
    [SerializeField] private float health = 30f;
    [SerializeField] private float damage = 5f;
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private Animator animator;
    
    private Player targetPlayer;
    
    private void Start()
    {
        targetPlayer = FindObjectOfType<Player>();
    }
    
    private void Update()
    {
        if (targetPlayer == null) return;
        
        float distanceToPlayer = Vector3.Distance(transform.position, targetPlayer.transform.position);
        
        if (distanceToPlayer < attackRange)
        {
            Attack();
        }
        else
        {
            MoveToward(targetPlayer.transform.position);
        }
    }
    
    private void MoveToward(Vector3 target)
    {
        transform.position = Vector3.Lerp(transform.position, target, Time.deltaTime * 3f);
    }
    
    private void Attack()
    {
        // targetPlayer.TakeDamage(damage);
        animator.SetTrigger("Attack");
    }
    
    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }
    
    private void Die()
    {
        Destroy(gameObject);
    }
}
```

### 2.4 Interactable Prefab Example

**File: Assets/Prefabs/Interactables/Chest.prefab**
```csharp
// Interactable.cs
public class Interactable : MonoBehaviour
{
    [SerializeField] private bool isLocked = false;
    [SerializeField] private ItemData[] items;
    [SerializeField] private Animator animator;
    
    private bool isOpened = false;
    
    public void Interact()
    {
        if (isLocked)
        {
            Debug.Log("Chest is locked!");
            return;
        }
        
        if (isOpened) return;
        
        isOpened = true;
        animator.SetTrigger("Open");
        
        // Drop items
        foreach (ItemData item in items)
        {
            DropItem(item);
        }
    }
    
    private void DropItem(ItemData item)
    {
        // Instantiate item in world
    }
}
```

### 2.5 ChapGameplay Setup Script

```csharp
// ChapGameplay.cs
public class ChapGameplay : MonoBehaviour, IChapterSetup
{
    [SerializeField] private AudioClip chapBGM;
    
    public void OnChapterSetup(int chapterNumber)
    {
        Debug.Log($"[ChapGameplay] Setting up chapter {chapterNumber}");
        
        // Play BGM
        if (chapBGM != null)
        {
            AudioManager.Instance.PlayBGM(chapBGM);
        }
        
        // Initialize chapter-specific systems
        InitializeEnemies();
        InitializeNPCs();
        InitializePuzzles();
    }
    
    private void InitializeEnemies()
    {
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        foreach (Enemy enemy in enemies)
        {
            // Setup spawning, patrol, etc.
        }
    }
    
    private void InitializeNPCs()
    {
        NPC[] npcs = FindObjectsOfType<NPC>();
        foreach (NPC npc in npcs)
        {
            // Setup dialogue trees, quests
        }
    }
    
    private void InitializePuzzles()
    {
        Puzzle[] puzzles = FindObjectsOfType<Puzzle>();
        foreach (Puzzle puzzle in puzzles)
        {
            // Setup puzzle states
        }
    }
    
    public void CompleteChapter()
    {
        SaveManager.Instance.CompleteChapter(GameManager.Instance.GetCurrentChapter());
        GameManager.Instance.NextChapter();
    }
}
```

---

## 🎨 Step 3: Tạo Chap01_Lighting Scene

### 3.1 Tạo Scene
```
File → New Scene
Đặt tên: Chap01_Lighting
Lưu vào: Assets/Scenes/Chapters/Chap01/
```

### 3.2 Hierarchy
```
Chap01_Lighting
├── Lighting
│   ├─ DirectionalLight (Sun)
│   │  ├─ Intensity: 1.2
│   │  ├─ Color: White
│   │  └─ Shadows: Soft (baked)
│   ├─ PointLights
│   │  ├─ Torch_Fire_01
│   │  │  ├─ Intensity: 2.5
│   │  │  ├─ Color: Orange
│   │  │  └─ Range: 15
│   │  ├─ Torch_Fire_02
│   │  └─ Lamp_01
│   └─ SpotLights
│      └─ SpotLight_Cave (để soi sáng hang động)
├── PostProcessing
│   └─ Volume
│      ├─ Profile: ChapProfile
│      └─ Overrides:
│         ├─ Bloom (Intensity: 2)
│         ├─ Ambient Occlusion (Intensity: 0.5)
│         ├─ Tonemapping (ACES)
│         └─ ChromaticAberration (Intensity: 0.1)
├── Fog
│   └─ Script: FogController.cs
│      ├─ FogDensity: 0.01
│      ├─ FogColor: Gray
│      └─ FogStart/End: 10 - 100
└── Effects
    └─ ParticleSystems
       ├─ Rain.prefab (nếu có)
       ├─ Dust.prefab
       └─ Fireflies.prefab
```

### 3.3 Post Processing Setup

**Window → Rendering → Volumes → New Volume**

```csharp
// FogController.cs
public class FogController : MonoBehaviour, IChapterSetup
{
    [SerializeField] private float fogDensity = 0.01f;
    [SerializeField] private Color fogColor = Color.gray;
    
    public void OnChapterSetup(int chapterNumber)
    {
        RenderSettings.fog = true;
        RenderSettings.fogDensity = fogDensity;
        RenderSettings.fogColor = fogColor;
    }
}
```

---

## 🔗 Step 4: Add Scenes Vào Build Settings

### 4.1 Build Settings
```
File → Build Settings → Scenes in Build

Thêm vào:
- Chap01_Environment.unity
- Chap01_Gameplay.unity
- Chap01_Lighting.unity
```

### 4.2 Load Order
```
// SceneLoadManager sẽ tự động load 3 scenes này additive
// Thứ tự: Environment → Gameplay → Lighting
```

---

## ✅ Step 5: Test Loading

### 5.1 Play từ BOOTSTRAP_Main
```
1. Click Play
2. Ấn phím 1 → Load Chap01
3. Kiểm tra Console có errors?
4. Player có spawn ở đúng vị trí?
5. Enemies có hiện không?
6. Lights có sáng không?
```

### 5.2 Debug Commands
```
Ấn phím 1-5: Load Chap1-5
Ấn phím N: Chap tiếp theo
Ấn phím R: Restart
Ấn phím ESC: Pause
```

---

## 🎯 Kế Tiếp

- [ ] Create Chap01_Environment scene
  - [ ] Paint terrain
  - [ ] Place trees & buildings
  - [ ] Bake NavMesh
- [ ] Create Chap01_Gameplay scene
  - [ ] Place player spawn
  - [ ] Spawn enemies
  - [ ] Create interactables
- [ ] Create Chap01_Lighting scene
  - [ ] Setup main light
  - [ ] Add point lights
  - [ ] Configure post-processing
- [ ] Test loading chain
- [ ] Duplicate & rename cho Chap02, Chap03...

---

## 💡 Tips

1. **Copy & Paste**: Để tạo Chap02, copy toàn bộ Chap01 folder → Rename → Edit
2. **Prefabs**: Luôn dùng prefabs cho enemies, NPCs, interactables
3. **Performance**: Bake lighting, unload scenes cũ khi load chapter mới
4. **Save/Load**: SaveManager sẽ tự động track chapter hoàn thành
