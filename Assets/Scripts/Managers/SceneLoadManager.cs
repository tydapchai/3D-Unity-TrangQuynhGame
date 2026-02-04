using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneLoadManager : MonoBehaviour
{
    public static SceneLoadManager Instance { get; private set; }

    private int currentLoadedChapter = -1;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("[SceneLoadManager] Initialized");
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public IEnumerator LoadChapter(int chapterNumber)
    {
        if (currentLoadedChapter != -1)
        {
            yield return UnloadChapter(currentLoadedChapter);
        }

        string envScene = $"Chap{chapterNumber:D2}_Environment";
        string gameScene = $"Chap{chapterNumber:D2}_Gameplay";
        string lightScene = $"Chap{chapterNumber:D2}_Lighting";

        yield return LoadSceneAdditive(envScene);
        yield return LoadSceneAdditive(gameScene);
        yield return LoadSceneAdditive(lightScene);

        currentLoadedChapter = chapterNumber;

        // Hide loading screen
        HideLoadingScreen();

        // Find all MonoBehaviours implementing IChapterSetup
        MonoBehaviour[] allObjects = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None);
        foreach (var obj in allObjects)
        {
            if (obj is IChapterSetup setupObject)
            {
                setupObject.OnChapterSetup(chapterNumber);
            }
        }

        Debug.Log($"[SceneLoadManager] Chapter {chapterNumber} fully loaded");
    }

    private void HideLoadingScreen()
    {
        // Find and hide loading screen canvas
        Canvas[] canvases = FindObjectsByType<Canvas>(FindObjectsSortMode.None);
        foreach (var canvas in canvases)
        {
            if (canvas.name.Contains("Loading") || canvas.name.Contains("loading"))
            {
                canvas.gameObject.SetActive(false);
                Debug.Log("[SceneLoadManager] Loading screen hidden");
                return;
            }
        }

        // If not found by name, try to find any canvas with LoadingScreen panel
        foreach (var canvas in canvases)
        {
            Transform loadingPanel = canvas.transform.Find("LoadingScreen");
            if (loadingPanel != null)
            {
                loadingPanel.gameObject.SetActive(false);
                Debug.Log("[SceneLoadManager] Loading panel hidden");
                return;
            }
        }
    }

    private IEnumerator LoadSceneAdditive(string sceneName)
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        yield return new WaitUntil(() => op.isDone);
        Debug.Log($"[SceneLoadManager] Scene '{sceneName}' loaded additive");
    }

    private IEnumerator UnloadChapter(int chapterNumber)
    {
        string envScene = $"Chap{chapterNumber:D2}_Environment";
        string gameScene = $"Chap{chapterNumber:D2}_Gameplay";
        string lightScene = $"Chap{chapterNumber:D2}_Lighting";

        yield return UnloadSceneAdditive(envScene);
        yield return UnloadSceneAdditive(gameScene);
        yield return UnloadSceneAdditive(lightScene);

        Debug.Log($"[SceneLoadManager] Chapter {chapterNumber} unloaded");
    }

    private IEnumerator UnloadSceneAdditive(string sceneName)
    {
        AsyncOperation op = SceneManager.UnloadSceneAsync(sceneName);
        yield return new WaitUntil(() => op.isDone);
        Debug.Log($"[SceneLoadManager] Scene '{sceneName}' unloaded");
    }
}

public interface IChapterSetup
{
    void OnChapterSetup(int chapterNumber);
}
