using System;
using System.Linq;
using UnityEngine;

public class PlateCompleteVisual : MonoBehaviour
{
    [Serializable]
    public struct KitchenObjectSO_GameObject
    {
        public KitchenObjectSO kitchenObjectSO;
        public GameObject gameObject;
    }

    [SerializeField] private PlateKitchenObject plateKitchenObject;
    [SerializeField] private KitchenObjectSO_GameObject[] kitchenObjectSO_GameObjects;

    private void Start()
    {
        plateKitchenObject.OnIngredientAdded += PlateKitchenObject_OnIngredientAdded;
    }

    private void PlateKitchenObject_OnIngredientAdded(KitchenObjectSO kitchenObjectSO)
    {
        KitchenObjectSO_GameObject? kitchenObjectSO_GameObject = kitchenObjectSO_GameObjects.FirstOrDefault(x => x.kitchenObjectSO == kitchenObjectSO);

        if (kitchenObjectSO_GameObject.HasValue)
        {
            kitchenObjectSO_GameObject.Value.gameObject.SetActive(true);
        }
    }
}
