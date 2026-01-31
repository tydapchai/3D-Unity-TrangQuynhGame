# 🤝 Collaboration Rules - Quy Tắc Làm Việc Nhóm

**Mục đích:** Định nghĩa quy tắc làm việc để tránh conflict khi multiple developers làm cùng 1 project.

---

## 📋 Nguyên Tắc Cơ Bản

### 1. **One Developer, One Scene**
- **Mỗi dev chỉ chịu trách nhiệm 1 scene hoặc 1 phần cụ thể**
- Không được chỉnh sửa scene của dev khác mà không thông báo
- Nếu cần thay đổi scene của người khác, phải **thông báo trước** qua Slack/Discord

**Ví dụ:**
```
Dev A → Chap01_Environment
Dev B → Chap01_Gameplay
Dev C → Chap01_Lighting
Dev D → UI/Prefabs
```

### 2. **Use Prefabs, Don't Edit Instances**
- Luôn dùng **Prefabs** để tạo objects trong scene
- **KHÔNG** chỉnh sửa trực tiếp instances trong scene (vì sẽ overwrite prefab)
- Nếu cần thay đổi, sửa **Prefab file** rồi apply lại

**Lỗi thường mắc:**
```
❌ WRONG: Sửa Enemy_Goblin.prefab → Instance trong scene mất sync
✅ RIGHT: Sửa prefab file → Tất cả instances tự động cập nhật
```

### 3. **Commit Frequently, Small Commits**
- **Commit 2-3 lần/ngày** thay vì 1 lần lớn
- Mỗi commit nên **liên quan đến 1 feature/fix cụ thể**
- Viết **clear commit messages** (bằng tiếng Anh hoặc Tiếng Việt rõ ràng)

**Commit Message Format:**
```
[ChapXX] Add Enemy prefab with AI behavior
[ChapXX] Setup environment terrain and props
[Prefabs] Create NPC_Merchant with dialogue system
[UI] Fix pause menu button layout
```

### 4. **Pull Before You Push**
- Luôn **pull latest changes** trước khi push
- Nếu có conflict, **hãy resolve trước** (không merge blindly)

```bash
git pull origin main  # Pull latest
git add <files>       # Stage changes
git commit -m "..."   # Commit
git push origin       # Push
```

---

## 📁 Folder & File Ownership

### **Assets/Scenes/**
```
Scenes/Bootstrap/
├── BOOTSTRAP_Main.unity         → Dev C (Manager setup)

Scenes/UI/
├── UI_Common.unity              → Dev D (UI/HUD)

Scenes/Chapters/Chap01/
├── Chap01_Environment.unity     → Dev A (Terrain, props)
├── Chap01_Gameplay.unity        → Dev B (Player, enemies, NPCs)
└── Chap01_Lighting.unity        → Dev C (Lights, post-processing)

Scenes/Chapters/Chap02/ ... Chap05/
├── ChapXX_Environment.unity     → Dev A
├── ChapXX_Gameplay.unity        → Dev B
└── ChapXX_Lighting.unity        → Dev C
```

### **Assets/Prefabs/**
```
Prefabs/
├── Managers/
│   └── ManagersBundle.prefab    → Dev C (READONLY, only update when needed)

├── Characters/
│   ├── Player.prefab            → Dev B
│   ├── BananaMan.prefab         → Dev B (Character model)
│   └── RegalElegance.prefab     → Dev B

├── Enemies/
│   ├── Enemy_Goblin.prefab      → Dev B
│   ├── Enemy_Orc.prefab         → Dev B
│   └── Enemy_Boss.prefab        → Dev B

├── NPCs/
│   ├── NPC_Merchant.prefab      → Dev B
│   └── NPC_Guard.prefab         → Dev B

├── Interactables/
│   ├── Chest.prefab             → Dev B
│   ├── Door.prefab              → Dev B
│   └── Lever.prefab             → Dev B

└── UI/
    ├── HUD.prefab               → Dev D
    ├── PauseMenu.prefab         → Dev D
    └── DialogueBox.prefab       → Dev D
```

