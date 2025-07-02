using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private const string IS_WALKING = "IsWalking";

    private Animator animator;
    private Player player;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        player = GetComponentInParent<Player>();

        if (animator == null)
        {
            Debug.LogError("Animator component not found on PlayerAnimator.");
        }

        if (player == null)
        {
            Debug.LogError("Player reference not set in PlayerAnimator.");
        }
    }

    private void Update()
    {
        if (player.IsWalking())
        {
            animator.SetBool(IS_WALKING, true);
        }
        else
        {
            animator.SetBool(IS_WALKING, false);
        }
    }
}
