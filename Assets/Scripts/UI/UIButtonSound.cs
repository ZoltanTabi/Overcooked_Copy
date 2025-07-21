using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIButtonSound : MonoBehaviour, ISelectHandler, IPointerEnterHandler
{
    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();

        if (button != null)
        {
            button.onClick.AddListener(OnButtonClick);
        }
    }

    private void OnButtonClick()
    {
        Debug.Log("Button clicked: " + gameObject.name);
        SoundManager.Instance.PlayButtonClickSound();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Pointer entered button: " + gameObject.name);
        SoundManager.Instance.PlayButtonSelectSound();
    }

    public void OnSelect(BaseEventData eventData)
    {
        Debug.Log("Button selected: " + gameObject.name);
        SoundManager.Instance.PlayButtonSelectSound();
    }
}
