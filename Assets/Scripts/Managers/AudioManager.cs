using UnityEngine;

/// <summary>
/// Audio Manager - Quản lý BGM, SFX
/// </summary>
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }
    
    [SerializeField] private AudioSource bgmSource;
    [SerializeField] private AudioSource sfxSource;
    
    [SerializeField] [Range(0f, 1f)] private float bgmVolume = 0.7f;
    [SerializeField] [Range(0f, 1f)] private float sfxVolume = 0.8f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log($"[AudioManager] Initialized");
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Tìm audio sources nếu chưa assign
        if (bgmSource == null)
        {
            bgmSource = gameObject.AddComponent<AudioSource>();
            bgmSource.loop = true;
        }
        
        if (sfxSource == null)
        {
            sfxSource = gameObject.AddComponent<AudioSource>();
            sfxSource.loop = false;
        }
        
        SetBGMVolume(bgmVolume);
        SetSFXVolume(sfxVolume);
    }

    /// <summary>
    /// Phát nhạc nền
    /// </summary>
    public void PlayBGM(AudioClip clip, float fadeInTime = 0.5f)
    {
        if (clip == null)
        {
            Debug.LogError("[AudioManager] BGM clip is null");
            return;
        }
        
        bgmSource.clip = clip;
        bgmSource.Play();
        Debug.Log($"[AudioManager] Playing BGM: {clip.name}");
    }

    /// <summary>
    /// Dừng nhạc nền với fade out
    /// </summary>
    public void StopBGM(float fadeOutTime = 0.5f)
    {
        if (fadeOutTime <= 0)
        {
            bgmSource.Stop();
        }
        else
        {
            StartCoroutine(FadeBGMOut(fadeOutTime));
        }
    }

    /// <summary>
    /// Phát sound effect
    /// </summary>
    public void PlaySFX(AudioClip clip, float volumeScale = 1f)
    {
        if (clip == null)
        {
            Debug.LogError("[AudioManager] SFX clip is null");
            return;
        }
        
        sfxSource.PlayOneShot(clip, volumeScale);
    }

    public void SetBGMVolume(float volume)
    {
        bgmVolume = Mathf.Clamp01(volume);
        if (bgmSource != null)
            bgmSource.volume = bgmVolume;
    }

    public void SetSFXVolume(float volume)
    {
        sfxVolume = Mathf.Clamp01(volume);
        if (sfxSource != null)
            sfxSource.volume = sfxVolume;
    }

    private System.Collections.IEnumerator FadeBGMOut(float duration)
    {
        float startVolume = bgmSource.volume;
        float elapsedTime = 0f;
        
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            bgmSource.volume = Mathf.Lerp(startVolume, 0f, elapsedTime / duration);
            yield return null;
        }
        
        bgmSource.Stop();
        bgmSource.volume = startVolume;
    }
}