### **Assets/Scripts/**
```
Scripts/
├── Managers/
│   ├── GameManager.cs           → Dev C (Core logic)
│   ├── SceneLoadManager.cs      → Dev C
│   ├── AudioManager.cs          → Dev C
│   └── SaveManager.cs           → Dev C

├── Gameplay/
│   ├── Enemy.cs                 → Dev B
│   ├── NPC.cs                   → Dev B
│   └── Interactable.cs          → Dev B

├── Player/
│   └── Player.cs                → Dev B

├── UI/
│   ├── HUDManager.cs            → Dev D
│   ├── PauseMenuManager.cs      → Dev D
│   └── DialogueManager.cs       → Dev D

└── Utils/
    └── SceneDebugger.cs         → Dev C (Shared utils)
```

---

## 🔄 Git Workflow

### **Branch Strategy**

```
main (STABLE - chỉ merge khi hoàn thành)
├── dev (INTEGRATION - test mọi thứ)
├── feature/chap01-environment (Dev A)
├── feature/chap01-gameplay (Dev B)
├── feature/chap01-lighting (Dev C)
└── feature/ui-menus (Dev D)
```

### **Pull Request (PR) Process**

1. **Tạo branch từ `dev`:**
```bash
git checkout dev
git pull origin dev
git checkout -b feature/chap01-environment
```

2. **Làm việc trên branch:**
```bash
# ... edit files ...
git add <files>
git commit -m "[Chap01] Add terrain and buildings"
git commit -m "[Chap01] Bake NavMesh"
```

3. **Push và tạo PR:**
```bash
git push origin feature/chap01-environment
# Vào GitHub → Create Pull Request → assign reviewer
```

4. **Code Review:**
- Ít nhất 1 dev khác phải review trước merge
- Kiểm tra: conflict, lỗi, best practices

5. **Merge vào `dev`:**
```
PR → Approve → Merge to dev
```

6. **Merge `dev` vào `main` khi hoàn thành chapter:**
```bash
git checkout main
git pull origin main
git merge dev
git push origin main
```

---

## ⚡ DONT's - Điều Cấm Kỵ

### ❌ Không được:

1. **Chỉnh sửa file của người khác mà không thông báo**
   ```
   ❌ Dev A sửa script của Dev B → Conflict!
   ✅ Dev A ping Dev B trước: "Tôi cần sửa NPC.cs"
   ```

2. **Merge conflict blindly**
   ```bash
   ❌ git merge -X ours  # Lấy version của mình
   ✅ Resolve conflict thủ công + test
   ```

3. **Commit trực tiếp lên `main` hoặc `dev`**
   ```bash
   ❌ git checkout main && git commit ...
   ✅ Tạo branch feature + PR + review
   ```

4. **Push 1 scene lớn mà không test trước**
   ```bash
   ❌ Chỉnh sửa 100 objects → Commit → Push
   ✅ Commit 10-20 objects/lần → Test → Push
   ```

5. **Sửa prefab instance trong scene**
   ```
   ❌ Edit Enemy_01 instance → Overwrite prefab
   ✅ Edit prefab file → Apply to all instances
   ```

6. **Để Unity auto-merge .unity files**
   ```
   ❌ Let Unity merge scene files
   ✅ Communicate with team before merging scenes
   ```

---

## 📝 Naming Conventions

### **Scenes**
```
✅ Chap01_Environment.unity
✅ Chap02_Gameplay.unity
❌ Chapter1Environment.unity
❌ ch1_env.unity
```

### **Prefabs**
```
✅ Enemy_Goblin.prefab
✅ NPC_Merchant.prefab
✅ UI_PauseMenu.prefab
❌ enemy.prefab
❌ EnemyGoblin.prefab
```

