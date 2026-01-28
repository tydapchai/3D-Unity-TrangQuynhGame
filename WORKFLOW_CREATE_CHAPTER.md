# 📝 Quy Trình Tạo 1 Chapter - Step by Step

## 🎯 Overview

Mỗi chapter gồm **3 scenes** được load **additive** (thêm vào, không thay thế):

1. **Chap01_Environment** - Terrain, buildings, static objects
2. **Chap01_Gameplay** - Player, enemies, NPCs, interactables, puzzles
3. **Chap01_Lighting** - Lights, post-processing, effects

**Thời gian:** ~2-3 ngày/chapter (tùy độ phức tạp)

---

## 📋 Checklist Trước Khi Bắt Đầu

- [ ] BOOTSTRAP_Main scene đã tạo + managers setup
- [ ] UI_Common scene đã tạo
- [ ] Build Settings có BOOTSTRAP_Main & UI_Common
- [ ] Player prefab có sẵn
- [ ] Tags tạo: "PlayerSpawn", "Ground", "Environment"
- [ ] Folders sạch: Scenes/Chapters/Chap01/

---

## 🚀 PHASE 1: ENVIRONMENT SCENE (Day 1)

### Step 1.1: Tạo Scene File

```
File → New Scene
Đặt tên: Chap01_Environment
Lưu vào: Assets/Scenes/Chapters/Chap01/
```

### Step 1.2: Setup Hierarchy

```
Chap01_Environment (Root)
├── Terrain
│   └── Terrain (Component)
├── Environment (Empty GameObject - organize)
│   ├── Buildings
│   │   ├── House_01
│   │   ├── House_02
│   │   └── Bridge
│   ├── Trees
│   │   ├── Tree_01 (với LOD Group)
│   │   ├── Tree_02
│   │   └── Tree_03
│   ├── Props
│   │   ├── Barrel
│   │   ├── Crate
│   │   └── Rocks
│   └── StaticDecoration
│       ├── Torches (không có light)
│       └── Signs
└── Navigation
    └── NavMesh (baked)
```

### Step 1.3: Tạo Terrain

```
Hierarchy → Right-click → 3D Object → Terrain
```

**Cài đặt Terrain:**
1. Chọn Terrain → Inspector
2. Terrain Tools:
   - **Paint Heightmap** - Vẽ độ cao
   - **Paint Texture** - Vẽ cỏ, đá, cát
3. Bake Lighting (Window → Rendering → Lighting)

**Hoặc import heightmap:**
```
Assets → Terrain Layers → Drag heightmap vào Terrain
```

### Step 1.4: Thêm Buildings & Props

**Từ Asset Store hoặc tự tạo:**

```csharp
// Import từ Blender/Maya
1. Export model as .fbx
2. Drag vào Assets/Art/Models/
3. Drag từ Assets vào scene
```

**Setup Building:**
```
Building_House (GameObject)
├── Model (MeshRenderer + MeshFilter)
├── Collider (BoxCollider)
└── Tag: "Environment"

Properties:
- Static: Tích (để bake lighting)
- Layer: "Default"
```

### Step 1.5: Bake NavMesh

Bước này **rất quan trọng** để enemy có thể di chuyển!

```
1. Chọn tất cả static objects (Buildings, Terrain)
2. Chọn tag: "Baked" hoặc "Navigation Static"
3. Window → AI → Navigation
4. Bake Tab:
   - Agent Radius: 0.5
   - Agent Height: 2
   - Agent Slope: 45
   - Step Height: 0.3
5. Click "Bake"
```

**Result:** NavMesh sẽ xuất hiện (xanh lục trong scene view)

### Step 1.6: Tạo Waypoints (Optional)

Nếu có NPC patrol:

```
Hierarchy:
WayPoints (Empty)
├── Waypoint_01 (Empty)
│   └── Position: (0, 0, 0)
├── Waypoint_02
│   └── Position: (10, 0, 0)
└── Waypoint_03
    └── Position: (10, 0, 10)
```

### Step 1.7: Save & Test Load

```
File → Save
Play từ BOOTSTRAP_Main scene
Ấn phím 1 → Chap01 có load không?
Kiểm tra Console - có error?
```

**Kết quả kỳ vọng:**
- ✅ Terrain, buildings, trees hiện
- ✅ Không có errors
- ✅ Scene load xong sau ~2-3 giây
- ✅ NavMesh baked (xanh lục)

