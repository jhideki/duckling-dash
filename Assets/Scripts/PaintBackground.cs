using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PaintBackground : MonoBehaviour
{
    public Camera mainCamera;
    public Tilemap tilemap;
    public Tile[] tiles;
    public float animationSpeed = 1.0f;

    private BoundsInt cameraBounds;
    private Coroutine animationCoroutine;

    void Start()
    {
        if (mainCamera == null || tilemap == null || tiles.Length == 0)
        {
            Debug.LogError("Camera, Tilemap, or Tiles not assigned!");
            return;
        }

        // Calculate the camera bounds in tile coordinates
        cameraBounds = CalculateCameraBounds();

        // Paint tiles within the camera bounds
        PaintTilesInBounds();

        // Start the animation coroutine
        animationCoroutine = StartCoroutine(AnimateTiles());
    }

    void Update()
    {
        // Check if the camera moved to update the painted tiles
        BoundsInt newBounds = CalculateCameraBounds();
        if (newBounds != cameraBounds)
        {
            cameraBounds = newBounds;
            PaintTilesInBounds();
        }
    }

    void PaintTilesInBounds()
    {
        tilemap.ClearAllTiles();

        for (int x = cameraBounds.x; x < cameraBounds.xMax; x++)
        {
            for (int y = cameraBounds.y; y < cameraBounds.yMax; y++)
            {
                Vector3Int tilePosition = new Vector3Int(x, y, 0);
                tilemap.SetTile(tilePosition, GetRandomTile());
            }
        }
    }

    Tile GetRandomTile()
    {
        return tiles[Random.Range(0, tiles.Length)];
    }

    BoundsInt CalculateCameraBounds()
    {
        Vector3 cameraPosition = mainCamera.transform.position;
        Vector3 cameraSize = mainCamera.ViewportToWorldPoint(new Vector3(1, 1, cameraPosition.z)) - cameraPosition;

        Vector3Int min = tilemap.WorldToCell(cameraPosition - cameraSize);
        Vector3Int max = tilemap.WorldToCell(cameraPosition + cameraSize);

        return new BoundsInt(min.x, min.y, 0, max.x - min.x, max.y - min.y, 1);
    }

    IEnumerator AnimateTiles()
    {
        while (true)
        {
            yield return new WaitForSeconds(1 / animationSpeed);

            for (int x = cameraBounds.x; x < cameraBounds.xMax; x++)
            {
                for (int y = cameraBounds.y; y < cameraBounds.yMax; y++)
                {
                    Vector3Int tilePosition = new Vector3Int(x, y, 0);
                    tilemap.SetTile(tilePosition, GetRandomTile());
                }
            }
        }
    }
}
