using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Map
{
    public Boundaries boundaries;
    public List<HashSet<Vector2Int>> partitions;
    public HashSet<Vector2Int> floorPositions;
    public HashSet<Vector2Int> islandPositions;
    public HashSet<Vector2Int> wallPositions;
    public HashSet<Vector2Int> corridorPositions;
    public List<Vector2Int> hawkPositions;
    public List<Vector2Int> bushPositions;
    public Spawner spawner;
    public DrawBackground background;
    public TileMapVisualizer tileMapVisualizer;
    public int corridorLength;

    public Map(TileMapVisualizer tileMapVisualizer, Spawner spawner, DrawBackground background)
    {
        this.tileMapVisualizer = tileMapVisualizer;
        this.spawner = spawner;
        this.background = background;
    }

    public void SetFloorPositions(HashSet<Vector2Int> floorPositions)
    {
        this.floorPositions = floorPositions;
        this.boundaries = BoundryCalculator.GetCornerBoundaries(floorPositions);
    }

    public void SetPartitions(int division)
    {
        this.partitions = PartitionMap(division);
    }

    public void SetCorridor(HashSet<Vector2Int> corridorPositions)
    {
        this.corridorPositions = corridorPositions;
        foreach (var position in corridorPositions)
        {
            if (wallPositions.Contains(position))
            {
                wallPositions.Remove(position);
            }
            if (islandPositions.Contains(position))
            {
                islandPositions.Remove(position);
            }
        }
    }

    public void SetCorridorLenth(int corridorLength)
    {
        this.corridorLength = corridorLength;
    }


    //Constructor method
    public void SetMap(HashSet<Vector2Int> islandPositions, HashSet<Vector2Int> wallPositions)
    {
        this.islandPositions = islandPositions;
        this.wallPositions = wallPositions;
        this.hawkPositions = new List<Vector2Int>();
    }

    // Partitions map int x and y divisions. E.g., division X division size grid
    private List<HashSet<Vector2Int>> PartitionMap(int divisions)
    {
        List<HashSet<Vector2Int>> mapPartitions = new List<HashSet<Vector2Int>>();

        int xSize = boundaries.topRight.x - boundaries.topLeft.x;
        int ySize = boundaries.topLeft.y - boundaries.bottomLeft.y;

        int xPartitionSize = xSize / divisions;
        int yPartitionSize = ySize / divisions;

        // Start partitioning from boundaries.topRight
        for (int i = 0; i < divisions; i++)
        {
            for (int j = 0; j < divisions; j++)
            {
                // Calculate the starting position for the current partition
                int startX = boundaries.topLeft.x + i * xPartitionSize;
                int startY = boundaries.topLeft.y - j * yPartitionSize;

                // Create a HashSet for each partition
                HashSet<Vector2Int> partition = new HashSet<Vector2Int>();

                // Fill the partition with grid locations
                for (int x = startX; x < startX + xPartitionSize; x++)
                {
                    for (int y = startY; y > startY - yPartitionSize; y--)
                    {
                        partition.Add(new Vector2Int(x, y));
                    }
                }

                // Add the partition to the list
                mapPartitions.Add(partition);
            }
        }

        return mapPartitions;
    }

    public void ShiftMap(int xShift, int yShift)
    {
        HashSet<Vector2Int> temp = new HashSet<Vector2Int>();

        //Shift boundareis
        boundaries.topLeft = new Vector2Int(boundaries.topLeft.x + xShift, boundaries.topLeft.y + yShift);
        boundaries.topRight = new Vector2Int(boundaries.topRight.x + xShift, boundaries.topRight.y + yShift);
        boundaries.bottomRight = new Vector2Int(boundaries.bottomRight.x + xShift, boundaries.bottomRight.y + yShift);
        boundaries.bottomLeft = new Vector2Int(boundaries.bottomLeft.x + xShift, boundaries.bottomLeft.y + yShift);

        foreach (var floor in floorPositions)
        {
            Vector2Int shiftedPosition = new Vector2Int(floor.x + xShift, floor.y + yShift);
            temp.Add(shiftedPosition);
        }
        floorPositions.Clear();
        floorPositions.UnionWith(temp);
        temp.Clear();

        foreach (var wall in wallPositions)
        {
            Vector2Int shiftedPosition = new Vector2Int(wall.x + xShift, wall.y + yShift);
            temp.Add(shiftedPosition);
        }
        wallPositions.Clear();
        wallPositions.UnionWith(temp);
        temp.Clear();

        foreach (var island in islandPositions)
        {
            Vector2Int shiftedPosition = new Vector2Int(island.x + xShift, island.y + yShift);
            temp.Add(shiftedPosition);
        }
        islandPositions.Clear();
        islandPositions.UnionWith(temp);
        temp.Clear();

    }

    //Sets random hawk position within each partition
    public List<Vector2Int> SetHawkPositions()
    {
        foreach (var partition in partitions)
        {
            // Check if the partition is not empty
            if (partition.Count > 0)
            {
                // Convert the partition to a list and select a random position
                List<Vector2Int> partitionList = partition.ToList();
                Vector2Int randomPosition = partitionList[Random.Range(0, partitionList.Count)];

                // Add the random position to hawkPositions
                hawkPositions.Add(randomPosition);
            }
        }
        return hawkPositions;
    }

    public void SetBushPositions(List<Vector2Int> bushPositions)
    {
        this.bushPositions = bushPositions;
    }


    public void ClearMap()
    {
        spawner.ClearObjects();
        background.clearBackground();
        //clear wall position refers to any grass block
        if (wallPositions != null && floorPositions != null)
        {
            foreach (var walls in wallPositions)
            {
                tileMapVisualizer.ClearWallTile(walls);
            }

            foreach (var floor in floorPositions)
            {
                tileMapVisualizer.ClearWallTile(floor);
            }

            foreach (var position in islandPositions)
            {
                tileMapVisualizer.ClearWallTile(position);
            }

            foreach (var position in corridorPositions)
            {
                tileMapVisualizer.ClearWallTile(position);
            }


        }
        else
        {
            tileMapVisualizer.Clear();
        }

    }

    public void PaintCorridor()
    {
        foreach (var position in new HashSet<Vector2Int>(corridorPositions))
        {
            corridorPositions.Remove(position);
            wallPositions.Add(position);
        }

    }

}