---

## 🎮 PHASE 2: GAMEPLAY SCENE (Day 2)

### Step 2.1: Tạo Scene File

```
File → New Scene
Đặt tên: Chap01_Gameplay
Lưu vào: Assets/Scenes/Chapters/Chap01/
```

### Step 2.2: Setup Hierarchy

```
Chap01_Gameplay
├── SpawnPoints
│   ├── PlayerSpawn (Empty)
│   │   └── Tag: "PlayerSpawn"
│   │   └── Position: (0, 1, 0) - trên terrain
│   └── EnemySpawns
│       ├── EnemySpawn_01
│       ├── EnemySpawn_02
│       └── EnemySpawn_03
├── Player
│   └── [Drag Player Prefab here]
├── Enemies (Organize)
│   ├── Enemy_Goblin (Prefab)
│   │   ├── Position: (5, 0, 5)
│   │   └── Health: 30
│   ├── Enemy_Orc (Prefab)
│   │   ├── Position: (10, 0, 10)
│   │   └── Health: 50
│   └── Enemy_Boss (Prefab)
│       ├── Position: (20, 0, 20)
│       └── Health: 100
├── NPCs
│   ├── NPC_Merchant (Prefab)
│   │   └── Position: (15, 0, 5)
│   └── NPC_Guard (Prefab)
│       └── Position: (20, 0, 0)
├── Interactables
│   ├── Chest_01 (Prefab)
│   │   ├── Position: (8, 0, 8)
│   │   └── Items: [Potion x3]
│   ├── Door_01 (Prefab)
│   │   ├── Position: (12, 0, 3)
│   │   └── Locked: true
│   ├── Lever_01 (Prefab)
│   │   ├── Position: (10, 0, 2)
│   │   └── LinkedObject: Door_01
│   └── Torch_01 (Prefab)
│       └── Position: (5, 0, 3)
├── Puzzles
│   ├── PuzzleManager (GameObject)
│   │   └── Script: PuzzleManager.cs
│   └── PuzzleElement_01
│       └── 4 Levers → Open Gate
├── Events
│   ├── EventTrigger_Intro
│   │   ├── SphereCollider (isTrigger: true)
│   │   └── Script: EventTrigger.cs
│   └── EventTrigger_BossArena
│       └── Script: EventTrigger.cs
└── GameplayManager (GameObject)
    └── Script: ChapGameplay.cs
```

### Step 2.3: Setup PlayerSpawn

```
Hierarchy → Right-click → Create Empty
Đặt tên: PlayerSpawn
Tag: "PlayerSpawn"
Position: (0, 1, 0)
```

**Cài đặt:**
```csharp
// PlayerSpawn là một empty object đánh dấu vị trí spawn
// SceneLoadManager sẽ tìm nó và teleport player tới đây
```

### Step 2.4: Thêm Player Prefab

```
Drag Assets/Prefabs/Player.prefab vào scene
Position: (0, 1, 0) - hoặc sẽ được teleport tới PlayerSpawn
```

### Step 2.5: Thêm Enemies

**Tạo Enemy Prefab (nếu chưa có):**

```csharp
// Assets/Prefabs/Enemies/Enemy_Goblin.prefab

Hierarchy:
Enemy_Goblin
├── Model (Mesh)
├── Animator
├── Collider (CapsuleCollider)
├── NavMeshAgent
├── Rigidbody (is Kinematic: true)
└── Script: Enemy.cs
```

**Cài đặt Enemy.cs Inspector:**
```
Health: 30
Damage: 5
AttackRange: 2
NavMeshAgent:
  - Speed: 3.5
  - Acceleration: 8
  - Stopping Distance: 1.5
  - Autobaking: false (quan trọng!)
```

**Đặt enemies vào scene:**
```
Drag Enemy_Goblin prefab từ Prefabs/Enemies/
Position: (5, 0, 5)
Rotate: theo hướng muốn
```

Lặp lại với Orc, Boss, v.v.

### Step 2.6: Thêm NPCs

**NPC Merchant:**
```
Drag NPC_Merchant prefab vào scene
Position: (15, 0, 5)
Inspector:
  - NPC Name: "Chủ Cửa Hàng"
  - Shop Items: [Potion, Mana Potion, Antidote]
```

