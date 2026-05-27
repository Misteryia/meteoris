using UnityEngine;

/// <summary>
/// Управляет звуковыми эффектами и фоновой музыкой.
/// Все звуки проигрываются через собственные AudioSource — без задержек.
/// </summary>
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Музыка")]
    [SerializeField] private AudioSource musicSource;

    [Header("Зацикленные звуки")]
    [SerializeField] private AudioSource overheatSource;

    [Header("One-shot звуки")]
    [SerializeField] private AudioSource sfxSource;

    [Header("Клипы")]
    [SerializeField] private AudioClip shootClip;
    [SerializeField] private AudioClip explosionClip;
    [SerializeField] private AudioClip overheatClip;
    [SerializeField] private AudioClip stationHitClip;

    [Header("Громкость")]
    [SerializeField] [Range(0f, 1f)] private float shootVolume = 0.3f;
    [SerializeField] [Range(0f, 1f)] private float explosionVolume = 0.7f;
    [SerializeField] [Range(0f, 1f)] private float stationHitVolume = 1f;
    [SerializeField] [Range(0f, 1f)] private float overheatVolume = 0.7f;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void PlayShoot()
    {
        PlayOneShot(shootClip, shootVolume);
    }

    public void PlayExplosion(Vector3 position)
    {
        PlayOneShot(explosionClip, explosionVolume);
    }

    public void PlayOverheatStart()
    {
        if (overheatSource != null && overheatClip != null)
        {
            overheatSource.clip = overheatClip;
            overheatSource.loop = true;
            overheatSource.volume = overheatVolume;
            overheatSource.Play();
        }
    }

    public void PlayOverheatEnd()
    {
        if (overheatSource != null)
            overheatSource.Stop();
    }

    public void PlayStationHit()
    {
        PlayOneShot(stationHitClip, stationHitVolume);
    }

    private void PlayOneShot(AudioClip clip, float volume)
    {
        if (clip != null && sfxSource != null)
            sfxSource.PlayOneShot(clip, volume);
    }

    void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }
}
