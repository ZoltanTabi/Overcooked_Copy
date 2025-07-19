using UnityEngine;

public class StoveCounterSound : MonoBehaviour
{
    [SerializeField] private StoveCounter stoveCounter;

    private AudioSource audioSource;

    private float warningSoundTimer;
    private readonly float warningSoundTimerMax = 0.2f;
    private bool isWarningSoundPlaying;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        stoveCounter.OnStateChanged += StoveCounter_OnStateChanged;
        stoveCounter.OnProgressChanged += StoveCounter_OnProgressChanged;
    }

    private void Update()
    {
        if (!isWarningSoundPlaying)
        {
            return;
        }

        warningSoundTimer -= Time.deltaTime;

        if (warningSoundTimer < 0f)
        {
            warningSoundTimer = warningSoundTimerMax;

            SoundManager.Instance.PlayWarningSound(stoveCounter.transform.position);
        }
    }

    private void StoveCounter_OnStateChanged(StoveCounter.StoveCounterState state)
    {
        bool playSound = state == StoveCounter.StoveCounterState.Frying
                                   || state == StoveCounter.StoveCounterState.Fried;

        if (playSound)
        {
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
        else
        {
            audioSource.Stop();
        }
    }

    private void StoveCounter_OnProgressChanged(float progress)
    {
        isWarningSoundPlaying = stoveCounter.IsFried() && progress >= StoveBurnWarningUI.BURN_SHOW_PROGRESS_AMOUNT;
    }
}
