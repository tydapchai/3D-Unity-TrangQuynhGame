using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class BootstrapInitializer : MonoBehaviour
{
    void Start()
    {
        Debug.Log("[BootstrapInitializer] Loading UI_Common scene...");
        StartCoroutine(LoadUICommon());
    }

    private IEnumerator LoadUICommon()
    {
        AsyncOperation op = SceneManager.LoadSceneAsync("UI_Common", LoadSceneMode.Additive);
        yield return new WaitUntil(() => op.isDone);
        Debug.Log("[BootstrapInitializer] UI_Common scene loaded!");

        // Find Chapter1_Home_UI (which IS the Canvas)
        GameObject homeUI = GameObject.Find("Chapter1_Home_UI");
        if (homeUI != null)
        {
            homeUI.SetActive(false);
            Debug.Log("[BootstrapInitializer] Chapter1_Home_UI disabled initially");
        }
        else
        {
            Debug.LogWarning("[BootstrapInitializer] Chapter1_Home_UI not found to disable!");
        }
    }
}



