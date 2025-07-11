using System;
using UnityEngine;

public class ContainerCounter : BaseCounter, IKitchenObjectParent
{
    [SerializeField] private KitchenObjectSO kitchenObjectSO;

    public event Action OnPlayerGrabbedObject;

    public override bool Interact(Player player)
    {
        if (base.Interact(player))
        {
            return true;
        }

        KitchenObject.SpawnKitchenObject(kitchenObjectSO, player);

        OnPlayerGrabbedObject?.Invoke();

        return true;
    }
}
