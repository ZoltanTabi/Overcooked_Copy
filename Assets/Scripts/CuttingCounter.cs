using System;
using System.Linq;
using UnityEngine;

public class CuttingCounter : BaseCounter
{
    [SerializeField] private CuttingRecipeSO[] cuttingRecipeSOs;

    private int cuttingProgress;

    public event Action<float> OnProgressChanged;
    public event Action OnCut;

    public override bool Interact(Player player)
    {
        if (!HasKitchenObject() && player.HasKitchenObject() && TryGetCuttingRecipeForInput(player.GetKitchenObject().GetKitchenObjectSO(), out var cuttingRecipeSO))
        {
            cuttingProgress = 0;

            InvokeProgressChanged(cuttingRecipeSO.cuttingProgressMax);
        }

        return base.Interact(player);
    }

    public override bool InteractAlternate(Player player)
    {
        if (HasKitchenObject() && TryGetCuttingRecipeForInput(GetKitchenObject().GetKitchenObjectSO(), out var cuttingRecipeSO))
        {
            ++cuttingProgress;

            OnCut?.Invoke();
            InvokeProgressChanged(cuttingRecipeSO.cuttingProgressMax);

            if (cuttingProgress < cuttingRecipeSO.cuttingProgressMax)
            {
                return true;
            }

            GetKitchenObject().DestroySelf();

            KitchenObject.SpawnKitchenObject(cuttingRecipeSO.output, this);

            return true;
        }

        return false;
    }

    private bool TryGetCuttingRecipeForInput(KitchenObjectSO inputKitchenObjectSO, out CuttingRecipeSO cuttingRecipeSO)
    {
        cuttingRecipeSO = cuttingRecipeSOs.FirstOrDefault(x => x.input == inputKitchenObjectSO);

        return cuttingRecipeSO != null;
    }

    private void InvokeProgressChanged(int cuttingProgressMax)
    {
        OnProgressChanged?.Invoke((float)cuttingProgress / cuttingProgressMax);
    }
}
