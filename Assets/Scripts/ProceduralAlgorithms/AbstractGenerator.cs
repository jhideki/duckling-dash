using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractGenerator : MonoBehaviour
{
    [SerializeField] protected TileMapVisualizer tileMapVisualizer = null;
    [SerializeField] protected Vector2Int startPosition = Vector2Int.zero;

    public void Generate()
    {
        tileMapVisualizer.Clear();
        RunProceduralGeneration();
    }

    protected abstract void RunProceduralGeneration();
}
