using UnityEngine;
using UnityEngine.InputSystem;

public class SceneDebugger : MonoBehaviour
{
    void Update()
    {
        // Load chapters 1-5
        if (Keyboard.current.digit1Key.wasPressedThisFrame)
            GameManager.Instance.LoadChapter(1);

        if (Keyboard.current.digit2Key.wasPressedThisFrame)
            GameManager.Instance.LoadChapter(2);

        if (Keyboard.current.digit3Key.wasPressedThisFrame)
            GameManager.Instance.LoadChapter(3);

        if (Keyboard.current.digit4Key.wasPressedThisFrame)
            GameManager.Instance.LoadChapter(4);

        if (Keyboard.current.digit5Key.wasPressedThisFrame)
            GameManager.Instance.LoadChapter(5);

        // Next chapter
        if (Keyboard.current.nKey.wasPressedThisFrame)
            GameManager.Instance.NextChapter();

        // Restart chapter
        if (Keyboard.current.rKey.wasPressedThisFrame)
            GameManager.Instance.RestartChapter();

        // Toggle pause
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
            GameManager.Instance.TogglePause();

        // Save game
        if (Keyboard.current.sKey.wasPressedThisFrame)
            SaveManager.Instance.SaveGame();

        // Load game
        if (Keyboard.current.lKey.wasPressedThisFrame)
            SaveManager.Instance.LoadGame();

        // Debug info
        if (Keyboard.current.dKey.wasPressedThisFrame)
            Debug.Log($"Chapter: {GameManager.Instance.CurrentChapter}, Paused: {GameManager.Instance.IsPaused}");
    }
}
