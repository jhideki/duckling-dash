using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WallGenerator : MonoBehaviour
{
    public void CreateWalls(HashSet<Vector2Int> floorPositions, TileMapVisualizer tileMapVisualizer, HashSet<Vector2Int> outsideWalls, HashSet<Vector2Int> islandWalls)
    {
        foreach (var wall in outsideWalls)
        {
            tileMapVisualizer.PaintSingleBasicWall(wall);
        }

        foreach (var wall in islandWalls)
        {
            tileMapVisualizer.PaintSingleBasicWall(wall);
        }
    }

    public HashSet<Vector2Int> FindOutSideWalls(HashSet<Vector2Int> floorPositions, (Vector2Int, Vector2Int, Vector2Int, Vector2Int) boundaries)
    {
        HashSet<Vector2Int> outsideWalls = new HashSet<Vector2Int>();

        // Coroutine 1
        IEnumerator Coroutine1()
        {
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

                yield return null;
            }
        }

        // Coroutine 2
        IEnumerator Coroutine2()
        {
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

                yield return null;
            }
        }

        // Coroutine 3
        IEnumerator Coroutine3()
        {
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

                yield return null;
            }
        }

        // Coroutine 4
        IEnumerator Coroutine4()
        {
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

                yield return null;
            }
        }

        // Run coroutines
        StartCoroutine(Coroutine1());
        StartCoroutine(Coroutine2());
        StartCoroutine(Coroutine3());
        StartCoroutine(Coroutine4());

        return outsideWalls;
    }

}


