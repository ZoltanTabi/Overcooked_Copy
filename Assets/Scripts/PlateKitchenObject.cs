using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlateKitchenObject : KitchenObject
{
    [SerializeField] private KitchenObjectSO[] validKitchenObjectSOs;

    private List<KitchenObjectSO> kitchenObjectSOs = new();

    public bool TryAddIngredient(KitchenObjectSO kitchenObjectSO)
    {
        if (!validKitchenObjectSOs.Contains(kitchenObjectSO) || kitchenObjectSOs.Contains(kitchenObjectSO))
        {
            return false;
        }

        kitchenObjectSOs.Add(kitchenObjectSO);

        return true;
    }
}
