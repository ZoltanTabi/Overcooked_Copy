using UnityEngine;
using UnityEngine.UI;

public class ProgressBarUI : MonoBehaviour
{
    [SerializeField] private CuttingCounter cuttingCounter;
    [SerializeField] private Image progressBarImage;

    private void Start()
    {
        progressBarImage.fillAmount = 0f;

        cuttingCounter.OnProgressChanged += CuttingCounter_OnProgressChanged;

        Hide();
    }

    private void CuttingCounter_OnProgressChanged(float progressNormalized)
    {
        progressBarImage.fillAmount = progressNormalized;

        if (progressNormalized <= 0f || progressNormalized >= 1f)
        {
            Hide();
        }
        else
        {
            Show();
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
