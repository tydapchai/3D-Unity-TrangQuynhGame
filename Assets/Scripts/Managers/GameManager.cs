using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public int CurrentChapter { get; private set; } = 1;
    public bool IsPaused { get; private set; } = false;

    public static event Action<int> OnChapterLoaded;
    public static event Action OnGamePaused;
    public static event Action OnGameResumed;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("[GameManager] Initialized");
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadChapter(int chapterNumber)
    {
        CurrentChapter = chapterNumber;
        Debug.Log($"[GameManager] Loading Chapter {chapterNumber}");
        StartCoroutine(SceneLoadManager.Instance.LoadChapter(chapterNumber));
        OnChapterLoaded?.Invoke(chapterNumber);
    }

    public void NextChapter()
    {
        LoadChapter(CurrentChapter + 1);
    }

    public void RestartChapter()
    {
        LoadChapter(CurrentChapter);
    }

    public void TogglePause()
    {
        IsPaused = !IsPaused;
        Time.timeScale = IsPaused ? 0f : 1f;

        if (IsPaused)
        {
            OnGamePaused?.Invoke();
            Debug.Log("[GameManager] Game Paused");
        }
        else
        {
            OnGameResumed?.Invoke();
            Debug.Log("[GameManager] Game Resumed");
        }
    }
}
