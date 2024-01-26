using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractGenerator : MonoBehaviour
{
    [SerializeField] public TileMapVisualizer tileMapVisualizer = null;
    [SerializeField] public SimpleRAndomWalkSO randomWalkParameters;
    [SerializeField] public GameObject hawkPrefab;
    [SerializeField] public GameObject backgroundPrefab;
    [SerializeField] public GameObject bushPrefab;
    [SerializeField] public GameObject duckPrefab;
    [SerializeField] public GameObject hunterPrefab;

    [SerializeField] public GameObject nestPrefab;
    [SerializeField] public int mapPartitions;
    [SerializeField] public int minBushes = 10;
    [SerializeField] public int maxBushes = 15;
    [SerializeField] public int numHunters = 3;
    [SerializeField] public int numWaterObjects = 10;
    [SerializeField] public int numNests = 2;
    public bool isLoading;
    public WallGenerator wallGenerator;

    protected DrawBackground background;

    public Map Generate()
    {
        if (gameObject.GetComponent<WallGenerator>() == null)
        {
            wallGenerator = gameObject.AddComponent<WallGenerator>();
        }
        Spawner spawner = gameObject.AddComponent<Spawner>();

        DrawBackground background = gameObject.AddComponent<DrawBackground>();

        Map map = new Map(tileMapVisualizer, spawner, background);

        return map;
    }

    public IEnumerator DrawMapObjects(Map map)
    {
        isLoading = true;
        //Partition map should occur after map is shifted
        map.SetPartitions(mapPartitions);

        //draw background
        map.background.drawBackground(backgroundPrefab, map.boundaries);
        //Draw walls and islands
        yield return StartCoroutine(tileMapVisualizer.PaintTilesAsync(map.wallPositions));

        yield return StartCoroutine(tileMapVisualizer.PaintTilesAsync(map.islandPositions));
        yield return StartCoroutine(tileMapVisualizer.PaintFoliageTiles(map.wallPositions, map));

        tileMapVisualizer.ClearWallTiles(map.corridorPositions, map);

        //get hawk positions
        List<Vector2Int> hawkPositions = map.SetHawkPositions();

        int numBushes = Random.Range(minBushes, maxBushes);

        //get bush positions
        List<Vector2Int> bushPositions = ProceduralGenerationAlgorithms.SetBushPositionsModified(numBushes, map.floorPositions, map.wallPositions, map.islandPositions);
        map.SetBushPositions(bushPositions);
        List<Vector2Int> nestPositions = ProceduralGenerationAlgorithms.SetNestPositions(numNests, map);

        //Spawn hawks
        yield return StartCoroutine(map.spawner.SpawnObjects(hawkPositions, hawkPrefab));
        yield return StartCoroutine(map.spawner.SpawnObjects(bushPositions, bushPrefab));
        yield return StartCoroutine(map.spawner.SpawnObjects(bushPositions, duckPrefab));
        yield return StartCoroutine(map.spawner.SpawnObjects(nestPositions, nestPrefab));

        yield return StartCoroutine(tileMapVisualizer.PaintWaterObjects(map, numWaterObjects));
        //get hunter positions
        List<Vector2Int> hunterPositions = map.SetHunterPositions(numHunters);
        yield return StartCoroutine(map.spawner.SpawnObjects(hunterPositions, hunterPrefab));
        isLoading = false;
    }

    public IEnumerator DrawMapObjects(Map map, Map map2)
    {
        //Partition map should occur after map is shifted
        map.SetPartitions(mapPartitions);

        //Draw walls and islands
        yield return StartCoroutine(tileMapVisualizer.PaintTilesAsync(map.wallPositions));

        yield return StartCoroutine(tileMapVisualizer.PaintTilesAsync(map.islandPositions));

        tileMapVisualizer.ClearWallTiles(map.corridorPositions, map2);

        tileMapVisualizer.ClearWallTiles(map2.corridorPositions, map2);
        tileMapVisualizer.ClearFoliage(map.corridorPositions, map);
        tileMapVisualizer.ClearFoliage(map2.corridorPositions, map2);
        tileMapVisualizer.ClearFoliage(map2.foliagePositions, map2);

        yield return StartCoroutine(tileMapVisualizer.PaintFoliageTiles(map.wallPositions, map));
        //draw background
        map.background.drawBackground(backgroundPrefab, map.boundaries);

        //get hawk positions
        List<Vector2Int> hawkPositions = map.SetHawkPositions();

        int numBushes = Random.Range(minBushes, maxBushes);

        //get bush positions
        List<Vector2Int> bushPositions = ProceduralGenerationAlgorithms.SetBushPositionsModified(numBushes, map.floorPositions, map.wallPositions, map.islandPositions);
        map.SetBushPositions(bushPositions);

        List<Vector2Int> nestPositions = ProceduralGenerationAlgorithms.SetNestPositions(numNests, map);
        //Spawn hawks
        yield return StartCoroutine(map.spawner.SpawnObjects(hawkPositions, hawkPrefab));
        yield return StartCoroutine(map.spawner.SpawnObjects(bushPositions, bushPrefab));
        yield return StartCoroutine(map.spawner.SpawnObjects(bushPositions, duckPrefab));

        yield return StartCoroutine(map.spawner.SpawnObjects(nestPositions, nestPrefab));
        yield return StartCoroutine(tileMapVisualizer.PaintWaterObjects(map, numWaterObjects));
        //get hunter positions
        List<Vector2Int> hunterPositions = map.SetHunterPositions(numHunters);
        yield return StartCoroutine(map.spawner.SpawnObjects(hunterPositions, hunterPrefab));

    }

    public IEnumerator FillCorridor(Map map)
    {
        //paint corridor
        yield return StartCoroutine(tileMapVisualizer.PaintTilesAsync(map.corridorPositions));

        //update map data
        //map.PaintCorridor();
    }


    public void ClearTiles()
    {
        tileMapVisualizer.Clear();
        background.clearBackground();
    }

    public abstract void RunProceduralGeneration(Map map, Vector2Int startPosition);
    public abstract void CreateCorridor(Map map, int mapEdge, Map map2);
}
