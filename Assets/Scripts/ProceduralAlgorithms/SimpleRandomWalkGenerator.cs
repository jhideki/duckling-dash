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
    [SerializeField] private SimpleRAndomWalkSO randomwWalkParameters;
    [SerializeField] private GameObject hawkPrefab;
    [SerializeField] private GameObject backgroundPrefab;
    [SerializeField] private int mapPartitions;
    static private (Vector2Int, Vector2Int, Vector2Int, Vector2Int) boundaries;
    public BoundaryData boundaryData;
    private Boundaries mapBoundaries;
    private Map map;

    protected override void RunProceduralGeneration()
    {
        HashSet<Vector2Int> floorPositions = RunRandomWalk();
        mapBoundaries = BoundryCalculator.GetCornerBoundaries(floorPositions);

        // Load boundary data for runtime use
        boundaryData.boundaries.topRight = mapBoundaries.topRight;
        boundaryData.boundaries.topLeft = mapBoundaries.topLeft;
        boundaryData.boundaries.bottomLeft = mapBoundaries.bottomLeft;
        boundaryData.boundaries.bottomRight = mapBoundaries.bottomRight;

        //Draw walls and islands
        (HashSet<Vector2Int>, HashSet<Vector2Int>) walls = WallGenerator.CreateWalls(floorPositions, tileMapVisualizer, mapBoundaries.GetBoundaries());
        //Save wall, island and floor in map class
        map = new Map(mapBoundaries, floorPositions, walls.Item1, walls.Item2, mapPartitions);

        //Draw background
        background.drawBackground(backgroundPrefab, mapBoundaries);

        //get hawk positions
        List<Vector2Int> hawkPositions = map.SetHawkPositions();

        //Spawn hawks
        spawner.SpawnObjects(hawkPositions, hawkPrefab);
    }

    protected HashSet<Vector2Int> RunRandomWalk()
    {
        var currentPosition = startPosition;
        HashSet<Vector2Int> floorPositions = new HashSet<Vector2Int>();
        for (int i = 0; i < randomwWalkParameters.iterations; i++)
        {
            var path = ProceduralGenerationAlgorithms.SimpleRandomWalk(currentPosition, randomwWalkParameters.walkLength);
            floorPositions.UnionWith(path);

            if (randomwWalkParameters.startRandomlyEachIteration)
            {
                currentPosition = floorPositions.ElementAt(Random.Range(0, floorPositions.Count));
            }
        }

        return floorPositions;
    }

}
