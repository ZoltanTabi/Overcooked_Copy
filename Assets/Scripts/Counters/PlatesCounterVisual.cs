using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlatesCounterVisual : MonoBehaviour
{
    [SerializeField] private PlatesCounter platesCounter;
    [SerializeField] private Transform counterTopPoint;
    [SerializeField] private Transform plateVisualPrefab;

    private readonly List<GameObject> plateVisuals = new();

    private void Start()
    {
        platesCounter.OnPlateSpawned += PlatesCounter_OnPlateSpawned;
        platesCounter.OnPlateRemoved += PlatesCounter_OnPlateRemoved;
    }

    private void PlatesCounter_OnPlateSpawned()
    {
        Transform plateVisual = Instantiate(plateVisualPrefab, counterTopPoint);

        float plateVisualOffsetY = 0.1f * plateVisuals.Count;
        plateVisual.localPosition = new Vector3(0f, plateVisualOffsetY, 0f);

        plateVisuals.Add(plateVisual.gameObject);
    }

    private void PlatesCounter_OnPlateRemoved()
    {
        GameObject lastPlateVisual = plateVisuals.Last();

        plateVisuals.Remove(lastPlateVisual);
        Destroy(lastPlateVisual);
    }
}
