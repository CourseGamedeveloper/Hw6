using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class myTilemapCaveGenerator : MonoBehaviour
{
    [SerializeField] private Tilemap tilemap = null;
    [SerializeField] private TileBase wallTile = null;
    [SerializeField] private TileBase floorTile = null;
    [SerializeField] private float randomFillPercent = 0.5f;
    [SerializeField] private int gridWidth = 100; // Width of the grid
    [SerializeField] private int gridHeight = 100; // Height of the grid
    [SerializeField] private int simulationSteps = 20;
    [SerializeField] private float pauseTime = 1f;

    [Tooltip("Prefab for the player")]
    [SerializeField] private GameObject playerPrefab = null;

    [Tooltip("Prefab for the enemies")]
    [SerializeField] private GameObject enemyPrefab = null;

    [Tooltip("Number of enemies to spawn")]
    [SerializeField] private int enemyCount = 3;

    private MyCaveGenerator caveGenerator;

    void Start()
    {
        Random.InitState(100);
        caveGenerator = new MyCaveGenerator(randomFillPercent, gridWidth, gridHeight);
        caveGenerator.RandomizeMap();

        GenerateAndDisplayTexture(caveGenerator.GetMap());
        SimulateCavePattern();
        SpawnEntities();
    }

    async void SimulateCavePattern()
    {
        for (int i = 0; i < simulationSteps; i++)
        {
            await Awaitable.WaitForSecondsAsync(pauseTime);
            caveGenerator.SmoothMap();
            GenerateAndDisplayTexture(caveGenerator.GetMap());
        }
        Debug.Log("Simulation completed!");
    }

    private void GenerateAndDisplayTexture(int[,] data)
    {
        for (int y = 0; y < gridHeight; y++)
        {
            for (int x = 0; x < gridWidth; x++)
            {
                var position = new Vector3Int(x, y, 0);
                var tile = data[x, y] == 1 ? wallTile : floorTile;
                tilemap.SetTile(position, tile);
            }
        }
    }

    private void SpawnEntities()
    {
        List<Vector3Int> floorPositions = GetFloorPositions();

        // Spawn player
        Vector3 playerPosition = tilemap.GetCellCenterWorld(floorPositions[0]);
        Instantiate(playerPrefab, playerPosition, Quaternion.identity);

        // Spawn enemies
        for (int i = 1; i <= enemyCount; i++)
        {
            Vector3 enemyPosition = tilemap.GetCellCenterWorld(floorPositions[i]);
            Instantiate(enemyPrefab, enemyPosition, Quaternion.identity);
        }
    }

    private List<Vector3Int> GetFloorPositions()
    {
        List<Vector3Int> floorPositions = new List<Vector3Int>();

        for (int y = 0; y < gridHeight; y++)
        {
            for (int x = 0; x < gridWidth; x++)
            {
                var position = new Vector3Int(x, y, 0);
                if (tilemap.GetTile(position) == floorTile)
                {
                    floorPositions.Add(position);
                }
            }
        }

        floorPositions.Shuffle(); // Shuffle the list to randomize positions
        return floorPositions;
    }
}

public static class ListExtensions
{
    public static void Shuffle<T>(this IList<T> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            T temp = list[i];
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }
}
