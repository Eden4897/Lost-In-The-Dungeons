﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridManager : MonoBehaviour
{
    public static GridManager instance;
    [SerializeField] public Tilemap wallTilemap;
    [SerializeField] public Tilemap tileTilemap;

    [SerializeField] public List<Vector2Int> walls = new List<Vector2Int>();
    [SerializeField] public List<Vector2Int> tiles = new List<Vector2Int>();

    [HideInInspector] public List<Lever> levers = new List<Lever>();
    [HideInInspector] public List<PressurePlate> pressurePlates = new List<PressurePlate>();
    [SerializeField] public List<Platform> platforms = new List<Platform>();
    [SerializeField] public List<FallingPlatform> fallingPlatforms = new List<FallingPlatform>();

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
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
                    tiles.Add(new Vector2Int(x + _tileBounds.xMin, y + _tileBounds.yMin));
                }
            }
        }
    }

    public bool IsWalkable(Vector2Int pos)
    {
        if (tiles.Contains(pos)) return true;
        if (GetPlatform(pos) != null) return true;
        if (GetFallingPlatform(pos) != null) return true;
        return false;
    }

    public Platform GetPlatform(Vector2Int pos)
    {
        foreach(Platform platform in platforms)
        {
            if(platform.position == pos && platform.isStationary)
            {
                return platform;
            }
        }
        return null;
    }

    public FallingPlatform GetFallingPlatform(Vector2Int pos)
    {
        foreach (FallingPlatform fallingPlatform in fallingPlatforms)
        {
            if (fallingPlatform.position == pos)
            {
                return fallingPlatform;
            }
        }
        return null;
    }

    public Lever GetLever(Vector2 pos)
    {
        foreach (Lever lever in levers)
        {
            if (lever.position == pos && lever.isStationary)
            {
                return lever;
            }
        }
        return null;
    }

    public PressurePlate GetPlate (Vector2 pos)
    {
        foreach (PressurePlate pressurePlate in pressurePlates)
        {
            if (pressurePlate.position == pos && pressurePlate.isStationary)
            {
                return pressurePlate;
            }
        }
        return null;
    }
}
