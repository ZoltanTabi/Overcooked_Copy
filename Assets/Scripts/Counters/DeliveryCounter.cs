using UnityEngine;

public class DeliveryCounter : BaseCounter
{
    public override bool Interact(Player player)
    {
        if (player.HasKitchenObject() && player.GetKitchenObject().TryGetPlate(out var plateKitchenObject))
        {
            plateKitchenObject.DestroySelf();

            return true;
        }

        return false;
    }
}
