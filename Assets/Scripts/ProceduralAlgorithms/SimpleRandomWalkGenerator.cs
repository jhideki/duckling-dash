using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Random = UnityEngine.Random;
using Unity.VisualScripting;

public class SimpleRandomWalkGenerator : AbstractGenerator
{
    [SerializeField] private SimpleRAndomWalkSO randomwWalkParameters;
    static public (Vector2Int, Vector2Int, Vector2Int, Vector2Int) boundaries;
    public BoundaryData boundaryData;


    protected override void RunProceduralGeneration()
    {
        HashSet<Vector2Int> floorPositions = RunRandomWalk();
        boundaries = BoundryCalculator.GetCornerBoundaries(floorPositions);

        // Load boundary data for runtime use
        boundaryData.topLeft = boundaries.Item1;
        boundaryData.topRight = boundaries.Item2;
        boundaryData.bottomLeft = boundaries.Item3;
        boundaryData.bottomRight = boundaries.Item4;

        tileMapVisualizer.Clear();
        //tileMapVisualizer.PaintFloorTiles(floorPositions);
        WallGenerator.CreateWalls(floorPositions, tileMapVisualizer, boundaries);
    }

    public static (Vector2Int, Vector2Int, Vector2Int, Vector2Int) GetCornerBoundaries()
    {
        return boundaries;
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
