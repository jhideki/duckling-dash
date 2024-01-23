using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public static class ProceduralGenerationAlgorithms
{
    public static HashSet<Vector2Int> SimpleRandomWalk(Vector2Int startPosition, int walkLength)
    {
        HashSet<Vector2Int> path = new HashSet<Vector2Int>();

        path.Add(startPosition);
        var previousPosition = startPosition;

        for (int i = 0; i < walkLength; i++)
        {
            var newPosition = previousPosition + Direction2D.GetRandomCardinalDirection();
            path.Add(newPosition);
            path.Add(new Vector2Int(newPosition.x + 1, newPosition.y + 1));
            previousPosition = newPosition;
        }
        return path;
    }

    public static HashSet<Vector2Int> Corridor(Map map, Vector2Int mapEdge, int direction)
    {
        HashSet<Vector2Int> path = new HashSet<Vector2Int>();
        Vector2Int currentPosition = mapEdge;
        int count = 0;
        while (!map.floorPositions.Contains(currentPosition) && count < 1000)
        {
            path.Add(currentPosition);

            Vector2Int thicken;
            int num = Random.Range(1, 4);

            if (direction == 1)
            {
                currentPosition.x -= 1;
                thicken = new Vector2Int(currentPosition.x, currentPosition.y + 1);
                path.Add(thicken);
                thicken = new Vector2Int(currentPosition.x, currentPosition.y - 1);
                path.Add(thicken);
                if (num == 1)
                {
                    thicken = new Vector2Int(currentPosition.x, currentPosition.y + 2);
                    path.Add(thicken);
                }
                else if (num == 2)
                {
                    thicken = new Vector2Int(currentPosition.x, currentPosition.y - 2);
                    path.Add(thicken);
                }

            }
            else if (direction == 2)
            {

                currentPosition.x += 1;
                thicken = new Vector2Int(currentPosition.x, currentPosition.y + 1);
                path.Add(thicken);
                thicken = new Vector2Int(currentPosition.x, currentPosition.y - 1);
                path.Add(thicken);
                if (num == 1)
                {
                    thicken = new Vector2Int(currentPosition.x, currentPosition.y + 2);
                    path.Add(thicken);
                }
                else if (num == 2)
                {
                    thicken = new Vector2Int(currentPosition.x, currentPosition.y - 2);
                    path.Add(thicken);
                }
            }
            else if (direction == 3)
            {
                currentPosition.y -= 1;
                thicken = new Vector2Int(currentPosition.x + 1, currentPosition.y);
                path.Add(thicken);
                thicken = new Vector2Int(currentPosition.x - 1, currentPosition.y);
                path.Add(thicken);
                if (num == 1)
                {
                    thicken = new Vector2Int(currentPosition.x + 2, currentPosition.y);

                    path.Add(thicken);
                }
                else if (num == 2)
                {
                    thicken = new Vector2Int(currentPosition.x - 2, currentPosition.y);
                    path.Add(thicken);
                }
            }
            else
            {
                currentPosition.y += 1;
                thicken = new Vector2Int(currentPosition.x + 1, currentPosition.y);
                path.Add(thicken);
                thicken = new Vector2Int(currentPosition.x - 1, currentPosition.y);
                path.Add(thicken);
                if (num == 1)
                {
                    thicken = new Vector2Int(currentPosition.x + 2, currentPosition.y);
                    path.Add(thicken);
                }
                else if (num == 2)
                {
                    thicken = new Vector2Int(currentPosition.x + 2, currentPosition.y);
                    path.Add(thicken);
                }
            }

            count++;
        }
        map.SetCorridorLenth(count);



        return path;

    }

}

public static class Direction2D
{
    public static List<Vector2Int> cardinalDirectionsList = new List<Vector2Int>
    {
        new Vector2Int(0,1), //UP
        new Vector2Int(1,0), //RIGHT
        new Vector2Int(0, -1), // DOWN
        new Vector2Int(-1, 0) //LEFT
    };

    public static List<Vector2Int> diagonalDirectionsList = new List<Vector2Int>
    {
        new Vector2Int(1,1), //UP-RIGHT
        new Vector2Int(1,-1), //RIGHT-DOWN
        new Vector2Int(-1, -1), // DOWN-LEFT
        new Vector2Int(-1, 1) //LEFT-UP
    };

    public static List<Vector2Int> eightDirectionsList = new List<Vector2Int>
    {
        new Vector2Int(0,1), //UP
        new Vector2Int(1,1), //UP-RIGHT
        new Vector2Int(1,0), //RIGHT
        new Vector2Int(1,-1), //RIGHT-DOWN
        new Vector2Int(0, -1), // DOWN
        new Vector2Int(-1, -1), // DOWN-LEFT
        new Vector2Int(-1, 0), //LEFT
        new Vector2Int(-1, 1) //LEFT-UP

    };

    public static Vector2Int GetRandomCardinalDirection()
    {
        return cardinalDirectionsList[UnityEngine.Random.Range(0, cardinalDirectionsList.Count)];
    }
}