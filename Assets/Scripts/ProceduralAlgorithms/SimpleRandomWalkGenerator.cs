using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Random = UnityEngine.Random;
using Unity.VisualScripting;
using UnityEngine.AI;
using UnityEngine.Tilemaps;

public class SimpleRandomWalkGenerator : AbstractGenerator
{
    public override void RunProceduralGeneration(Map map, Vector2Int startPosition)
    {
        HashSet<Vector2Int> floorPositions = RunRandomWalk(startPosition);
        //Set map floor positions and boundries
        map.SetFloorPositions(floorPositions);
        HashSet<Vector2Int> walls = WallGenerator.FindOutSideWalls(floorPositions, map.boundaries.GetBoundaries());
        HashSet<Vector2Int> islands = BoundryCalculator.GetGridLocationsNotInBoundaries(floorPositions, walls, map.boundaries.GetBoundaries());
        map.SetMap(walls, islands);
    }

    public override void DrawMapObjects(Map map)
    {
        //Partition map should occur after map is shifted
        map.SetPartitions(mapPartitions);
        //Draw walls and islands
        WallGenerator.CreateWalls(map.floorPositions, tileMapVisualizer, map.wallPositions, map.islandPositions);

        //draw background
        map.background.drawBackground(backgroundPrefab, map.boundaries);

        //get hawk positions
        List<Vector2Int> hawkPositions = map.SetHawkPositions();

        //Spawn hawks
        map.spawner.SpawnObjects(hawkPositions, hawkPrefab);
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

}
