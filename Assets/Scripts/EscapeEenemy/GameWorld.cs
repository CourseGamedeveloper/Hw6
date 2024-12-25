using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class GameWorld : MonoBehaviour
{
    public static GameWorld Instance { get; private set; } // Singleton instance

    [SerializeField] private Tilemap tilemap; // ���� �������
    [SerializeField] private AllowedTiles allowedTiles; // ������� �� AllowedTiles

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogError("Multiple instances of GameWorld detected!");
            Destroy(gameObject);
        }
    }

    public Tilemap Tilemap => tilemap;

    /// <summary>
    /// ���� �� ���� ����� ����� ��� ���� ������
    /// </summary>
    public bool IsWalkable(Vector3Int gridPosition)
    {
        var tile = tilemap.GetTile(gridPosition);
        return tile != null && allowedTiles.Contains(tile); // ����� �-AllowedTiles ������
    }

    /// <summary>
    /// ����� ����� �� ������ ������� ������
    /// </summary>
    public List<Vector3Int> GetNeighbors(Vector3Int gridPosition)
    {
        List<Vector3Int> neighbors = new List<Vector3Int>();
        Vector3Int[] directions = {
            new Vector3Int(1, 0, 0), // ����
            new Vector3Int(-1, 0, 0), // ����
            new Vector3Int(0, 1, 0), // �����
            new Vector3Int(0, -1, 0) // ����
        };

        foreach (var direction in directions)
        {
            Vector3Int neighbor = gridPosition + direction;
            if (IsWalkable(neighbor))
            {
                neighbors.Add(neighbor);
            }
        }

        return neighbors;
    }
}
