using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Moves the object away from the player using a custom escape algorithm.
/// </summary>
public class Escaper : MonoBehaviour
{
    [SerializeField] private Transform player; // ����� ����� ����� ���� �����
    [SerializeField] private float moveSpeed = 2f; // ������ ������ �� �����

    private void Update()
    {
        Vector3Int currentGridPosition = GameWorld.Instance.Tilemap.WorldToCell(transform.position);
        Vector3Int playerGridPosition = GameWorld.Instance.Tilemap.WorldToCell(player.position);

        // ���� ����� �����
        List<Vector3Int> neighbors = GameWorld.Instance.GetNeighbors(currentGridPosition);

        // ����� ������ ��� ���� ������
        Vector3Int farthestNode = currentGridPosition;
        float maxDistance = Vector3.Distance(currentGridPosition, playerGridPosition);

        foreach (var neighbor in neighbors)
        {
            float distance = Vector3.Distance(neighbor, playerGridPosition);
            if (distance > maxDistance)
            {
                maxDistance = distance;
                farthestNode = neighbor;
            }
        }

        // ����� ������ �� �����
        Vector3 targetPosition = GameWorld.Instance.Tilemap.GetCellCenterWorld(farthestNode);
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
    }
}
