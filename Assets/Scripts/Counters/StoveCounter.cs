using System;
using System.Linq;
using UnityEngine;

public class StoveCounter : BaseCounter, IHasProgress
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
    public event Action<float> OnProgressChanged;

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
                HandleFryingState();
                break;
            case StoveCounterState.Fried:
                HandleFriedState();
                break;
            case StoveCounterState.Burned:
                break;
        }
    }

    private void HandleFryingState()
    {
        fryingTimer += Time.deltaTime;
        InvokeProgressChanged(fryingTimer, fryingRecipeSO.fryingTimerMax);

        if (fryingTimer >= fryingRecipeSO.fryingTimerMax)
        {
            GetKitchenObject().DestroySelf();
            KitchenObject.SpawnKitchenObject(fryingRecipeSO.output, this);

            SetState(StoveCounterState.Fried);
            burningTimer = 0f;
            burningRecipeSO = GetBurningRecipeForInput(GetKitchenObject().GetKitchenObjectSO());
        }
    }

    private void HandleFriedState()
    {
        burningTimer += Time.deltaTime;
        InvokeProgressChanged(burningTimer, burningRecipeSO.burningTimerMax);

        if (burningTimer >= burningRecipeSO.burningTimerMax)
        {
            GetKitchenObject().DestroySelf();
            KitchenObject.SpawnKitchenObject(burningRecipeSO.output, this);

            SetState(StoveCounterState.Burned);
            OnProgressChanged?.Invoke(0f);
        }
    }

    public override bool Interact(Player player)
    {
        if (!HasKitchenObject() && player.HasKitchenObject() && TryGetFryingRecipeForInput(player.GetKitchenObject().GetKitchenObjectSO(), out var fryingRecipeSO))
        {
            this.fryingRecipeSO = fryingRecipeSO;
            player.GetKitchenObject().SetKitchenObjectParent(this);

            fryingTimer = 0f;
            SetState(StoveCounterState.Frying);
            InvokeProgressChanged(fryingTimer, fryingRecipeSO.fryingTimerMax);

            return true;
        }
        else if (HasKitchenObject() && !player.HasKitchenObject())
        {
            GetKitchenObject().SetKitchenObjectParent(player);

            SetState(StoveCounterState.Idle);
            OnProgressChanged?.Invoke(0f);

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

    private void InvokeProgressChanged(float timer, float timerMax)
    {
        OnProgressChanged?.Invoke(timer / timerMax);
    }
}
