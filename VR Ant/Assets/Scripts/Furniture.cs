using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ROOM
{
    Bathroom = 0,
    Bedroom = 1,
    Kitchen = 2,
    LivingRoom = 3
}

public class Furniture : MonoBehaviour
{
    public static Furniture Instance;

    [SerializeField] private FurnitureObject[] furniture;
    private List<GameObject> spawnedFurniture;

    [SerializeField] private Vector3 spawnOrigin;
    [SerializeField] private Vector2 spawnRange;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }

    private void Start()
    {
        spawnedFurniture = new List<GameObject>();
    }

    public FurnitureObject[] GetAllFurniture() { return furniture; }
    public FurnitureObject GetSingleFurniture(string name)
    {
        for (int i = 0; i < furniture.Length; i++)
            if (furniture[i].name == name)
                return furniture[i];

        return null; // Shouldn't be called, if all handled as expected
    }

    //
    // EXTERNAL CALLS
    //
    public void SpawnFurniture(string furnitureName)
    {
        // Get the definition of the furniture containing its information
        FurnitureObject furnitureObject = GetSingleFurniture(furnitureName);

        // Create a random spawn point within the range at the origin
        Vector3 spawnPosition = new Vector3(Random.Range(-spawnRange.x, spawnRange.x) + spawnOrigin.x, 100, Random.Range(-spawnRange.y, spawnRange.y) + spawnOrigin.z);

        // Shoot a ray from above straight down at the generated coordinates to find the Y level of the surface
        float yFromAbove = Tools.GetYFromAbove(spawnPosition);
        spawnPosition.y = yFromAbove;

        // Instantiate the object
        GameObject newFurniture = Instantiate(furnitureObject.prefab, spawnPosition, Quaternion.Euler(0, Random.Range(0, 360), 0));
        spawnedFurniture.Add(newFurniture);
    }

    //
    // GIZMOS
    //
    private void OnDrawGizmos()
    {
        if (GameManager.Instance != null && GameManager.IsDebugMode())
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(spawnOrigin, new Vector3(spawnRange.x, 5, spawnRange.y));
        }
    }
}

