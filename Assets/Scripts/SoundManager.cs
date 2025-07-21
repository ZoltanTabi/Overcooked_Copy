using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private const string SOUND_EFFECTS_VOLUME_KEY = "SoundEffectsVolume";

    public static SoundManager Instance { get; private set; }

    [SerializeField] private AudioClipsRefSO audioClipsRefSO;

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

        Volume = PlayerPrefs.GetFloat(SOUND_EFFECTS_VOLUME_KEY, 1f);
    }

    private void Start()
    {
        if (DeliveryManager.Instance != null)
        {
            DeliveryManager.Instance.OnRecipeSuccess += DeliveryManager_OnRecipeSuccess;
            DeliveryManager.Instance.OnRecipeFailed += DeliveryManager_OnRecipeFailed;
        }

        if (Player.Instance != null)
        {
            Player.Instance.OnPickSomething += Player_OnPickSomething;
        }

        CuttingCounter.OnAnyCut += CuttingCounter_OnAnyCut;
        BaseCounter.OnAnyObjectPlacedOnCounter += BaseCounter_OnAnyObjectPlacedOnCounter;
        TrashCounter.OnAnyObjectPlacedInTrashCounter += TrashCounter_OnAnyObjectPlacedInTrashCounter;
    }

    private void DeliveryManager_OnRecipeSuccess()
    {
        PlaySound(audioClipsRefSO.deliverySuccess, DeliveryCounter.Instance.transform.position);
    }

    private void DeliveryManager_OnRecipeFailed()
    {
        PlaySound(audioClipsRefSO.deliveryFail, DeliveryCounter.Instance.transform.position);
    }

    private void CuttingCounter_OnAnyCut(CuttingCounter cuttingCounter)
    {
        PlaySound(audioClipsRefSO.chop, cuttingCounter.transform.position);
    }

    private void Player_OnPickSomething()
    {
        PlaySound(audioClipsRefSO.objectPickup, Player.Instance.transform.position);
    }

    private void BaseCounter_OnAnyObjectPlacedOnCounter(BaseCounter counter)
    {
        PlaySound(audioClipsRefSO.objectDrop, counter.transform.position);
    }

    private void TrashCounter_OnAnyObjectPlacedInTrashCounter(TrashCounter trashCounter)
    {
        PlaySound(audioClipsRefSO.trash, trashCounter.transform.position);
    }

    private void PlaySound(AudioClip audioClip, Vector3 position, float volumeMultiplier = 1f)
    {
        AudioSource.PlayClipAtPoint(audioClip, position, volumeMultiplier * Volume);
    }

    private void PlaySound(AudioClip[] audioClips, Vector3 position, float volume = 1f)
    {
        PlaySound(audioClips[UnityEngine.Random.Range(0, audioClips.Length)], position, volume);
    }

    public void PlayFootStepsSound(Vector3 position, float volume = 1f)
    {
        PlaySound(audioClipsRefSO.footStep, position, volume);
    }

    public void PlayCountdownSound()
    {
        PlaySound(audioClipsRefSO.warning, Camera.main.transform.position);
    }

    public void PlayWarningSound(Vector3 postion)
    {
        PlaySound(audioClipsRefSO.warning, postion);
    }

    public void PlayButtonClickSound()
    {
        Debug.Log("Button clicked sound played.");
        PlaySound(audioClipsRefSO.buttonClick, Camera.main.transform.position);
    }

    public void PlayButtonSelectSound()
    {
        Debug.Log("Button selected sound played.");
        PlaySound(audioClipsRefSO.buttonSelect, Camera.main.transform.position);
    }

    public void ChangeVolume()
    {
        Volume += 0.1f;

        if (Volume > 1.01f)
        {
            Volume = 0f;
        }

        PlayerPrefs.SetFloat(SOUND_EFFECTS_VOLUME_KEY, Volume);
        PlayerPrefs.Save();
    }
}