**NPC Guard (với patrol):**
```
Drag NPC_Guard prefab vào scene
Position: (20, 0, 0)
Inspector:
  - NPC Name: "Lính Gác"
  - Patrol Points: [Waypoint_01, Waypoint_02, Waypoint_03]
  - Patrol Speed: 2
```

### Step 2.7: Thêm Interactables

**Chest:**
```
Drag Chest prefab vào scene
Position: (8, 0, 8)
Inspector:
  - Interactable Name: "Rương Vàng"
  - Items: [Potion x3, Gold x10]
  - IsLocked: false
  - Animator: [drag animation từ Assets/Animations/]
```

**Door:**
```
Drag Door prefab vào scene
Position: (12, 0, 3)
Inspector:
  - Interactable Name: "Cửa Bí Mật"
  - IsLocked: true
  - Animator: Drag door open/close animation
```

**Lever (để mở door):**
```
Drag Lever prefab vào scene
Position: (10, 0, 2)
Inspector:
  - Interactable Name: "Đòn Bẩy"
  - LinkedObject: [Drag Door_01 từ hierarchy]
  - Animator: Lever pull animation
```

### Step 2.8: Tạo ChapGameplay Script

**File: Assets/Scripts/Chapter/ChapGameplay.cs**

```csharp
public class ChapGameplay : MonoBehaviour, IChapterSetup
{
    [SerializeField] private AudioClip chapBGM;
    [SerializeField] private int chapterNumber = 1;
    
    public void OnChapterSetup(int chapter)
    {
        chapterNumber = chapter;
        Debug.Log($"[ChapGameplay] Setting up chapter {chapter}");
        
        // Play BGM
        if (chapBGM != null)
        {
            AudioManager.Instance.PlayBGM(chapBGM);
        }
        else
        {
            Debug.LogWarning("[ChapGameplay] BGM not assigned!");
        }
        
        // Initialize systems
        InitializeEnemies();
        InitializeNPCs();
        InitializePuzzles();
    }
    
    private void InitializeEnemies()
    {
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        Debug.Log($"[ChapGameplay] Initialized {enemies.Length} enemies");
    }
    
    private void InitializeNPCs()
    {
        NPC[] npcs = FindObjectsOfType<NPC>();
        Debug.Log($"[ChapGameplay] Initialized {npcs.Length} NPCs");
    }
    
    private void InitializePuzzles()
    {
        // Setup puzzles here
        Debug.Log("[ChapGameplay] Puzzles initialized");
    }
    
    public void CompleteChapter()
    {
        Debug.Log($"[ChapGameplay] Chapter {chapterNumber} completed!");
        SaveManager.Instance.CompleteChapter(chapterNumber);
        GameManager.Instance.NextChapter();
    }
}
```

**Drag vào scene:**
```
Hierarchy → GameplayManager → Add Component → ChapGameplay
Inspector:
  - Chap BGM: [Drag audio file từ Resources/Audio/BGM/Chap01_BGM]
  - Chapter Number: 1
```

### Step 2.9: Test

```
Play từ BOOTSTRAP_Main
Ấn phím 1 → Load Chap01
Kiểm tra:
  ✅ Player spawn tại PlayerSpawn
  ✅ Enemies di chuyển (tìm player)
  ✅ NPCs hiện
  ✅ Interactables react (chest mở, door khóa)
  ✅ BGM phát
```

---

## 🎨 PHASE 3: LIGHTING SCENE (Day 2-3)

### Step 3.1: Tạo Scene File

```
File → New Scene
Đặt tên: Chap01_Lighting
Lưu vào: Assets/Scenes/Chapters/Chap01/
```

### Step 3.2: Setup Hierarchy

