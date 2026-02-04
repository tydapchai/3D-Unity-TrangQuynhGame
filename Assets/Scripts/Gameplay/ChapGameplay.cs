using UnityEngine;

public class ChapGameplay : MonoBehaviour, IChapterSetup
{
    [SerializeField] private AudioClip chapBGM;

    public void OnChapterSetup(int chapterNumber)
    {
        Debug.Log($"[ChapGameplay] Chapter {chapterNumber} setup");

        if (chapBGM != null)
        {
            AudioManager.Instance.PlayBGM(chapBGM);
        }
    }
}