### **Scripts**
```
✅ GameManager.cs
✅ Enemy.cs
✅ IChapterSetup.cs (interfaces)
❌ gameMgr.cs
❌ game_manager.cs
```

### **Folders**
```
✅ Assets/Prefabs/Enemies/
✅ Assets/Scripts/Managers/
✅ Assets/Art/Characters/
❌ Assets/prefabs/
❌ Assets/Scripts/gameplay/
```

---

## 🎯 Conflict Resolution Guide

### **Scenario 1: Merge Conflict in `.cs` file**

```bash
# Dev B pulls and sees conflict in NPC.cs
git status
# → both modified: Assets/Scripts/Gameplay/NPC.cs

# Open NPC.cs and find conflict markers:
<<<<<<< HEAD
    // Dev B's code
    public void Talk() { ... }
=======
    // Dev A's code
    public void Interact() { ... }
>>>>>>> feature/chap01-gameplay

# Resolve:
# 1. If independent → Keep both
# 2. If conflicting → Talk to Dev A which one is correct
# 3. Remove conflict markers
# 4. Test the code

git add Assets/Scripts/Gameplay/NPC.cs
git commit -m "Resolve conflict in NPC.cs"
```

### **Scenario 2: Merge Conflict in `.unity` scene file**

```
⚠️ DO NOT AUTO-MERGE!
```

**Option A: One dev takes ownership**
```bash
# If you own this scene, take your version:
git checkout --theirs Assets/Scenes/Chapters/Chap01/Chap01_Gameplay.unity
git add ...
git commit -m "Resolve scene conflict - take Chap01_Gameplay"
```

**Option B: Re-do changes manually**
```
1. Dev A & Dev B open the scene
2. Compare in Unity Editor
3. Re-apply changes manually
4. Save & commit
```

**Best Practice:**
```
→ Communicate BEFORE merge
→ Take turns editing same scene
→ Don't both edit 1 scene simultaneously
```

### **Scenario 3: Prefab Instance Mismatch**

```
Error: "Prefab instance is newer than original asset"

Fix:
1. Open the prefab instance
2. Right-click → Prefab → Overwrite
   (ONLY if you made intentional changes to instance)

OR

1. Right-click instance → Revert
   (If you made mistakes)
```

---

## 📅 Daily Standup Checklist

**Mỗi ngày, team nên check:**

- [ ] Assigned to you?
- [ ] Any blockers?
- [ ] Need help from another dev?
- [ ] Ready to push?

**Example Slack message:**
```
🎮 Chap01 Status:
✅ Dev A: Terrain done, NavMesh baked. Pushing today.
✅ Dev B: Enemy prefabs 80% done, fixing AI pathfinding.
🔄 Dev C: Waiting for terrain from Dev A to add lighting.
⚠️ Dev D: Need character model from Design team.
```

---

## 🔐 Critical Files - READ ONLY

```
❌ Không edit trực tiếp:
├── Assets/Prefabs/Managers/ManagersBundle.prefab
├── Assets/Scripts/Managers/*.cs (unless authorized)
├── .gitignore
├── QUICK_START.md (edit cùng lúc = conflict)
└── README.md

✅ Nếu cần edit, create PR + ask lead dev
```

---

## 🚀 Workflow Example - Ngày 1 của Dev Team

```
Morning (9:00 AM):
├── All: Pull latest main
├── Dev A: Create feature/chap01-environment branch
├── Dev B: Create feature/chap01-gameplay branch
├── Dev C: Create feature/chap01-lighting branch
└── Dev D: Create feature/ui-setup branch

Afternoon (12:00 PM):
├── Dev A: Add terrain, test locally
├── Dev B: Setup player spawn, enemies
├── Dev C: Waiting for terrain from A
└── Dev D: UI prefabs

Evening (5:00 PM):
├── Dev A: Commit terrain + NavMesh, push to feature branch
├── Dev B: Commit enemies, push to feature branch
├── Dev C: Pull terrain, add lighting, push
└── Dev D: Push UI prefabs

Before 6:00 PM:
├── All: Create PRs
├── All: Review each other's PRs
└── All: Merge to dev branch

Next day:
├── All: Pull latest dev
└── Continue working...
```

