using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class HawkSpawner : MonoBehaviour
{
    public GameObject hawkPrefab;
    public TileMapVisualizer tileMapVisualizer;

    // Number of hawks to spawn
    public int numberOfHawks = 3;

    // Distance from other hawks
    public int distanceFromHawk = 8;

    // Minimum distance from the origin (0, 0, 0)
    public float minDistanceFromOrigin = 6f;

    // Maximum attempts to find spawn points for all hawks
    public int maxAttempts;

    void Start()
    {
        // Set maxAttempts to be numberOfHawks * 5
        maxAttempts = numberOfHawks * 5;
        
        SpawnHawks();
    }

    void SpawnHawks()
    {
        int hawksSpawned = 0;

        for (int attempt = 0; attempt < maxAttempts; attempt++)
        {
            List<Vector3Int> spawnPositions = FindValidSpawnPositions();

            if (spawnPositions.Count >= numberOfHawks)
            {
                for (int i = 0; i < numberOfHawks; i++)
                {
                    Instantiate(hawkPrefab, tileMapVisualizer.GetCellCenterWorld(spawnPositions[i]), Quaternion.identity);
                    hawksSpawned++;
                }
                break;
            }
        }
    }

    List<Vector3Int> FindValidSpawnPositions()
    {
        List<Vector3Int> validPositions = new List<Vector3Int>();

        BoundsInt bounds = tileMapVisualizer.GetTilemapBounds();

        foreach (Vector3Int pos in bounds.allPositionsWithin)
        {
            if (IsFloor(pos) && IsFarFromHawks(pos) && IsFarFromOrigin(pos))
            {
                validPositions.Add(pos);

                if (validPositions.Count >= numberOfHawks)
                {
                    break; // Stop searching once we have enough valid positions
                }
            }
        }

        return validPositions;
    }

    bool IsFloor(Vector3Int position)
    {
        return tileMapVisualizer.IsFloorTile(position);
    }

    bool IsFarFromHawks(Vector3Int position)
    {
        // Check if the position is at least distanceFromHawk away from other hawks
        Collider2D[] colliders = Physics2D.OverlapCircleAll(tileMapVisualizer.GetCellCenterWorld(position), distanceFromHawk);

        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Hawk"))
            {
                return false; // There's a hawk nearby
            }
        }

        return true; // No hawk found nearby
    }

    bool IsFarFromOrigin(Vector3Int position)
    {
        // Check if the position is at least minDistanceFromOrigin away from the origin (0, 0, 0)
        return Vector3.Distance(tileMapVisualizer.GetCellCenterWorld(position), Vector3.zero) >= minDistanceFromOrigin;
    }
}