using System;
using UnityEngine;

public class BaseCounter : MonoBehaviour, IKitchenObjectParent
{
    [SerializeField] private Transform counterTopPoint;

    private KitchenObject kitchenObject;

    public static event Action<BaseCounter> OnAnyObjectPlacedOnCounter;

    public virtual bool Interact(Player player)
    {
        if (!HasKitchenObject() && player.HasKitchenObject())
        {
            player.GetKitchenObject().SetKitchenObjectParent(this);

            return true;
        }
        else if (HasKitchenObject() && !player.HasKitchenObject())
        {
            GetKitchenObject().SetKitchenObjectParent(player);

            return true;
        }
        else if (HasKitchenObject() && player.HasKitchenObject() && player.GetKitchenObject().TryGetPlate(out var plateKitchenObject))
        {
            if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO()))
            {
                GetKitchenObject().DestroySelf();

                return true;
            }
        }
        else if (HasKitchenObject() && player.HasKitchenObject() && GetKitchenObject().TryGetPlate(out plateKitchenObject))
        {
            if (plateKitchenObject.TryAddIngredient(player.GetKitchenObject().GetKitchenObjectSO()))
            {
                player.GetKitchenObject().DestroySelf();

                return true;
            }
        }

        return false;
    }

    public virtual bool InteractAlternate(Player player)
    {
        return false;
    }

    public Transform GetKitchenObjectFollowTransform()
    {
        return counterTopPoint;
    }

    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        this.kitchenObject = kitchenObject;

        if (this is not TrashCounter && kitchenObject != null)
        {
            OnAnyObjectPlacedOnCounter?.Invoke(this);
        }
    }

    public KitchenObject GetKitchenObject()
    {
        return kitchenObject;
    }

    public void ClearKitchenObject()
    {
        kitchenObject = null;
    }

    public bool HasKitchenObject()
    {
        return kitchenObject != null;
    }
}
