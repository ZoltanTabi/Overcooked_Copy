using UnityEngine;

public class BaseCounter : MonoBehaviour, IKitchenObjectParent
{
    [SerializeField] private Transform counterTopPoint;

    private KitchenObject kitchenObject;

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
