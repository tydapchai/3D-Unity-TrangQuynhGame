using UnityEngine;

public class FogController : MonoBehaviour, IChapterSetup
{
    [SerializeField] private bool enableFog = true;
    [SerializeField] private float fogDensity = 0.01f;

    public void OnChapterSetup(int chapterNumber)
    {
        RenderSettings.fog = enableFog;
        RenderSettings.fogDensity = fogDensity;
        Debug.Log("[FogController] Configured");
    }
}
