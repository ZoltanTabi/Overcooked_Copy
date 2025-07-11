using System.Linq;
using UnityEngine;

public class CuttingCounter : BaseCounter
{
    [SerializeField] private CuttingRecipeSO[] cuttingRecipeSOs;

    public override bool Interact(Player player)
    {
        //if (!HasKitchenObject() && player.HasKitchenObject())
        //{
        //    if (!TryGetOutputForInput(player.GetKitchenObject().GetKitchenObjectSO(), out var _))
        //    {
        //        return false;
        //    }

        //    player.GetKitchenObject().SetKitchenObjectParent(this);

        //    return true;
        //}

        return base.Interact(player);
    }

    public override bool InteractAlternate(Player player)
    {
        if (HasKitchenObject() && TryGetOutputForInput(GetKitchenObject().GetKitchenObjectSO(), out var outputKitchenObjectSO))
        {
            GetKitchenObject().DestroySelf();

            KitchenObject.SpawnKitchenObject(outputKitchenObjectSO, this);

            return true;
        }

        return false;
    }

    private bool TryGetOutputForInput(KitchenObjectSO inputKitchenObjectSO, out KitchenObjectSO outputKitchenObjectSO)
    {
        CuttingRecipeSO cuttingRecipeSO = cuttingRecipeSOs.FirstOrDefault(x => x.input == inputKitchenObjectSO);

        if (cuttingRecipeSO != null)
        {
            outputKitchenObjectSO = cuttingRecipeSO.output;
        }
        else
        {
            outputKitchenObjectSO = null;
        }

        return outputKitchenObjectSO != null;
    }
}
