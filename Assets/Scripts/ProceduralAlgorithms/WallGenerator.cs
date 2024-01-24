using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WallGenerator : MonoBehaviour
{
    public HashSet<Vector2Int> FindOutSideWalls(HashSet<Vector2Int> floorPositions, (Vector2Int, Vector2Int, Vector2Int, Vector2Int) boundaries)
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

        // Coroutine 4
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


