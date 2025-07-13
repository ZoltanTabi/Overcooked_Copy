using System;
using UnityEngine;

public class TrashCounter : BaseCounter
{
    [SerializeField] private Transform counterBottomPoint;

    private float fallSpeed = 0f;

    public static event Action<TrashCounter> OnAnyObjectPlacedInTrashCounter;

    private void Update()
    {
        if (HasKitchenObject())
        {
            fallSpeed += -Physics.gravity.y * Time.deltaTime;
            GetKitchenObject().transform.position += fallSpeed * Time.deltaTime * Vector3.down;

            if (GetKitchenObject().transform.position.y <= counterBottomPoint.position.y)
            {
                GetKitchenObject().DestroySelf();
                fallSpeed = 0f;
            }
        }
    }

    public override bool Interact(Player player)
    {
        if (player.HasKitchenObject())
        {
            if (HasKitchenObject())
            {
                GetKitchenObject().DestroySelf();
            }

            fallSpeed = 0f;

            player.GetKitchenObject().SetKitchenObjectParent(this);
            OnAnyObjectPlacedInTrashCounter?.Invoke(this);

            return true;
        }

        return false;
    }
}
