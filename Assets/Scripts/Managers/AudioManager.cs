using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    private AudioSource bgmSource;
    private AudioSource sfxSource;

    [SerializeField] private float bgmVolume = 0.7f;
    [SerializeField] private float sfxVolume = 0.8f;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            AudioSource[] sources = GetComponents<AudioSource>();
            if (sources.Length >= 2)
            {
                bgmSource = sources[0];
                bgmSource.loop = true;
                sfxSource = sources[1];
                sfxSource.loop = false;
                Debug.Log("[AudioManager] Initialized");
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayBGM(AudioClip clip, float fadeInTime = 1f)
    {
        if (bgmSource == null) return;
        StartCoroutine(FadeBGM(clip, fadeInTime));
    }

    private IEnumerator FadeBGM(AudioClip clip, float fadeTime)
    {
        float elapsedTime = 0f;
        while (elapsedTime < fadeTime && bgmSource.isPlaying)
        {
            elapsedTime += Time.deltaTime;
            bgmSource.volume = Mathf.Lerp(bgmVolume, 0, elapsedTime / fadeTime);
            yield return null;
        }

        bgmSource.clip = clip;
        bgmSource.volume = 0;
        bgmSource.Play();

        elapsedTime = 0f;
        while (elapsedTime < fadeTime)
        {
            elapsedTime += Time.deltaTime;
            bgmSource.volume = Mathf.Lerp(0, bgmVolume, elapsedTime / fadeTime);
            yield return null;
        }

        bgmSource.volume = bgmVolume;
    }

    public void PlaySFX(AudioClip clip)
    {
        if (sfxSource == null) return;
        sfxSource.PlayOneShot(clip, sfxVolume);
    }
}
