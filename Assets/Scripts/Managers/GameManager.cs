using UnityEngine;
using System;

/// <summary>
/// Quản lý trạng thái game, chapter hiện tại, save/load
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    [SerializeField] private int currentChapter = 1;
    [SerializeField] private int maxChapters = 5;
    
    // Events
    public static Action<int> OnChapterLoaded;
    public static Action<int> OnChapterUnloaded;
    public static Action OnGamePaused;
    public static Action OnGameResumed;
    
    private bool isPaused = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log($"[GameManager] Initialized");
        }
        else
        {
            Debug.LogWarning($"[GameManager] Duplicate instance destroyed");
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Load lần đầu
        LoadChapter(currentChapter);
    }

    private void Update()
    {
        // Ví dụ: Pause game với ESC
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    /// <summary>
    /// Load một chapter cụ thể
    /// </summary>
    public void LoadChapter(int chapterNumber)
    {
        if (chapterNumber < 1 || chapterNumber > maxChapters)
        {
            Debug.LogError($"[GameManager] Invalid chapter: {chapterNumber}");
            return;
        }

        currentChapter = chapterNumber;
        SceneLoadManager.Instance.LoadChapter(chapterNumber);
        OnChapterLoaded?.Invoke(chapterNumber);
        
        Debug.Log($"[GameManager] Loaded chapter {chapterNumber}");
    }

    /// <summary>
    /// Chuyển đến chapter tiếp theo
    /// </summary>
    public void NextChapter()
    {
        if (currentChapter < maxChapters)
        {
            LoadChapter(currentChapter + 1);
        }
        else
        {
            Debug.Log($"[GameManager] Game completed!");
            // TODO: Show ending screen
        }
    }

    /// <summary>
    /// Restart chapter hiện tại
    /// </summary>
    public void RestartChapter()
    {
        LoadChapter(currentChapter);
    }

    /// <summary>
    /// Toggle pause
    /// </summary>
    public void TogglePause()
    {
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0f : 1f;
        
        if (isPaused)
            OnGamePaused?.Invoke();
        else
            OnGameResumed?.Invoke();
        
        Debug.Log($"[GameManager] Game {(isPaused ? "paused" : "resumed")}");
    }

    public int GetCurrentChapter() => currentChapter;
    public bool IsPaused() => isPaused;
    public int GetMaxChapters() => maxChapters;
}
