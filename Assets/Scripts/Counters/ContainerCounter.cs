using System;
using UnityEngine;

public class ContainerCounter : BaseCounter, IKitchenObjectParent
{
    [SerializeField] private KitchenObjectSO kitchenObjectSO;

    public event Action OnPlayerGrabbedObject;

    public override bool Interact(IKitchenObjectParent parent)
    {
        if (base.Interact(parent))
        {
            return true;
        }

        KitchenObject.SpawnKitchenObject(kitchenObjectSO, parent);

        OnPlayerGrabbedObject?.Invoke();

        return true;
    }

    public KitchenObjectSO GetKitchenObjectSO()
    {
        return kitchenObjectSO;
    }
}
