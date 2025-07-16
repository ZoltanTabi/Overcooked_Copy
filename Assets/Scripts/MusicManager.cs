using UnityEngine;

public class MusicManager : MonoBehaviour
{
    private const string MUSIC_VOLUME_KEY = "MusicVolume";

    public static MusicManager Instance { get; private set; }

    private AudioSource audioSource;

    public float Volume { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        audioSource = GetComponent<AudioSource>();

        Volume = PlayerPrefs.GetFloat(MUSIC_VOLUME_KEY, .3f);
        audioSource.volume = Volume;
    }

    public void ChangeVolume()
    {
        Volume += 0.1f;

        if (Volume > 1.01f)
        {
            Volume = 0f;
        }

        audioSource.volume = Volume;

        PlayerPrefs.SetFloat(MUSIC_VOLUME_KEY, Volume);
        PlayerPrefs.Save();
    }
}