```
Chap01_Lighting
├── Lighting
│   ├── DirectionalLight (Sun)
│   │   ├── Intensity: 1.2
│   │   ├── Color: White or slight yellow
│   │   └── Shadows: Soft (cast shadows)
│   ├── PointLights
│   │   ├── Torch_Fire_01
│   │   │   ├── Position: (5, 1, 3)
│   │   │   ├── Intensity: 2.5
│   │   │   ├── Color: Orange (#FFA500)
│   │   │   ├── Range: 15
│   │   │   └── Shadows: Soft
│   │   ├── Torch_Fire_02
│   │   ├── Lamp_Entrance
│   │   └── Lamp_Interior
│   └── SpotLights (Optional)
│       └── SpotLight_Cave (để soi sáng hang động)
├── PostProcessing
│   └── Volume
│       ├── Profile: Chap01Profile
│       └── Overrides:
│           ├── Bloom (Intensity: 2)
│           ├── Ambient Occlusion (Intensity: 0.5)
│           ├── Tonemapping (ACES)
│           └── Color Adjustments (Contrast: 1.1)
├── Fog
│   └── FogController (Script)
│       ├── FogDensity: 0.01
│       └── FogColor: Gray
├── Effects
│   ├── ParticleSystems
│   │   ├── Dust.prefab
│   │   ├── Fireflies.prefab (optional)
│   │   └── Leaves_Falling (optional)
│   └── Weather (optional)
└── LightingManager (GameObject)
    └── Script: LightingManager.cs (optional)
```

### Step 3.3: Setup Directional Light (Sun)

```
Hierarchy → Right-click → Light → Directional Light
```

**Cài đặt:**
```
Transform:
  - Rotation: X: 50, Y: -60, Z: 0
  - (Để ánh nắng từ góc 45 độ)

Light Component:
  - Type: Directional
  - Intensity: 1.2
  - Color: White (#FFFFFF)
  - Shadows:
    - Type: Soft Shadows
    - Resolution: High
    - Bias: 0.05
```

### Step 3.4: Thêm Point Lights (Torches)

```
Hierarchy → Right-click → Light → Point Light
Đặt tên: Torch_Fire_01
Position: (5, 1, 3) - tại vị trí torch từ Environment scene
```

**Cài đặt:**
```
Light Component:
  - Intensity: 2.5 (sáng)
  - Color: Orange (#FFA500)
  - Range: 15 (bán kính chiếu sáng)
  - Shadows: Soft Shadows
  - Volumetric: Enabled (optional, đẹp hơn)
```

**Lặp lại cho tất cả torches trong scene.**

### Step 3.5: Setup Post-Processing

**Kiểm tra URP được cài:**
```
Window → Rendering → Graphics Settings
→ Scriptable Render Pipeline: UniversalRenderPipelineAsset
```

**Tạo Volume:**
```
Hierarchy → Right-click → Volume → Global Volume
```

**Cài đặt:**
```
Volume Component:
  - Is Global: true (áp dụng cho toàn scene)
  - Profile: [Tạo mới] → Chap01Profile
```

**Add Overrides (Effects):**
```
Inspector → Profile → Add Override

1. Bloom
   - Intensity: 2
   - Threshold: 1

2. Ambient Occlusion
   - Enabled: true
   - Intensity: 0.5
   - Radius: 0.25

3. Tonemapping
   - Mode: ACES (cinematic)

4. Color Adjustments
   - Hue Shift: 0 (hoặc -5 để hơi lạnh)
   - Saturation: 1.1
   - Contrast: 1.1
```

### Step 3.6: Setup Fog

```
Hierarchy → Create Empty
Đặt tên: FogController
Add Component → Script: FogController.cs
```

**FogController.cs:**

```csharp
public class FogController : MonoBehaviour, IChapterSetup
{
    [SerializeField] private bool enableFog = true;
    [SerializeField] private float fogDensity = 0.01f;
    [SerializeField] private Color fogColor = Color.gray;
    
    public void OnChapterSetup(int chapterNumber)
    {
        RenderSettings.fog = enableFog;
        RenderSettings.fogDensity = fogDensity;
        RenderSettings.fogColor = fogColor;
        Debug.Log("[FogController] Fog enabled");
    }
}
```

**Cài đặt trong Inspector:**
```
Enable Fog: true
Fog Density: 0.01
Fog Color: Gray (#808080)
```

### Step 3.7: Thêm Particle Effects (Optional)

```
Hierarchy → Create Empty
Đặt tên: Effects

Thêm Particle Systems:
1. Dust particles (từ prefab)
2. Fireflies (nếu có)
3. Falling leaves (nếu cần)
```

### Step 3.8: Bake Lighting

```
Window → Rendering → Lighting
Lighting Tab:
  - Realtime Lights: Enabled
  - Baked Lights: Enabled
  - Bake Type: Baked
  
Click "Generate Lighting"
(Chờ 5-10 phút tùy độ phức tạp)
```

