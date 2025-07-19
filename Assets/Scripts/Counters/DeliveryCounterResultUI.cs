using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeliveryCounterResultUI : MonoBehaviour
{
    private const string POPUP_TRIGGER = "Popup";

    [Header("UI elements")]
    [SerializeField] private Image backgroundImage;
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI messagetext;

    [Header("Success Delivery Settings")]
    [SerializeField] private Color successColor;
    [SerializeField] private Sprite successSprite;

    [Header("Failed Delivery Settings")]
    [SerializeField] private Color failedColor;
    [SerializeField] private Sprite failedSprite;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        DeliveryManager.Instance.OnRecipeSuccess += DeliveryManager_OnRecipeSuccess;
        DeliveryManager.Instance.OnRecipeFailed += DeliveryManager_OnRecipeFailed;

        Hide();
    }

    private void DeliveryManager_OnRecipeSuccess()
    {
        Show();

        backgroundImage.color = successColor;
        iconImage.sprite = successSprite;
        messagetext.text = "Delivery\nSuccess!";

        animator.SetTrigger(POPUP_TRIGGER);
    }

    private void DeliveryManager_OnRecipeFailed()
    {
        Show();

        backgroundImage.color = failedColor;
        iconImage.sprite = failedSprite;
        messagetext.text = "Delivery\nFailed!";

        animator.SetTrigger(POPUP_TRIGGER);
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
