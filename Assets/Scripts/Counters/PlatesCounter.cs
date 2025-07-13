using System;
using UnityEngine;

public class PlatesCounter : BaseCounter
{
    [SerializeField] private KitchenObjectSO plateKitchenObjectSO;
    [SerializeField] private float spawnPlateTimerMax = 4f;
    [SerializeField] private int platesSpawnedAmountMax = 4;

    private float spawnPlateTimer;
    private int platesSpawnedAmount;

    public event Action OnPlateSpawned;
    public event Action OnPlateRemoved;

    private void Update()
    {
        if (!GameManager.Instance.IsGamePlaying())
        {
            return;
        }

        spawnPlateTimer += Time.deltaTime;

        if (spawnPlateTimer <= spawnPlateTimerMax)
        {
            return;
        }

        spawnPlateTimer = 0f;

        if (platesSpawnedAmount < platesSpawnedAmountMax)
        {
            ++platesSpawnedAmount;

            OnPlateSpawned?.Invoke();
        }
    }

    public override bool Interact(Player player)
    {
        if (!player.HasKitchenObject() && platesSpawnedAmount > 0)
        {
            --platesSpawnedAmount;

            KitchenObject.SpawnKitchenObject(plateKitchenObjectSO, player);
            OnPlateRemoved?.Invoke();

            return true;
        }

        return false;
    }
}
