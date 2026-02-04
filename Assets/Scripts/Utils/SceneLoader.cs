using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    void Start()
    {
        // Load UI_Common additive khi game start
        SceneManager.LoadSceneAsync("UI_Common", LoadSceneMode.Additive);
    }
}
