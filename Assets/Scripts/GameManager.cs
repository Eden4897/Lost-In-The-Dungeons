using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    //input
    [SerializeField] public Tilemap wallTilemap;
    [SerializeField] public Tilemap tileTilemap;
    [SerializeField] private Gate gates;
    [SerializeField] private Player[] players;
    [SerializeField] private GameObject winScreen;
    [SerializeField] private GameObject reloadScreen;

    //visualised info
    [SerializeField] public List<Vector2Int> walls = new List<Vector2Int>();
    [SerializeField] public List<Vector2Int> tiles = new List<Vector2Int>();


    [HideInInspector] public List<Lever> levers = new List<Lever>();
    [HideInInspector] public List<PressurePlate> pressurePlates = new List<PressurePlate>();
    [HideInInspector] public List<Platform> platforms = new List<Platform>();
    [HideInInspector] public List<FallingPlatform> fallingPlatforms = new List<FallingPlatform>();
    [HideInInspector] public List<Mirror> mirrors = new List<Mirror>();

    [HideInInspector] public List<Vector2Int> targetTiles = new List<Vector2Int>();

    public int activeCollectables = 0;

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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            reloadScreen.SetActive(true);
            Invoke("ReloadScene", 0.5f);
        }
        if (activeCollectables <= 0)
        {
            gates.Open();
        }
        bool isDone = true;
        foreach (Player player in players)
        {
            if (!targetTiles.Contains(player.position)) isDone = false;
        }
        if (isDone)
        {
            winScreen.SetActive(true);
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

    public Mirror GetMirror(Vector2 pos)
    {
        foreach (Mirror mirror in mirrors)
        {
            if (mirror.position == pos && mirror.isStationary)
            {
                return mirror;
            }
        }
        return null;
    }

    public void NextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void LoadScene(string name)
    {
        SceneManager.LoadScene(name);
    }

    private void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
