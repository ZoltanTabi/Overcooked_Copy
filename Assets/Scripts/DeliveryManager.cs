using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DeliveryManager : MonoBehaviour
{
    public static DeliveryManager Instance { get; private set; }

    [SerializeField] private RecipeListSO recipeListSO;

    private readonly List<RecipeSO> waitingRecipeSOs = new();
    private float spawnRecipeTimer;
    private readonly float spawnRecipeTimerMax = 4f;
    private readonly int waitingRecipesMax = 4;

    public event Action OnRecipeSpawned;
    public event Action OnRecipeDelivered;
    public event Action OnRecipeSuccess;
    public event Action OnRecipeFailed;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (!GameManager.Instance.IsGamePlaying())
        {
            return;
        }

        spawnRecipeTimer += Time.deltaTime;

        if (spawnRecipeTimer < spawnRecipeTimerMax)
        {
            return;
        }

        spawnRecipeTimer = 0f;

        if (waitingRecipeSOs.Count >= waitingRecipesMax)
        {
            return;
        }

        RecipeSO waitingRecipeSO = recipeListSO.recipeSOs[UnityEngine.Random.Range(0, recipeListSO.recipeSOs.Count)];
        waitingRecipeSOs.Add(waitingRecipeSO);

        OnRecipeSpawned?.Invoke();
    }

    public void DeliverRecipe(PlateKitchenObject plateKitchenObject)
    {
        RecipeSO waitingRecipeSO = waitingRecipeSOs.FirstOrDefault(recipe => recipe.kitchenObjectSOs.Length == plateKitchenObject.GetKitchenObjectSOs().Count
            && recipe.kitchenObjectSOs.All(kitchenObjectSO => plateKitchenObject.GetKitchenObjectSOs().Contains(kitchenObjectSO)));

        if (waitingRecipeSO == null)
        {
            OnRecipeFailed?.Invoke();

            return;
        }

        waitingRecipeSOs.Remove(waitingRecipeSO);

        OnRecipeDelivered?.Invoke();
        OnRecipeSuccess?.Invoke();
    }

    public List<RecipeSO> GetWaitingRecipeSOs()
    {
        return waitingRecipeSOs;
    }
}
