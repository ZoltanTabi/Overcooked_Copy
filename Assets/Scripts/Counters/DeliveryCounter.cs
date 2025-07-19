using UnityEngine;

public class DeliveryCounter : BaseCounter
{
    public static DeliveryCounter Instance { get; private set; }

    [SerializeField] private Transform counterDeliveryPoint;
    [SerializeField] private float deliverySpeed = 2f;

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

        if (HasKitchenObject())
        {
            GetKitchenObject().transform.position = Vector3.MoveTowards(GetKitchenObject().transform.position, counterDeliveryPoint.position, deliverySpeed * Time.deltaTime);

            if (Vector3.Distance(GetKitchenObject().transform.position, counterDeliveryPoint.position) < 0.01f)
            {
                DeliveryManager.Instance.DeliverRecipe(GetKitchenObject() as PlateKitchenObject);

                GetKitchenObject().DestroySelf();
            }
        }
    }

    public override bool Interact(Player player)
    {
        if (player.HasKitchenObject() && player.GetKitchenObject().TryGetPlate(out var _))
        {
            if (HasKitchenObject())
            {
                DeliveryManager.Instance.DeliverRecipe(GetKitchenObject() as PlateKitchenObject);

                GetKitchenObject().DestroySelf();
            }

            player.GetKitchenObject().SetKitchenObjectParent(this);

            return true;
        }

        return false;
    }
}
