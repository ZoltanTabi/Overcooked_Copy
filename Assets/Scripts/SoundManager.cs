using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [SerializeField] private AudioClipsRefSO audioClipsRefSO;

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
    }

    private void Start()
    {
        DeliveryManager.Instance.OnRecipeSuccess += DeliveryManager_OnRecipeSuccess;
        DeliveryManager.Instance.OnRecipeFailed += DeliveryManager_OnRecipeFailed;
        CuttingCounter.OnAnyCut += CuttingCounter_OnAnyCut;
        Player.Instance.OnPickSomething += Player_OnPickSomething;
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

    private void PlaySound(AudioClip audioClip, Vector3 position, float volume = 1f)
    {
        AudioSource.PlayClipAtPoint(audioClip, position, volume);
    }

    private void PlaySound(AudioClip[] audioClips, Vector3 position, float volume = 1f)
    {
        PlaySound(audioClips[UnityEngine.Random.Range(0, audioClips.Length)], position, volume);
    }

    public void PlayFootStepsSound(Vector3 position, float volume = 1f)
    {
        PlaySound(audioClipsRefSO.footStep, position, volume);
    }
}
