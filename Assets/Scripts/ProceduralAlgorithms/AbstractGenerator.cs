using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractGenerator : MonoBehaviour
{
    [SerializeField] public TileMapVisualizer tileMapVisualizer = null;
    [SerializeField] public SimpleRAndomWalkSO randomWalkParameters;
    [SerializeField] public GameObject hawkPrefab;
    [SerializeField] public GameObject backgroundPrefab;
    [SerializeField] public int mapPartitions;
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


    public void ClearTiles()
    {
        tileMapVisualizer.Clear();
        background.clearBackground();
    }

    public abstract void RunProceduralGeneration(Map map, Vector2Int startPosition);
    public abstract void DrawMapObjects(Map map);
    public abstract void DrawCorridor(Map map, Map map2);
    public abstract void CreateCorridor(Map map, int mapEdge, Map map2);
}
