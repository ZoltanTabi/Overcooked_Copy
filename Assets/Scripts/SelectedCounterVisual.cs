using UnityEngine;

public class SelectedCounterVisual : MonoBehaviour
{
    [SerializeField] private ClearCounter clearCounter;
    [SerializeField] private GameObject selectedVisual;

    private void Start()
    {
        Player.Instance.OnCounterSelected += Player_OnCounterSelected;
    }

    private void Player_OnCounterSelected(ClearCounter counter)
    {
        if (clearCounter == counter)
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
        selectedVisual.SetActive(true);
    }

    private void Hide()
    {
        selectedVisual.SetActive(false);
    }
}
