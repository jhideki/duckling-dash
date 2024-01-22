using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileMapVisualizer : MonoBehaviour
{
    [SerializeField] private Tilemap floorTilemap, wallTilemap;
    [SerializeField] private TileBase floorTile, wallTop, debugTile;

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
        floorTilemap.SetTile(tilePosition, null);
    }

    public void ClearFloorTile(Vector2Int position)
    {
        var tilePosition = floorTilemap.WorldToCell((Vector3Int)position);
        floorTilemap.SetTile(tilePosition, null);
    }
    internal void PaintSingleDebug(Vector2Int position)
    {
        PaintSingleTile(wallTilemap, debugTile, position);
    }

    public void Clear()
    {
        floorTilemap.ClearAllTiles();
        wallTilemap.ClearAllTiles();
    }
}