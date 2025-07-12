using UnityEngine;

public class ClearCounter : BaseCounter
{
    [SerializeField] private KitchenObjectSO kitchenObjectSO;

    public override bool Interact(Player player)
    {
        return base.Interact(player);
    }
}