**Result:**
- Lighting sẽ baked vào texture
- Performance tốt hơn trong gameplay
- Shadows sẽ realtime từ directional light

### Step 3.9: Test

```
Play từ BOOTSTRAP_Main
Ấn phím 1 → Load Chap01
Kiểm tra:
  ✅ Ánh sáng tự nhiên từ sun
  ✅ Torches phát sáng da cam
  ✅ Post-processing đẹp (bloom, AO)
  ✅ Fog có hạn tầm nhìn
  ✅ Shadows sắc nét
```

---

## ✅ PHASE 4: BUILD SETTINGS & FINAL TEST

### Step 4.1: Add Scenes to Build

```
File → Build Settings → Scenes in Build

Add:
0 - Assets/Scenes/Bootstrap/BOOTSTRAP_Main
1 - Assets/Scenes/UI/UI_Common
2 - Assets/Scenes/Chapters/Chap01/Chap01_Environment
3 - Assets/Scenes/Chapters/Chap01/Chap01_Gameplay
4 - Assets/Scenes/Chapters/Chap01/Chap01_Lighting
```

### Step 4.2: Full Test

```
Play từ BOOTSTRAP_Main
Ấn phím 1 → Load Chap01

Kiểm tra toàn bộ:
  ✅ 3 scenes load additive
  ✅ Environment có terrain, buildings
  ✅ Gameplay có player, enemies, NPCs
  ✅ Lighting có sun, torches, post-processing
  ✅ Player có thể move & fight enemies
  ✅ NPCs react khi approach
  ✅ Interactables hoạt động
  ✅ BGM phát
  ✅ Save game hoạt động (ấn S)
  ✅ Pause hoạt động (ấn ESC)
  ✅ Không có lỗi trong Console
```

### Step 4.3: Performance Check

```
Window → Analysis → Profiler
Play game
Kiểm tra:
  ✅ FPS: 60+ (target)
  ✅ Memory: < 500MB
  ✅ CPU: Balanced
  ✅ GPU: Không quá cao
```

---

## 🎯 WORKFLOW SUMMARY

```
Day 1 (Environment):
  Morning   → Create scene, setup terrain, paint texture
  Afternoon → Add buildings, trees, props
  Evening   → Bake NavMesh, save

Day 2 (Gameplay):
  Morning   → Create scene, setup spawn points, add player
  Afternoon → Add enemies, NPCs, interactables
  Evening   → Create ChapGameplay script, test

Day 2-3 (Lighting):
  Morning   → Create scene, setup sun light
  Afternoon → Add point lights, post-processing
  Evening   → Bake lighting, particle effects, test

Day 3 (Polish):
  → Full test play-through
  → Performance optimization
  → Bug fixes
  → Save/load test
```

---

## 📝 Checklist Hoàn Thành Chapter

- [ ] 3 scenes tạo (Environment, Gameplay, Lighting)
- [ ] Add vào Build Settings (đúng order)
- [ ] NavMesh baked
- [ ] Player spawn tại PlayerSpawn tag
- [ ] Enemies di chuyển được
- [ ] NPCs hiện, patrol đúng
- [ ] Interactables react
- [ ] Lighting beautiful
- [ ] Post-processing applied
- [ ] BGM plays
- [ ] All 3 scenes load without errors
- [ ] Test save/load
- [ ] Test pause
- [ ] FPS 60+
- [ ] No memory leaks

---

## 🐛 Troubleshooting

| Problem | Solution |
|---------|----------|
| Enemies không di chuyển | NavMesh chưa bake hoặc NavMeshAgent chưa add |
| Player không spawn | PlayerSpawn tag chưa set |
| Interactables không react | Script chưa add hoặc Animator chưa setup |
| Lighting tối | DirectionalLight intensity quá thấp |
| Post-processing không thấy | Volume phải Global & có Profile |
| BGM không phát | Audio clip path sai hoặc volume = 0 |

---

## 🚀 Kế Tiếp

1. Hoàn thành Chap01 theo guide này
2. Copy Chap01 folder → Rename thành Chap02
3. Edit Chap02 (khác terrain, khác NPCs, etc.)
4. Lặp lại cho Chap03, 04, 05

**Mỗi chapter sau sẽ nhanh hơn vì có template!**

Good luck! 🎮✨
