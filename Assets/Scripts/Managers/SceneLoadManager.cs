using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System;

/// <summary>
/// Quản lý loading/unloading scenes theo cấu trúc chapter
/// </summary>
public class SceneLoadManager : MonoBehaviour
{
    public static SceneLoadManager Instance { get; private set; }
    
    private string[] currentChapterScenes = new string[3];
    private bool isLoading = false;
    
    // Events
    public Action OnLoadingStarted;
    public Action OnLoadingCompleted;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log($"[SceneLoadManager] Initialized");
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Load chapter với 3 scenes
    /// </summary>
    public void LoadChapter(int chapterNumber)
    {
        if (isLoading)
        {
            Debug.LogWarning("[SceneLoadManager] Already loading");
            return;
        }
        
        StartCoroutine(LoadChapterCoroutine(chapterNumber));
    }

    private IEnumerator LoadChapterCoroutine(int chapterNumber)
    {
        isLoading = true;
        OnLoadingStarted?.Invoke();
        
        // Unload chapter cũ
        if (!string.IsNullOrEmpty(currentChapterScenes[0]))
        {
            yield return UnloadChapterScenes();
        }
        
        // Tạo scene names
        string chapName = $"Chap{chapterNumber:D2}";
        string[] scenesToLoad = new string[3]
        {
            $"{chapName}_Environment",
            $"{chapName}_Gameplay",
            $"{chapName}_Lighting"
        };
        
        // Load 3 scenes additively
        for (int i = 0; i < scenesToLoad.Length; i++)
        {
            yield return LoadSceneAsync(scenesToLoad[i], LoadSceneMode.Additive);
            currentChapterScenes[i] = scenesToLoad[i];
        }
        
        // Setup camera & player spawn
        yield return new WaitForEndOfFrame();
        SetupChapter(chapterNumber);
        
        isLoading = false;
        OnLoadingCompleted?.Invoke();
        
        Debug.Log($"[SceneLoadManager] Chapter {chapterNumber} fully loaded");
    }

    private IEnumerator UnloadChapterScenes()
    {
        Debug.Log("[SceneLoadManager] Unloading previous chapter...");
        
        foreach (string sceneName in currentChapterScenes)
        {
            if (!string.IsNullOrEmpty(sceneName))
            {
                yield return SceneManager.UnloadSceneAsync(sceneName);
            }
        }
        
        System.Array.Clear(currentChapterScenes, 0, currentChapterScenes.Length);
    }

    private IEnumerator LoadSceneAsync(string sceneName, LoadSceneMode mode)
    {
        Debug.Log($"[SceneLoadManager] Loading {sceneName}...");
        
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, mode);
        
        // Có thể show loading bar ở đây
        while (!asyncLoad.isDone)
        {
            // Progress: asyncLoad.progress (0.0 - 0.9)
            yield return null;
        }
        
        Debug.Log($"[SceneLoadManager] {sceneName} loaded");
    }

    private void SetupChapter(int chapterNumber)
    {
        // Tìm player spawn point
        GameObject spawnPoint = GameObject.FindGameObjectWithTag("PlayerSpawn");
        if (spawnPoint != null)
        {
            Player player = UnityEngine.Object.FindFirstObjectByType<Player>();
            if (player != null)
            {
                player.transform.position = spawnPoint.transform.position;
                player.transform.rotation = spawnPoint.transform.rotation;
                Debug.Log($"[SceneLoadManager] Player spawned at {spawnPoint.name}");
            }
        }
        
        // Trigger chapter-specific setup
        MonoBehaviour[] allObjects = UnityEngine.Object.FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None);
        foreach (var obj in allObjects)
        {
            if (obj is IChapterSetup setup)
            {
                setup.OnChapterSetup(chapterNumber);
            }
        }
    }

    public bool IsLoading() => isLoading;
    
    public string[] GetCurrentChapterScenes() => currentChapterScenes;
}

/// <summary>
/// Interface cho các objects cần setup khi chapter load
/// </summary>
public interface IChapterSetup
{
    void OnChapterSetup(int chapterNumber);
}
