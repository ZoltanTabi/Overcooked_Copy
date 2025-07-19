using UnityEngine;

public class StoveCounterBurnFlashingBar : MonoBehaviour
{
    private const string IS_FLASHING = "IsFlashing";

    [SerializeField] private StoveCounter stoveCounter;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        stoveCounter.OnProgressChanged += StoveCounter_OnProgressChanged;

        animator.SetBool(IS_FLASHING, false);
    }

    private void StoveCounter_OnProgressChanged(float progress)
    {
        bool isFlashing = stoveCounter.IsFried() && progress >= StoveBurnWarningUI.BURN_SHOW_PROGRESS_AMOUNT;

        animator.SetBool(IS_FLASHING, isFlashing);
    }
}
