using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileMapVisualizer : MonoBehaviour
{
    [SerializeField] private Tilemap floorTilemap, wallTilemap, debugTilemap, foliageTilemap;
    [SerializeField] private TileBase floorTile, wallTop, debugTile, foliageTile1, foliageTile2, folliageTile3, folliagetile4, rockTile, woodtTile;

    public void PaintFloorTiles(IEnumerable<Vector2Int> floorPositions)
    {
        PaintTiles(floorPositions, floorTilemap, floorTile);
    }

    public Vector3 GetCellCenterWorld(Vector3Int position)
    {
        return floorTilemap.GetCellCenterWorld(position);
    }

    public bool IsFloorTile(Vector3Int position)
    {
        return floorTilemap.GetTile(position) != null;
    }
    public IEnumerator PaintFoliageTiles(HashSet<Vector2Int> positions)
    {
        foreach (var position in positions)
        {
            int random = Random.Range(0, 30);
            if (random == 1 || random == 2)
            {
                PaintSingleTile(foliageTilemap, foliageTile1, position);
            }
            else if (random == 4 || random == 3)
            {
                PaintSingleTile(foliageTilemap, foliageTile2, position);
            }
            else if (random == 5)
            {
                PaintSingleTile(foliageTilemap, folliageTile3, position);
            }
            yield return null;
        }

    }

    public IEnumerator PaintWaterObjects(Map map, int numObjects)
    {
        List<Vector2Int> waterPositions = new List<Vector2Int>(map.floorPositions);
        HashSet<Vector2Int> objPositions = new HashSet<Vector2Int>();
        for (int i = 0; i < numObjects; i++)
        {
            Vector2Int pos = new Vector2Int();
            int random = Random.Range(0, waterPositions.Count);
            int random2 = Random.Range(1, 4);
            pos = waterPositions[random];
            if (!objPositions.Contains(pos))
            {
                Debug.Log("this never runs");
                if (random2 == 1)
                {
                    PaintSingleTile(foliageTilemap, rockTile, pos);
                }
                else if (random2 == 2)
                {
                    PaintSingleTile(foliageTilemap, woodtTile, pos);
                }
                else
                {
                    PaintSingleTile(foliageTilemap, folliagetile4, pos);
                }
                objPositions.Add(pos);
            }

            yield return null;
        }
    }

    public BoundsInt GetTilemapBounds()
    {
        return floorTilemap.cellBounds;
    }

    private void PaintTiles(IEnumerable<Vector2Int> positions, Tilemap tilemap, TileBase tile)
    {
        foreach (var position in positions)
        {
            PaintSingleTile(tilemap, tile, position);
        }
    }

    //async method for painting walls over a period of frames (10 wall per frame)
    public IEnumerator PaintTilesAsync(HashSet<Vector2Int> positions)
    {
        int numElements = 0;
        foreach (var position in new HashSet<Vector2Int>(positions))
        {
            PaintSingleTile(wallTilemap, wallTop, position);
            numElements++;
            if (numElements % 2 == 0)
            {
                yield return null;
            }
        }
    }

    private void PaintSingleTile(Tilemap tilemap, TileBase tile, Vector2Int position)
    {
        var tilePosition = tilemap.WorldToCell((Vector3Int)position);
        tilemap.SetTile(tilePosition, tile);
    }

    internal void PaintSingleBasicWall(Vector2Int position)
    {
        PaintSingleTile(wallTilemap, wallTop, position);
    }

    public void ClearWallTile(Vector2Int position)
    {
        var tilePosition = wallTilemap.WorldToCell((Vector3Int)position);
        wallTilemap.SetTile(tilePosition, null);
    }

    public void ClearWallTiles(HashSet<Vector2Int> positions)
    {
        foreach (var position in positions)
        {
            ClearWallTile(position);
        }
    }

    public void ClearFloorTile(Vector2Int position)
    {
        var tilePosition = floorTilemap.WorldToCell((Vector3Int)position);
        floorTilemap.SetTile(tilePosition, null);
    }
    internal void PaintSingleDebug(Vector2Int position)
    {
        PaintSingleTile(debugTilemap, debugTile, position);
    }

    public void Clear()
    {
        floorTilemap.ClearAllTiles();
        wallTilemap.ClearAllTiles();
    }
}