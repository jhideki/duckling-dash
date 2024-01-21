using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractGenerator : MonoBehaviour
{
    [SerializeField] protected TileMapVisualizer tileMapVisualizer = null;
    [SerializeField] protected Vector2Int startPosition = Vector2Int.zero;

    protected Spawner spawner;
    protected DrawBackground background;

    public void Generate()
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

        //Clear existing map before drawing a new one
        spawner.ClearObjects();
        background.clearBackground();
        tileMapVisualizer.Clear();

        RunProceduralGeneration();
    }

    protected abstract void RunProceduralGeneration();
}
