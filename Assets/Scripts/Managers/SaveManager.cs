using UnityEngine;
using System.IO;

[System.Serializable]
public class SaveData
{
    public int currentChapter = 1;
    public bool[] completedChapters = new bool[6];
}

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { get; private set; }

    private SaveData saveData;
    private string savePath;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            savePath = Application.persistentDataPath + "/savegame.json";
            LoadGame();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SaveGame()
    {
        if (saveData == null)
            return;

        if (GameManager.Instance != null)
        {
            saveData.currentChapter = GameManager.Instance.CurrentChapter;
        }

        string json = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(savePath, json);
        Debug.Log("[SaveManager] Game saved");
    }

    public void LoadGame()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            saveData = JsonUtility.FromJson<SaveData>(json);
            Debug.Log("[SaveManager] Game loaded");
        }
        else
        {
            saveData = new SaveData { currentChapter = 1 };
            Debug.Log("[SaveManager] No save file found, creating new save data");
        }
    }

    public void CompleteChapter(int chapterNumber)
    {
        if (chapterNumber >= 1 && chapterNumber <= 5)
        {
            saveData.completedChapters[chapterNumber] = true;
            SaveGame();
        }
    }
}
