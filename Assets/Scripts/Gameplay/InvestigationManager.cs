using UnityEngine;
using System.Collections.Generic;

public class InvestigationManager : MonoBehaviour
{
    public static InvestigationManager Instance { get; private set; }

    private List<Suspect> suspects = new List<Suspect>();
    private List<Clue> clues = new List<Clue>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("[InvestigationManager] Initialized");
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        FindAllSuspectsAndClues();
    }

    private void FindAllSuspectsAndClues()
    {
        // Find all suspects in scene
        suspects.AddRange(FindObjectsByType<Suspect>(FindObjectsSortMode.None));
        Debug.Log($"[InvestigationManager] Found {suspects.Count} suspects");

        // Find all clues in scene
        clues.AddRange(FindObjectsByType<Clue>(FindObjectsSortMode.None));
        Debug.Log($"[InvestigationManager] Found {clues.Count} clues");
    }

    public Suspect GetSuspectByName(string name)
    {
        foreach (var suspect in suspects)
        {
            if (suspect.SuspectName == name)
                return suspect;
        }

        Debug.LogWarning($"[InvestigationManager] Suspect '{name}' not found");
        return null;
    }

    public List<Clue> GetAllClues()
    {
        return clues;
    }

    public bool IsGuiltyCorrect(string suspectName)
    {
        Suspect suspect = GetSuspectByName(suspectName);
        if (suspect != null)
        {
            return suspect.IsGuilty;
        }

        return false;
    }

    public void ResetInvestigation()
    {
        Debug.Log("[InvestigationManager] Investigation reset");
        // Reset any investigation state if needed
    }
}
