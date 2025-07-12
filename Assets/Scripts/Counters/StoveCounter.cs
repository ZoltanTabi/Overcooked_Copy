using System;
using System.Linq;
using UnityEngine;

public class StoveCounter : BaseCounter
{
    public enum StoveCounterState
    {
        Idle,
        Frying,
        Fried,
        Burned
    }

    [SerializeField] private FryingRecipeSO[] fryingRecipeSOs;
    [SerializeField] private BurningRecipeSO[] burningRecipeSOs;

    private StoveCounterState state = StoveCounterState.Idle;
    private float fryingTimer;
    private float burningTimer;
    private FryingRecipeSO fryingRecipeSO;
    private BurningRecipeSO burningRecipeSO;

    public event Action<StoveCounterState> OnStateChanged;

    private void Start()
    {
        state = StoveCounterState.Idle;
        fryingTimer = 0f;
        burningTimer = 0f;
        fryingRecipeSO = null;
        burningRecipeSO = null;
    }

    private void Update()
    {
        if (!HasKitchenObject())
        {
            return;
        }

        switch (state)
        {
            case StoveCounterState.Idle:
                break;
            case StoveCounterState.Frying:
                fryingTimer += Time.deltaTime;

                if (fryingTimer >= fryingRecipeSO.fryingTimerMax)
                {
                    GetKitchenObject().DestroySelf();
                    KitchenObject.SpawnKitchenObject(fryingRecipeSO.output, this);

                    SetState(StoveCounterState.Fried);
                    burningTimer = 0f;
                    burningRecipeSO = GetBurningRecipeForInput(GetKitchenObject().GetKitchenObjectSO());
                }
                break;
            case StoveCounterState.Fried:
                burningTimer += Time.deltaTime;

                if (burningTimer >= burningRecipeSO.burningTimerMax)
                {
                    GetKitchenObject().DestroySelf();
                    KitchenObject.SpawnKitchenObject(burningRecipeSO.output, this);

                    SetState(StoveCounterState.Burned);
                }
                break;
            case StoveCounterState.Burned:
                break;
        }
    }

    public override bool Interact(Player player)
    {
        if (!HasKitchenObject() && player.HasKitchenObject() && TryGetFryingRecipeForInput(player.GetKitchenObject().GetKitchenObjectSO(), out var fryingRecipeSO))
        {
            this.fryingRecipeSO = fryingRecipeSO;
            player.GetKitchenObject().SetKitchenObjectParent(this);

            SetState(StoveCounterState.Frying);
            fryingTimer = 0f;

            return true;
        }
        else if (HasKitchenObject() && !player.HasKitchenObject())
        {
            GetKitchenObject().SetKitchenObjectParent(player);

            SetState(StoveCounterState.Idle);

            return true;
        }

        return false;
    }

    private FryingRecipeSO GetFryingRecipeForInput(KitchenObjectSO inputKitchenObjectSO)
    {
        return fryingRecipeSOs.FirstOrDefault(x => x.input == inputKitchenObjectSO);
    }

    private bool TryGetFryingRecipeForInput(KitchenObjectSO inputKitchenObjectSO, out FryingRecipeSO fryingRecipeSO)
    {
        fryingRecipeSO = GetFryingRecipeForInput(inputKitchenObjectSO);

        return fryingRecipeSO != null;
    }

    private BurningRecipeSO GetBurningRecipeForInput(KitchenObjectSO inputKitchenObjectSO)
    {
        return burningRecipeSOs.FirstOrDefault(x => x.input == inputKitchenObjectSO);
    }

    private bool TryGetBurningRecipeForInput(KitchenObjectSO inputKitchenObjectSO, out BurningRecipeSO burningRecipeSO)
    {
        burningRecipeSO = GetBurningRecipeForInput(inputKitchenObjectSO);

        return burningRecipeSO != null;
    }

    private void SetState(StoveCounterState state)
    {
        this.state = state;
        OnStateChanged?.Invoke(state);
    }
}
