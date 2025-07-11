using UnityEngine;

public class CuttingCounter : BaseCounter
{
    [SerializeField] private KitchenObjectSO cutKitchenObjectSO;

    public override bool Interact(Player player)
    {
        return base.Interact(player);
    }

    public override bool InteractAlternate(Player player)
    {
        if (HasKitchenObject())
        {
            GetKitchenObject().DestroySelf();

            KitchenObject.SpawnKitchenObject(cutKitchenObjectSO, this);

            return true;
        }

        return false;
    }
}
