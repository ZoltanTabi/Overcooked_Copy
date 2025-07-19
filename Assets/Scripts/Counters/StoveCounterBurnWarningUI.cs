using UnityEngine;

public class StoveBurnWarningUI : MonoBehaviour
{
    public const float BURN_SHOW_PROGRESS_AMOUNT = 0.5f;

    [SerializeField] private StoveCounter stoveCounter;

    private void Start()
    {
        stoveCounter.OnProgressChanged += StoveCounter_OnProgressChanged;

        Hide();
    }

    private void StoveCounter_OnProgressChanged(float progress)
    {
        bool show = stoveCounter.IsFried() && progress >= BURN_SHOW_PROGRESS_AMOUNT;

        if (show)
        {
            Show();
        }
        else
        {
            Hide();
        }
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
