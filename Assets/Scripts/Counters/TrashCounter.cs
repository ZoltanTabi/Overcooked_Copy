using UnityEngine;

public class TrashCounter : BaseCounter
{
    public override bool Interact(Player player)
    {
        if (player.HasKitchenObject())
        {
            player.GetKitchenObject().DestroySelf();

            return true;
        }

        return false;
    }
}
