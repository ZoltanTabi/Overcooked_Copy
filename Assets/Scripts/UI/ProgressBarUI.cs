using UnityEngine;
using UnityEngine.UI;

public class ProgressBarUI : MonoBehaviour
{
    [SerializeField] private GameObject hasProgressGameObject;
    [SerializeField] private Image progressBarImage;

    private IHasProgress hasProgress;

    private void Start()
    {
        hasProgress = hasProgressGameObject.GetComponent<IHasProgress>();

        if (hasProgress == null)
        {
            Debug.LogError("IHasProgress component not found on the specified GameObject.: " + hasProgressGameObject);
            return;
        }

        progressBarImage.fillAmount = 0f;

        hasProgress.OnProgressChanged += HasProgress_OnProgressChanged;

        Hide();
    }

    private void HasProgress_OnProgressChanged(float progressNormalized)
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
