using UnityEngine;

public class Clue : MonoBehaviour
{
    [SerializeField] private string clueDescription;
    [TextArea(2, 4)]
    [SerializeField] private string clueDetails;

    public string ClueDescription => clueDescription;
    public string ClueDetails => clueDetails;

    void Start()
    {
        Debug.Log($"[Clue] Found: {clueDescription}");
    }

    public void Examine()
    {
        Debug.Log($"[Clue] Examined: {clueDescription}");
        Debug.Log($"[Clue] Details: {clueDetails}");
    }

    public void HighlightClue()
    {
        // Visual feedback when clue is examined
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material.color = Color.yellow;
        }

        Debug.Log($"[Clue] Highlighting: {clueDescription}");
    }

    public void UnhighlightClue()
    {
        // Remove visual feedback
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material.color = Color.white;
        }
    }
}
