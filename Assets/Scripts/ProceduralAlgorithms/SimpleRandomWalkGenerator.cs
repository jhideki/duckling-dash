using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Random = UnityEngine.Random;
using Unity.VisualScripting;
using UnityEngine.AI;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class SimpleRandomWalkGenerator : AbstractGenerator
{
    public override void RunProceduralGeneration(Map map, Vector2Int startPosition)
    {
        HashSet<Vector2Int> floorPositions = RunRandomWalk(startPosition);
        //Set map floor positions and boundries
        map.SetFloorPositions(floorPositions);
        HashSet<Vector2Int> walls = wallGenerator.FindOutSideWalls(floorPositions, map.boundaries.GetBoundaries());
        HashSet<Vector2Int> islands = BoundryCalculator.GetGridLocationsNotInBoundaries(floorPositions, walls, map.boundaries.GetBoundaries());
        map.SetMap(islands, walls);
    }

    public override void DrawMapObjects(Map map)
    {
        //Partition map should occur after map is shifted
        map.SetPartitions(mapPartitions);

        //Draw walls and islands
        wallGenerator.CreateWalls(map.floorPositions, map.tileMapVisualizer, map.wallPositions, map.islandPositions);

        //draw background
        map.background.drawBackground(backgroundPrefab, map.boundaries);

        //get hawk positions
        List<Vector2Int> hawkPositions = map.SetHawkPositions();

        //Spawn hawks
        map.spawner.SpawnObjects(hawkPositions, hawkPrefab);
    }

    public override void DrawCorridor(Map map, Map map2)
    {
        tileMapVisualizer.ClearWallTiles(map.corridorPositions);
        tileMapVisualizer.ClearWallTiles(map2.corridorPositions);
    }

    protected HashSet<Vector2Int> RunRandomWalk(Vector2Int startPosition)
    {
        var currentPosition = startPosition;
        HashSet<Vector2Int> floorPositions = new HashSet<Vector2Int>();
        for (int i = 0; i < randomWalkParameters.iterations; i++)
        {
            var path = ProceduralGenerationAlgorithms.SimpleRandomWalk(currentPosition, randomWalkParameters.walkLength);
            floorPositions.UnionWith(path);

            if (randomWalkParameters.startRandomlyEachIteration)
            {
                currentPosition = floorPositions.ElementAt(Random.Range(0, floorPositions.Count));
            }
        }

        return floorPositions;
    }

    public override void CreateCorridor(Map map, int side, Map map2)
    {

        Vector2Int startLocation;
        switch (side)
        {
            // Right side
            case 1:
                startLocation = new Vector2Int(map.boundaries.topRight.x, (map.boundaries.topRight.y + map.boundaries.bottomRight.y) / 2);
                break;
            // Left side
            case 2:
                startLocation = new Vector2Int(map.boundaries.topLeft.x, (map.boundaries.topLeft.y + map.boundaries.bottomLeft.y) / 2);
                break;
            // Top side
            case 3:
                startLocation = new Vector2Int((map.boundaries.topRight.x + map.boundaries.topLeft.x) / 2, map.boundaries.topRight.y);
                break;
            // Bottom side
            default:
                startLocation = new Vector2Int((map.boundaries.bottomRight.x + map.boundaries.bottomLeft.x) / 2, map.boundaries.bottomRight.y);
                break;
        }

        HashSet<Vector2Int> corridor = ProceduralGenerationAlgorithms.Corridor(map, startLocation, side);

        //update the direction to paint corridor
        switch (side)
        {
            case 1:
                side = 2;
                break;
            case 2:
                side = 1;
                break;
            case 3:
                side = 4;
                break;
            default:
                side = 3;
                break;
        }
        HashSet<Vector2Int> corridor2 = ProceduralGenerationAlgorithms.Corridor(map2, startLocation, side);
        map.SetCorridor(corridor);
        map2.SetCorridor(corridor2);
    }
}