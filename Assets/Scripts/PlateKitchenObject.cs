using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlateKitchenObject : KitchenObject
{
    [SerializeField] private KitchenObjectSO[] validKitchenObjectSOs;

    private readonly List<KitchenObjectSO> kitchenObjectSOs = new();

    public event Action<KitchenObjectSO> OnIngredientAdded;

    public bool TryAddIngredient(KitchenObjectSO kitchenObjectSO)
    {
        if (!validKitchenObjectSOs.Contains(kitchenObjectSO) || kitchenObjectSOs.Contains(kitchenObjectSO))
        {
            return false;
        }

        kitchenObjectSOs.Add(kitchenObjectSO);
        OnIngredientAdded?.Invoke(kitchenObjectSO);

        return true;
    }

    public List<KitchenObjectSO> GetKitchenObjectSOs()
    {
        return kitchenObjectSOs;
    }
}
