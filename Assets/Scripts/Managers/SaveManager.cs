using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Save Manager - Quản lý save/load game
/// </summary>
[System.Serializable]
public class SaveData
{
    public int currentChapter = 1;
    public int playedTime = 0; // seconds
    public int playerLevel = 1;
    public float playerHP = 100f;
    public List<string> unlockedItems = new List<string>();
    public bool[] completedChapters = new bool[5];
}

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { get; private set; }
    
    private SaveData currentSave;
    private string savePath;
    
    private const string SAVE_FILE = "gamesave.json";

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log($"[SaveManager] Initialized");
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        savePath = System.IO.Path.Combine(Application.persistentDataPath, SAVE_FILE);
        LoadGame();
    }

    /// <summary>
    /// Tải game từ file
    /// </summary>
    public void LoadGame()
    {
        if (System.IO.File.Exists(savePath))
        {
            string json = System.IO.File.ReadAllText(savePath);
            currentSave = JsonUtility.FromJson<SaveData>(json);
            Debug.Log($"[SaveManager] Game loaded from {savePath}");
            Debug.Log($"[SaveManager] Current chapter: {currentSave.currentChapter}");
        }
        else
        {
            currentSave = new SaveData();
            Debug.Log("[SaveManager] No save file found, creating new game");
        }
    }

    /// <summary>
    /// Lưu game vào file
    /// </summary>
    public void SaveGame()
    {
        if (currentSave == null)
            currentSave = new SaveData();
        
        // Update current data
        if (GameManager.Instance != null)
        {
            currentSave.currentChapter = GameManager.Instance.GetCurrentChapter();
        }
        
        string json = JsonUtility.ToJson(currentSave, true);
        System.IO.File.WriteAllText(savePath, json);
        
        Debug.Log($"[SaveManager] Game saved to {savePath}");
    }

    /// <summary>
    /// Xóa save file
    /// </summary>
    public void DeleteSave()
    {
        if (System.IO.File.Exists(savePath))
        {
            System.IO.File.Delete(savePath);
            currentSave = new SaveData();
            Debug.Log("[SaveManager] Save deleted");
        }
    }

    // Getters
    public SaveData GetCurrentSave() => currentSave;
    public int GetCurrentChapter() => currentSave.currentChapter;
    public int GetPlayedTime() => currentSave.playedTime;
    
    // Setters
    public void SetCurrentChapter(int chapter)
    {
        currentSave.currentChapter = chapter;
    }

    public void CompleteChapter(int chapterNumber)
    {
        if (chapterNumber >= 0 && chapterNumber < currentSave.completedChapters.Length)
        {
            currentSave.completedChapters[chapterNumber] = true;
            SaveGame();
            Debug.Log($"[SaveManager] Chapter {chapterNumber} marked as completed");
        }
    }

    public bool IsChapterCompleted(int chapterNumber)
    {
        if (chapterNumber >= 0 && chapterNumber < currentSave.completedChapters.Length)
        {
            return currentSave.completedChapters[chapterNumber];
        }
        return false;
    }
}
