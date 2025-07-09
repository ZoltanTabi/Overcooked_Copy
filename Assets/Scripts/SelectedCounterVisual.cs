using UnityEngine;

public class SelectedCounterVisual : MonoBehaviour
{
    [SerializeField] private BaseCounter baseCounter;
    [SerializeField] private GameObject[] selectedVisuals;

    private void Start()
    {
        Player.Instance.OnCounterSelected += Player_OnCounterSelected;
    }

    private void Player_OnCounterSelected(BaseCounter counter)
    {
        if (baseCounter == counter)
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
        foreach (GameObject selectedVisual in selectedVisuals)
        {
            selectedVisual.SetActive(true);
        }
    }

    private void Hide()
    {
        foreach (GameObject selectedVisual in selectedVisuals)
        {
            selectedVisual.SetActive(false);
        }
    }
}
