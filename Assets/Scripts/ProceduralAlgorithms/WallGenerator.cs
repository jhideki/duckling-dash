using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public static class WallGenerator
{

    public static void CreateWalls(HashSet<Vector2Int> floorPositions, TileMapVisualizer tileMapVisualizer)
    {
        var boundaries = BoundryCalculator.GetCornerBoundaries(floorPositions);
        var outsideWalls = FindOutSideWalls(floorPositions, boundaries);
        var islandWalls = BoundryCalculator.GetGridLocationsNotInBoundaries(floorPositions, outsideWalls, boundaries);

        foreach (var wall in outsideWalls)
        {
            tileMapVisualizer.PaintSingleBasicWall(wall);
        }

        foreach (var wall in islandWalls)
        {
            tileMapVisualizer.PaintSingleBasicWall(wall);
        }

    }

    public static HashSet<Vector2Int> FindOutSideWalls(HashSet<Vector2Int> floorPositions, (Vector2Int, Vector2Int, Vector2Int, Vector2Int) boundaries)
    {
        HashSet<Vector2Int> outsideWalls = new HashSet<Vector2Int>();

        for (int i = boundaries.Item1.x; i <= boundaries.Item2.x; i++)
        {
            for (int j = boundaries.Item1.y; j >= boundaries.Item3.y; j--)
            {
                Vector2Int position = new Vector2Int(i, j);
                if (floorPositions.Contains(position) == false)
                {
                    outsideWalls.Add(position);
                }
                else
                {
                    break;
                }
            }
        }

        for (int i = boundaries.Item1.x; i <= boundaries.Item2.x; i++)
        {
            for (int j = boundaries.Item3.y; j <= boundaries.Item1.y; j++)
            {
                Vector2Int position = new Vector2Int(i, j);
                if (floorPositions.Contains(position) == false)
                {
                    outsideWalls.Add(position);
                }
                else
                {
                    break;
                }
            }
        }

        for (int i = boundaries.Item3.y; i <= boundaries.Item1.y; i++)
        {
            for (int j = boundaries.Item1.x; j <= boundaries.Item2.x; j++)
            {
                Vector2Int position = new Vector2Int(j, i);
                if (floorPositions.Contains(position) == false)
                {
                    outsideWalls.Add(position);
                }
                else
                {
                    break;
                }
            }
        }

        for (int i = boundaries.Item3.y; i <= boundaries.Item1.y; i++)
        {
            for (int j = boundaries.Item2.x; j >= boundaries.Item1.x; j--)
            {
                Vector2Int position = new Vector2Int(j, i);
                if (floorPositions.Contains(position) == false)
                {
                    outsideWalls.Add(position);
                }
                else
                {
                    break;
                }
            }
        }
        return outsideWalls;
    }

}