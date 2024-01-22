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

    protected Spawner spawner;
    protected DrawBackground background;

    public Map Generate()
    {
        // Check if spawner is not assigned
        if (spawner == null)
        {
            // Try to find the Spawner component on the current GameObject
            spawner = GetComponent<Spawner>();

            // If it's still null, add the Spawner component dynamically
            if (spawner == null)
            {
                spawner = gameObject.AddComponent<Spawner>();
            }
        }

        if (background == null)
        {
            // Try to find the Spawner component on the current GameObject
            background = GetComponent<DrawBackground>();

            // If it's still null, add the Spawner component dynamically
            if (background == null)
            {
                background = gameObject.AddComponent<DrawBackground>();
            }
        }

        Map map = new Map(tileMapVisualizer, spawner, background);

        return map;
    }


    public void ClearTiles()
    {
        tileMapVisualizer.Clear();
        spawner.ClearObjects();
        background.clearBackground();
    }

    public abstract void RunProceduralGeneration(Map map, Vector2Int startPosition);
    public abstract void DrawMapObjects(Map map);
}