---

## 📞 Communication Channels

### **For Different Issues:**

| Issue | Channel | Response Time |
|-------|---------|----------------|
| Quick question | Slack #chat | Real-time |
| Need code review | GitHub PR | Same day |
| Blocking issue | Discord voice | 5 min |
| Scene conflict | Video call | Immediate |
| New feature idea | Email + meeting | 1 day |

### **Daily Check-ins:**
- **10:00 AM** - Team standup (15 min)
- **3:00 PM** - Status update on Slack

---

## ✅ Pre-Push Checklist

Before pushing to GitHub:

```
[ ] Pulled latest changes
[ ] All scripts compile without errors
[ ] Tested locally (play-through)
[ ] No console errors/warnings
[ ] Committed with clear message
[ ] Removed debug logs (if any)
[ ] Updated relevant documentation
[ ] Checked .gitignore (no personal files)
[ ] Ready for code review
```

---

## 🎯 Best Practices

### 1. **Use Locks for Large Assets**
```
Working on big terrain? Notify team:
→ "I'm working on Chap01_Environment until 5 PM"
→ Others avoid this scene
```

### 2. **Test Before Pushing**
```
✅ Play game locally
✅ Test all 3 chapter scenes load
✅ Check for console errors
✅ Verify no regressions
```

### 3. **Document Your Changes**
```
Commit message example:
[Chap01] Add 5 enemy spawns with AI pathfinding

- Created 5 enemy spawn points in Chap01_Gameplay
- Added NavMeshAgent to each enemy
- Tested pathfinding with terrain NavMesh
- Performance: 60 FPS with 5 enemies active
```

### 4. **Keep Branches Updated**
```bash
# Regularly sync with dev
git fetch origin
git rebase origin/dev
```

### 5. **Review Others' Code**
```
Take 10 min each PR to review
→ Catches bugs early
→ Team learns from each other
```

---

## 🚨 Emergency Situations

### **Someone Pushed Breaking Code**
```bash
git log --oneline  # Find the bad commit
git revert <commit-hash>
git push origin dev
→ Notify team on Slack
```

### **Accidental Commit to Main**
```bash
git revert <commit-hash>  # Revert the change
git push origin main
→ Fix on branch, create PR, merge properly
```

### **Large Merge Conflict**
```
→ Call team meeting (video)
→ Both devs sit together
→ Resolve conflict with context
→ Test thoroughly before merge
```

---

## 📚 Additional Resources

- [Git Collaboration Guide](https://git-scm.com/book/en/v2/Git-Branching-Branching-Workflows)
- [Unity Collaboration Best Practices](https://docs.unity3d.com/Manual/BestPracticeGuides.html)
- [Semantic Commits](https://www.conventionalcommits.org/)

---

## ✍️ Checklist Khi Onboard Dev Mới

Khi có dev mới join team:

- [ ] Clone repo
- [ ] Setup local environment
- [ ] Read QUICK_START.md
- [ ] Read COLLABORATION_RULES.md (file này)
- [ ] Attend setup walkthrough (1 hour)
- [ ] Create first branch + do PR practice
- [ ] Assigned first task (small)
- [ ] Pair program với 1 dev (1 hour)

---

**Last Updated:** January 31, 2026
**Status:** Active
**Maintainer:** Team Lead

---

## 🎮 Let's Build This Game Together! 🚀

Tuân thủ quy tắc này → Ít conflict → Hạnh phúc team → Tốc độ development nhanh!

Nếu có câu hỏi, hãy liên hệ Team Lead hoặc discuss trên #dev-chat.
