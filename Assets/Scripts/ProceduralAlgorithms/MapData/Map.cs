using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Map
{
    public Boundaries boundaries;
    public List<HashSet<Vector2Int>> partitions;
    public HashSet<Vector2Int> floorPositions;
    public HashSet<Vector2Int> islandPositions;
    public HashSet<Vector2Int> wallPositions;
    public List<Vector2Int> hawkPositions;

    public Map(Boundaries boundaries)
    {
        this.boundaries = boundaries;
    }

    //Constructor method
    public Map(Boundaries boundaries, HashSet<Vector2Int> floorPositions, HashSet<Vector2Int> islandPositions, HashSet<Vector2Int> wallPositions, int divisions)
    {
        this.boundaries = boundaries;
        this.floorPositions = floorPositions;
        this.islandPositions = islandPositions;
        this.wallPositions = wallPositions;
        this.partitions = PartitionMap(divisions);
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


}
