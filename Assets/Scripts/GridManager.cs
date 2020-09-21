using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridManager : MonoBehaviour
{
    [SerializeField] public Tilemap wallTilemap;
    [SerializeField] public Tilemap tileTilemap;

    [SerializeField] public List<Vector2Int> walls = new List<Vector2Int>();
    [SerializeField] public List<Vector2Int> tiles = new List<Vector2Int>();

    [HideInInspector] public Dictionary<Vector2, Lever> levers = new Dictionary<Vector2, Lever>();

    private void Awake()
    {
        wallTilemap.CompressBounds();
        tileTilemap.CompressBounds();
        BoundsInt _wallBounds = wallTilemap.cellBounds;
        BoundsInt _tileBounds = tileTilemap.cellBounds;
        TileBase[] _wallTiles = wallTilemap.GetTilesBlock(_wallBounds);
        TileBase[] _tileTiles = tileTilemap.GetTilesBlock(_tileBounds);
        for (int x = 0; x < _wallBounds.size.x; ++x)
        {
            for (int y = 0; y < _wallBounds.size.y; ++y)
            {
                TileBase tile = _wallTiles[x + y * _wallBounds.size.x];
                if (tile != null)
                {
                    walls.Add(new Vector2Int(x + _wallBounds.xMin, y + _wallBounds.yMin));
                }
            }
        }
        for (int x = 0; x < _tileBounds.size.x; ++x)
        {
            for (int y = 0; y < _tileBounds.size.y; ++y)
            {
                TileBase tile = _tileTiles[x + y * _tileBounds.size.x];
                if (tile != null)
                {
                    tiles.Add(new Vector2Int(x + _wallBounds.xMin, y + _wallBounds.yMin));
                }
            }
        }
    }
}
