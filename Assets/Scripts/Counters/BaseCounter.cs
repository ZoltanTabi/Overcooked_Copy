using System;
using UnityEngine;

public class BaseCounter : MonoBehaviour, IKitchenObjectParent
{
    [SerializeField] private Transform counterTopPoint;

    private KitchenObject kitchenObject;

    public static event Action<BaseCounter> OnAnyObjectPlacedOnCounter;

    public virtual bool Interact(IKitchenObjectParent parent)
    {
        if (!HasKitchenObject() && parent.HasKitchenObject())
        {
            parent.GetKitchenObject().SetKitchenObjectParent(this);

            return true;
        }
        else if (HasKitchenObject() && !parent.HasKitchenObject())
        {
            GetKitchenObject().SetKitchenObjectParent(parent);

            return true;
        }
        else if (HasKitchenObject() && parent.HasKitchenObject() && parent.GetKitchenObject().TryGetPlate(out var plateKitchenObject))
        {
            if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO()))
            {
                GetKitchenObject().DestroySelf();

                return true;
            }
        }
        else if (HasKitchenObject() && parent.HasKitchenObject() && GetKitchenObject().TryGetPlate(out plateKitchenObject))
        {
            if (plateKitchenObject.TryAddIngredient(parent.GetKitchenObject().GetKitchenObjectSO()))
            {
                parent.GetKitchenObject().DestroySelf();

                return true;
            }
        }

        return false;
    }
    
    public virtual bool InteractAlternate(IKitchenObjectParent parent)
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

    public static void ResetStaticData()
    {
        OnAnyObjectPlacedOnCounter = null;
    }
}
