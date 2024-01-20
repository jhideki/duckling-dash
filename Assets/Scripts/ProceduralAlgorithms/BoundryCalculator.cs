using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BoundryCalculator
{
    public static (Vector2Int, Vector2Int, Vector2Int, Vector2Int) GetCornerBoundaries(HashSet<Vector2Int> positions)
    {
        int maxX = int.MinValue;
        int maxY = int.MinValue;
        int minX = int.MaxValue;
        int minY = int.MaxValue;
        const int BOUNDARY_OFFSET = 10;

        foreach (var position in positions)
        {
            maxX = Mathf.Max(maxX, position.x);
            maxY = Mathf.Max(maxY, position.y);
            minX = Mathf.Min(minX, position.x);
            minY = Mathf.Min(minY, position.y);
        }

        Vector2Int maxVector = new Vector2Int(maxX + BOUNDARY_OFFSET, maxY + BOUNDARY_OFFSET);
        Vector2Int minVector = new Vector2Int(minX - BOUNDARY_OFFSET, minY - BOUNDARY_OFFSET);

        // Calculate the corner boundaries
        Vector2Int topLeft = new Vector2Int(minX - BOUNDARY_OFFSET, maxY + BOUNDARY_OFFSET);
        Vector2Int topRight = maxVector;
        Vector2Int bottomLeft = minVector;
        Vector2Int bottomRight = new Vector2Int(maxX + BOUNDARY_OFFSET, minY - BOUNDARY_OFFSET);

        return (topLeft, topRight, bottomLeft, bottomRight);
    }
    public static HashSet<Vector2Int> GetGridLocationsNotInBoundaries(HashSet<Vector2Int> set1, HashSet<Vector2Int> set2, (Vector2Int, Vector2Int, Vector2Int, Vector2Int) boundaries)
    {
        HashSet<Vector2Int> result = new HashSet<Vector2Int>();

        // Iterate through all grid locations within the boundaries
        for (int x = boundaries.Item3.x; x <= boundaries.Item2.x; x++)
        {
            for (int y = boundaries.Item4.y; y <= boundaries.Item1.y; y++)
            {
                Vector2Int currentLocation = new Vector2Int(x, y);

                // Check if the location is not in either set
                if (!set1.Contains(currentLocation) && !set2.Contains(currentLocation))
                {
                    result.Add(currentLocation);
                }
            }
        }

        return result;
    }
}
