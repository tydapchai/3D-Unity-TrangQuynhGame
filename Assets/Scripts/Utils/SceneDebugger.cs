using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Debug Helper - Test scene loading với keyboard shortcuts
/// </summary>
public class SceneDebugger : MonoBehaviour
{
    private void Update()
    {
        // Phím 1-5 để load chapters
        for (int i = 1; i <= 5; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha0 + i))
            {
                Debug.Log($"[SceneDebugger] Loading Chapter {i}");
                GameManager.Instance.LoadChapter(i);
            }
        }
        
        // Phím N để chapter tiếp theo
        if (Input.GetKeyDown(KeyCode.N))
        {
            Debug.Log("[SceneDebugger] Loading next chapter");
            GameManager.Instance.NextChapter();
        }
        
        // Phím R để restart
        if (Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log("[SceneDebugger] Restarting chapter");
            GameManager.Instance.RestartChapter();
        }
        
        // Phím ESC để pause
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameManager.Instance.TogglePause();
        }
        
        // Phím S để save
        if (Input.GetKeyDown(KeyCode.S))
        {
            Debug.Log("[SceneDebugger] Saving game");
            SaveManager.Instance.SaveGame();
        }
        
        // Phím L để load
        if (Input.GetKeyDown(KeyCode.L))
        {
            Debug.Log("[SceneDebugger] Loading game");
            SaveManager.Instance.LoadGame();
        }
        
        // Phím D để show debug info
        if (Input.GetKeyDown(KeyCode.D))
        {
            ShowDebugInfo();
        }
    }
    
    private void ShowDebugInfo()
    {
        Debug.Log($"\n========== DEBUG INFO ==========");
        Debug.Log($"Current Chapter: {GameManager.Instance.GetCurrentChapter()}");
        Debug.Log($"Is Paused: {GameManager.Instance.IsPaused()}");
        Debug.Log($"Scene Loading: {SceneLoadManager.Instance.IsLoading()}");
        
        string[] scenes = SceneLoadManager.Instance.GetCurrentChapterScenes();
        Debug.Log($"Loaded Scenes:");
        foreach (string scene in scenes)
        {
            Debug.Log($"  - {scene}");
        }
        
        Debug.Log($"Played Time: {SaveManager.Instance.GetPlayedTime()}s");
        Debug.Log($"================================\n");
    }
}
