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
    [SerializeField] private GameObject pauseScreen;

    //visualised info
    [SerializeField] public List<Vector2Int> walls = new List<Vector2Int>();
    [SerializeField] public List<Vector2Int> tiles = new List<Vector2Int>();


    [HideInInspector] public List<Lever> levers = new List<Lever>();
    [HideInInspector] public List<PressurePlate> pressurePlates = new List<PressurePlate>();
    [HideInInspector] public List<Platform> platforms = new List<Platform>();
    [HideInInspector] public List<FallingPlatform> fallingPlatforms = new List<FallingPlatform>();
    [HideInInspector] public List<Mirror> mirrors = new List<Mirror>();
    [HideInInspector] public List<Detector> detectors = new List<Detector>();
    [HideInInspector] public List<Laser> lasers = new List<Laser>();
    [HideInInspector] public List<Box> boxes = new List<Box>();

    [HideInInspector] public List<Vector2Int> targetTiles = new List<Vector2Int>();

    [HideInInspector] public int activeCollectables = 0;
    [HideInInspector] public bool isGamePaused = false;


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
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isGamePaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
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
        if (GetBox(pos) != null) return false;
        if (GetLaser(pos) != null) return false;
        if (GetMirror(pos) != null) return false;
        if (GetDetector(pos) != null) return false;
        if (walls.Contains(pos)) return false;

        if (tiles.Contains(pos)) return true;
        if (GetPlatform(pos) != null) return true;
        if (GetFallingPlatform(pos) != null) return true;
        return false;
    }

    public bool IsPushable(Vector2Int pos, GameElement.Direction dir)
    {
        Vector2Int force = Vector2Int.zero;
        switch (dir)
        {
            case GameElement.Direction.Up:
                force = Vector2Int.up;
                break;
            case GameElement.Direction.Down:
                force = Vector2Int.down;
                break;
            case GameElement.Direction.Left:
                force = Vector2Int.left;
                break;
            case GameElement.Direction.Right:
                force = Vector2Int.right;
                break;
        }
        if(GetMirror(pos) != null || GetBox(pos) != null)
        {
            if (walls.Contains(pos + force)) return false;
            if (GetDetector(pos + force) != null) return false;
            if (GetLaser(pos + force) != null) return false;

            if (GetMirror(pos + force) != null || GetBox(pos + force) != null)
            {
                return IsPushable(pos + force, dir);
            }

            if (GetPlatform(pos + force) != null) return true;
            if (GetFallingPlatform(pos + force) != null) return true;
            if (tiles.Contains(pos + force)) return true;

            
            return false;
        }
        return false;
    }

    public GameElement[] GetPushTargets(Vector2Int pos, GameElement.Direction dir)
    {
        List<GameElement> output = new List<GameElement>();
        Vector2Int force = Vector2Int.zero;
        switch (dir)
        {
            case GameElement.Direction.Up:
                force = Vector2Int.up;
                break;
            case GameElement.Direction.Down:
                force = Vector2Int.down;
                break;
            case GameElement.Direction.Left:
                force = Vector2Int.left;
                break;
            case GameElement.Direction.Right:
                force = Vector2Int.right;
                break;
        }
        Vector2Int target = pos;
        while (true)
        {
            Mirror mirror = GetMirror(target);
            Box box = GetBox(target);
            if (mirror != null)
            {
                output.Add(mirror);
            }
            else if(box != null)
            {
                output.Add(box);
            }
            else break;
            target += force;
        }
        return output.ToArray();
    }

    #region Get Methods

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

    public Detector GetDetector(Vector2 pos)
    {
        foreach (Detector detector in detectors)
        {
            if (detector.position == pos && detector.isStationary)
            {
                return detector;
            }
        }
        return null;
    }

    public Laser GetLaser(Vector2Int pos)
    {
        foreach(Laser laser in lasers)
        {
            if(laser.position == pos && laser.isStationary)
            {
                return laser;
            }
        }
        return null;
    }

    public Box GetBox(Vector2Int pos)
    {
        foreach (Box box in boxes)
        {
            if(box.position == pos && box.isStationary)
            {
                return box;
            }
        }
        return null;
    }
    #endregion

    #region Scene Related Methods
    public void NextLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void LoadScene(string name)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(name);
    }

    private void ReloadScene()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Pause()
    {
        pauseScreen.SetActive(true);
        pauseScreen.GetComponent<Animator>().Play("PauseAppear");
        Time.timeScale = 0f;
        isGamePaused = true;
    }

    public void Resume()
    {
        pauseScreen.GetComponent<Animator>().Play("PauseDisappear");
        StartCoroutine(DisablePause());
    }
    private IEnumerator DisablePause()
    {
        yield return new WaitForSecondsRealtime(1f);
        pauseScreen.SetActive(false);
        Time.timeScale = 1f;
        isGamePaused = false;
    }
    #endregion
}
