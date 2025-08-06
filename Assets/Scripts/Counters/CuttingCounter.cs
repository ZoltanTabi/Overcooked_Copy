using System;
using System.Linq;
using UnityEngine;

public class CuttingCounter : BaseCounter, IHasProgress
{
    [SerializeField] private CuttingRecipeSO[] cuttingRecipeSOs;

    private int cuttingProgress;

    public static event Action<CuttingCounter> OnAnyCut;

    public event Action<float> OnProgressChanged;
    public event Action OnCut;

    public override bool Interact(IKitchenObjectParent parent)
    {
        if (!HasKitchenObject() && parent.HasKitchenObject() && TryGetCuttingRecipeForInput(parent.GetKitchenObject().GetKitchenObjectSO(), out var cuttingRecipeSO))
        {
            cuttingProgress = 0;

            InvokeProgressChanged(cuttingRecipeSO.cuttingProgressMax);
        }

        return base.Interact(parent);
    }

    public override bool InteractAlternate(IKitchenObjectParent parent)
    {
        if (HasKitchenObject() && TryGetCuttingRecipeForInput(GetKitchenObject().GetKitchenObjectSO(), out var cuttingRecipeSO))
        {
            ++cuttingProgress;

            OnCut?.Invoke();
            OnAnyCut?.Invoke(this);

            if (cuttingProgress >= cuttingRecipeSO.cuttingProgressMax)
            {
                GetKitchenObject().DestroySelf();

                KitchenObject.SpawnKitchenObject(cuttingRecipeSO.output, this);
            }

            InvokeProgressChanged(cuttingRecipeSO.cuttingProgressMax);

            return true;
        }

        return false;
    }

    private CuttingRecipeSO GetCuttingRecipeForInput(KitchenObjectSO inputKitchenObjectSO)
    {
        return cuttingRecipeSOs.FirstOrDefault(x => x.input == inputKitchenObjectSO);
    }

    private bool TryGetCuttingRecipeForInput(KitchenObjectSO inputKitchenObjectSO, out CuttingRecipeSO cuttingRecipeSO)
    {
        cuttingRecipeSO = GetCuttingRecipeForInput(inputKitchenObjectSO);

        return cuttingRecipeSO != null;
    }

    private void InvokeProgressChanged(int cuttingProgressMax)
    {
        OnProgressChanged?.Invoke((float)cuttingProgress / cuttingProgressMax);
    }

    public static new void ResetStaticData()
    {
        BaseCounter.ResetStaticData();

        OnAnyCut = null;
    }
}
