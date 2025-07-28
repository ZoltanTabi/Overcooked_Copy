using System;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

public class PlayerStateMachine : BaseStateMachine<PlayerStateMachine>
{
    public Player Player { get; private set; }
    public bool IsStarted { get; set; }
    public RecipeSO CurrentRecipe { get; private set; }
    public PlateKitchenObject PlateKitchenObject { get; private set; }
    public StoveCounter StoveCounter { get; private set; } = null;

    public PlayerStateMachine(Player player)
    {
        Player = player;
    }

    public override void Update()
    {
        if (!GameManager.Instance.IsGamePlaying())
        {
            return;
        }

        base.Update();
    }
    
    public void SetCurrentRecipe(RecipeSO recipeSO)
    {
        SetPlateKitchenObject(null);
        CurrentRecipe = recipeSO;
    }

    public void SetPlateKitchenObject(PlateKitchenObject plateKitchenObject)
    {
        PlateKitchenObject = plateKitchenObject;
    }

    public void PutIngredientOnStove(StoveCounter stoveCounter)
    {
        if (stoveCounter == null)
        {
            Debug.LogError("StoveCounter is null!");
            return;
        }

        StoveCounter = stoveCounter;
    }

    public void TakeIngredientFromStove()
    {
        StoveCounter = null;
    }

    public T GetNearestCounter<T>(Func<T, bool> predicate = null) where T : BaseCounter
    {
        var counters = Object.FindObjectsByType<T>(FindObjectsSortMode.None);

        if (predicate != null)
        {
            counters = counters.Where(predicate).ToArray();
        }

        return counters
            .OrderBy(counter => Vector3.Distance(Player.transform.position, counter.transform.position))
            .FirstOrDefault();
    }
    
    public ContainerCounter GetNearestContainerCounterByIngredient(KitchenObjectSO kitchenObjectSO)
        => GetNearestCounter<ContainerCounter>(counter => counter.GetKitchenObjectSO() == kitchenObjectSO);
}
