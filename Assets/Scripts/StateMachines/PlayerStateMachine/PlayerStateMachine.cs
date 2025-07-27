using System.Linq;
using UnityEngine;

public class PlayerStateMachine : BaseStateMachine<PlayerStateMachine>
{
    public Player Player { get; private set; }
    public bool IsStarted { get; private set; }
    public RecipeSO CurrentRecipe { get; private set; }
    public PlateKitchenObject PlateKitchenObject { get; private set; }
    public StoveCounter StoveCounter { get; private set; } = null;
    public bool IngredientIsReadyOnStove { get; private set; } = false;

    public PlayerStateMachine(Player player)
    {
        Player = player;
    }

    public void Start()
    {
        IsStarted = true;
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
        StoveCounter.OnStateChanged += StoveCounter_OnStateChanged;
    }

    public void GetIngredientFromStove()
    {
        StoveCounter = null;
        IngredientIsReadyOnStove = false;
    }

    private void StoveCounter_OnStateChanged(StoveCounter.StoveCounterState stoveCounterState)
    {
        StoveCounter.OnStateChanged -= StoveCounter_OnStateChanged;
        
        IngredientIsReadyOnStove = true;
    }

    public T GetNearestCounter<T>() where T : BaseCounter
    {
        var counters = Object.FindObjectsByType<T>(FindObjectsSortMode.None);

        return GetClosestCounter(counters);
    }

    public ContainerCounter GetNearestContainerCounterByIngredient(KitchenObjectSO kitchenObjectSO)
    {
        var counters = Object.FindObjectsByType<ContainerCounter>(FindObjectsSortMode.None)
            .Where(counter => counter.GetKitchenObjectSO() == kitchenObjectSO)
            .ToArray();

        return GetClosestCounter(counters);
    }

    public (T, Vector2) GetNearestCounterAndDirection<T>() where T : BaseCounter
    {
        var counters = Object.FindObjectsByType<T>(FindObjectsSortMode.None);

        return GetClosestCounterAndDirection(counters);
    }

    public Vector2 GetMoveDirection(BaseCounter baseCounter)
    {
        Vector3 dir3D = baseCounter.transform.position - Player.transform.position;

        return new Vector2(dir3D.x, dir3D.z).normalized;
    }

    public (ContainerCounter, Vector2) GetNearestContainerCounterAndDirectionByIngredient(KitchenObjectSO kitchenObjectSO)
    {
        var counters = Object.FindObjectsByType<ContainerCounter>(FindObjectsSortMode.None)
            .Where(counter => counter.GetKitchenObjectSO() == kitchenObjectSO)
            .ToArray();

        return GetClosestCounterAndDirection(counters);
    }

    private (T, Vector2) GetClosestCounterAndDirection<T>(T[] counters) where T : BaseCounter
    {
        T targetCounter = GetClosestCounter(counters);
        Vector2 moveDirection = GetMoveDirection(targetCounter);

        return (targetCounter, moveDirection);
    }

    private T GetClosestCounter<T>(T[] counters) where T : BaseCounter
    {
        return counters
            .OrderBy(counter => Vector3.Distance(Player.transform.position, counter.transform.position))
            .FirstOrDefault();
    }
}
