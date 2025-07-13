using UnityEngine;

public class PlayerSound : MonoBehaviour
{
    private Player player;
    private float footstepTimer;
    private readonly float footstepTimerMax = 0.1f;

    private void Awake()
    {
        player = GetComponent<Player>();
    }

    private void Update()
    {
        if (player.IsWalking())
        {
            footstepTimer += Time.deltaTime;

            if (footstepTimer >= footstepTimerMax)
            {
                footstepTimer = 0f;
                SoundManager.Instance.PlayFootStepsSound(player.transform.position);
            }
        }
        else
        {
            footstepTimer = 0f;
        }
    }
}
