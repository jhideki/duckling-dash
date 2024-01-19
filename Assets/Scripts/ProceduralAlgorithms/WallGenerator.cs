using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public static class WallGenerator
{
    public static void CreateWalls(HashSet<Vector2Int> floorPositions, TileMapVisualizer tileMapVisualizer)
    {
        var basicWallPositions = FindWallsInDirections(floorPositions, Direction2D.cardinalDirectionsList);
        foreach (var position in basicWallPositions)
        {
            tileMapVisualizer.PaintSingleBasicWall(position);
        }
    }
    private static HashSet<Vector2Int> FindWallsInDirections(HashSet<Vector2Int> floorPositions, List<Vector2Int> directionList)
    {
        HashSet<Vector2Int> wallPositions = new HashSet<Vector2Int>();
        foreach (var position in floorPositions)
        {
            foreach (var direction in directionList)
            {
                var neighborPosition = position + direction;
                if (floorPositions.Contains(neighborPosition) == false)
                {
                    if (direction.x > 0)
                    {
                        wallPositions.Add(new Vector2Int(neighborPosition.x + 1, neighborPosition.y));
                    }

                    wallPositions.Add(neighborPosition);
                    if (direction.y > 0)
                    {
                        wallPositions.Add(new Vector2Int(neighborPosition.x, neighborPosition.y + 1));
                    }

                    if (direction.x < 0)
                    {
                        wallPositions.Add(new Vector2Int(neighborPosition.x - 1, neighborPosition.y));
                    }

                    if (direction.y < 0)
                    {
                        wallPositions.Add(new Vector2Int(neighborPosition.x, neighborPosition.y - 1));
                    }
                }
            }
        }
        return wallPositions;
    }
}