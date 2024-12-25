using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class GameWorld : MonoBehaviour
{
    public static GameWorld Instance { get; private set; } // Singleton instance

    [SerializeField] private Tilemap tilemap; // טבלת האריחים
    [SerializeField] private AllowedTiles allowedTiles; // אובייקט של AllowedTiles

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
    /// בודק אם אריח מסוים בטבלה הוא ניתן להליכה
    /// </summary>
    public bool IsWalkable(Vector3Int gridPosition)
    {
        var tile = tilemap.GetTile(gridPosition);
        return tile != null && allowedTiles.Contains(tile); // משתמש ב-AllowedTiles לבדיקה
    }

    /// <summary>
    /// מחזיר רשימה של השכנים הניתנים להליכה
    /// </summary>
    public List<Vector3Int> GetNeighbors(Vector3Int gridPosition)
    {
        List<Vector3Int> neighbors = new List<Vector3Int>();
        Vector3Int[] directions = {
            new Vector3Int(1, 0, 0), // ימין
            new Vector3Int(-1, 0, 0), // שמאל
            new Vector3Int(0, 1, 0), // למעלה
            new Vector3Int(0, -1, 0) // למטה
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
