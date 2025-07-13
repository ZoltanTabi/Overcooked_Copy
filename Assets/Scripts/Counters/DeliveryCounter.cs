using UnityEngine;

public class DeliveryCounter : BaseCounter
{
    public static DeliveryCounter Instance { get; private set; }

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

    public override bool Interact(Player player)
    {
        if (player.HasKitchenObject() && player.GetKitchenObject().TryGetPlate(out var plateKitchenObject))
        {
            DeliveryManager.Instance.DeliverRecipe(plateKitchenObject);

            plateKitchenObject.DestroySelf();

            return true;
        }

        return false;
    }
}
