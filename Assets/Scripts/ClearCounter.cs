using UnityEngine;

public class ClearCounter : MonoBehaviour
{
    [SerializeField] private KitchenObjectSO kitchenObjectSO;
    [SerializeField] private Transform counterTopPoint;

    public void Interact()
    {
        Transform kitchenObjectTransform = Instantiate(kitchenObjectSO.prefab, counterTopPoint);
        kitchenObjectTransform.localPosition = Vector3.zero;

        KitchenObject kitchenObject = kitchenObjectTransform.GetComponent<KitchenObject>();
        Debug.Log($"Cleared counter with object: {kitchenObject.GetKitchenObjectSO().objectName}");
    }
}
