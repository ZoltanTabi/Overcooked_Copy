using System.Collections;
using UnityEngine;

public class TrashCounter : BaseCounter
{
    [SerializeField] private Transform counterBottomPoint;

    private float fallSpeed = 0f;

    public override bool Interact(Player player)
    {
        if (player.HasKitchenObject())
        {
            if (HasKitchenObject())
            {
                StopCoroutine(KitchenObjectFallingAndDestroy());
                GetKitchenObject().DestroySelf();
            }

            fallSpeed = 0f;

            player.GetKitchenObject().SetKitchenObjectParent(this);
            StartCoroutine(KitchenObjectFallingAndDestroy());

            return true;
        }

        return false;
    }

    private IEnumerator KitchenObjectFallingAndDestroy()
    {
        while (HasKitchenObject() && GetKitchenObject().transform.position.y >= counterBottomPoint.position.y)
        {
            fallSpeed += -Physics.gravity.y * Time.deltaTime;

            GetKitchenObject().transform.position += fallSpeed * Time.deltaTime * Vector3.down;

            yield return null;
        }

        if (HasKitchenObject())
        {
            GetKitchenObject().DestroySelf();
        }
    }
}
