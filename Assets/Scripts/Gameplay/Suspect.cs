using UnityEngine;

public class Suspect : MonoBehaviour
{
    [SerializeField] private string suspectName;
    [SerializeField] private bool isGuilty = false;

    public string SuspectName => suspectName;
    public bool IsGuilty => isGuilty;

    void Start()
    {
        Debug.Log($"[Suspect] {suspectName} initialized. Guilty: {isGuilty}");
    }

    public void Question()
    {
        Debug.Log($"[Suspect] Questioning {suspectName}...");

        if (isGuilty)
        {
            Debug.Log($"[Suspect] {suspectName} is suspicious and nervous!");
        }
        else
        {
            Debug.Log($"[Suspect] {suspectName} claims innocence");
        }
    }

    public void ShowClues()
    {
        Debug.Log($"[Suspect] Showing clues for {suspectName}");
    }
}
