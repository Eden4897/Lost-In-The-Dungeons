using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : GameElement
{
    [SerializeField] private Direction direction;
    [SerializeField] public Sprite horizontalBeam;
    [SerializeField] public Sprite verticalBeam;

    [SerializeField] public Sprite upEnd;
    [SerializeField] public Sprite downEnd;
    [SerializeField] public Sprite leftEnd;
    [SerializeField] public Sprite rightEnd;

    [SerializeField] public Sprite upEndLeft;
    [SerializeField] public Sprite upLeftEnd;
    [SerializeField] public Sprite upEndRight;
    [SerializeField] public Sprite upRightEnd;
    [SerializeField] public Sprite downEndLeft;
    [SerializeField] public Sprite downLeftEnd;
    [SerializeField] public Sprite downEndRight;
    [SerializeField] public Sprite downRightEnd;

    [SerializeField] public Sprite upLeft;
    [SerializeField] public Sprite upRight;
    [SerializeField] public Sprite downRight;
    [SerializeField] public Sprite downLeft;
    [SerializeField] private GameObject laserTemplate;

    private List<GameObject> laserBeams = new List<GameObject>();
    private bool isLaserCasted = false;
    private Detector currentDetector = null;
    private List<Mirror> reflectingMirrors = new List<Mirror>();

    public int running = 0;


    protected override void Start()
    {
        base.Start();
        grid.lasers.Add(this);
        StartCoroutine(LaserCast());
    }

    public override void OnStartMove()
    {
        base.OnStartMove();
        ClearAll();
    }

    public override void OnMoved()
    {
        base.OnMoved();
        StartCoroutine(LaserCast());
    }

    public IEnumerator LaserCast()
    {
        ClearAll();

        Vector2Int increment = Vector2Int.zero;
        increment = DirectionToVector(direction);
        Vector2Int lastPos = position;
        Direction currentDir = direction;

        Direction lastDirection = Direction.Null;
        while (true)
        {
            Vector2Int target = lastPos + increment;
            if (grid.walls.Contains(target)) //if wall ahead
            {
                //Destroy last beam
                Destroy(laserBeams[laserBeams.Count - 1]);
                laserBeams.RemoveAt(laserBeams.Count - 1);

                //change beam to end
                GameObject newLaser = Instantiate(laserTemplate, (Vector3Int)lastPos + new Vector3(0.5f, 0.5f), Quaternion.identity, transform);
                laserBeams.Add(newLaser);
                SpriteRenderer spriteRenderer = newLaser.GetComponent<SpriteRenderer>();
                if (lastDirection != Direction.Null) //reflected beam
                {
                    spriteRenderer.sprite = GetReflectedEnd(Reverse(lastDirection), currentDir);
                }
                else //normal beam
                {
                    spriteRenderer.sprite = GetEnd(currentDir);
                }
                break;
            }
            else if(grid.GetBox(target) != null)
            {
                //Destroy last beam
                Destroy(laserBeams[laserBeams.Count - 1]);
                laserBeams.RemoveAt(laserBeams.Count - 1);

                //change beam to end
                GameObject newLaser = Instantiate(laserTemplate, (Vector3Int)lastPos + new Vector3(0.5f, 0.5f), Quaternion.identity, transform);
                laserBeams.Add(newLaser);
                SpriteRenderer spriteRenderer = newLaser.GetComponent<SpriteRenderer>();
                if (lastDirection != Direction.Null) //reflected beam
                {
                    spriteRenderer.sprite = GetReflectedEnd(Reverse(lastDirection), currentDir);
                }
                else //normal beam
                {
                    spriteRenderer.sprite = GetEnd(currentDir);
                }
                grid.GetBox(target).Cast(this);
                break;
            }
            else if(grid.GetMirror(target) != null) //if mirror ahead
            {
                //check if need to reflect or wrong side
                Mirror mirror = grid.GetMirror(target);
                Direction reflection = mirror.Reflect(currentDir);

                if (reflection != Direction.Null) //if need to reflect
                {
                    GameObject newLaser = Instantiate(laserTemplate, (Vector3Int)target + new Vector3(0.5f, 0.5f), Quaternion.identity, transform);
                    laserBeams.Add(newLaser);
                    SpriteRenderer spriteRenderer = newLaser.GetComponent<SpriteRenderer>();
                    spriteRenderer.sprite = GetReflectedBeam(Reverse(currentDir), reflection);

                    lastDirection = currentDir;
                    lastPos = target;
                    currentDir = reflection;
                    increment = DirectionToVector(currentDir);

                    mirror.Cast(this);
                    reflectingMirrors.Add(mirror);
                }
                else //if hitting wrong side
                {
                    if (laserBeams.Count == 0) break;

                    //destroy last beam
                    Destroy(laserBeams[laserBeams.Count - 1]);
                    laserBeams.RemoveAt(laserBeams.Count - 1);

                    //change beam to end
                    GameObject newLaser = Instantiate(laserTemplate, (Vector3Int)lastPos + new Vector3(0.5f, 0.5f), Quaternion.identity, transform);
                    laserBeams.Add(newLaser);
                    SpriteRenderer spriteRenderer = newLaser.GetComponent<SpriteRenderer>();
                    if (lastDirection != Direction.Null) //reflected beam
                    {
                        spriteRenderer.sprite = GetReflectedEnd(Reverse(lastDirection), currentDir);
                    }
                    else //normal beam
                    {
                        spriteRenderer.sprite = GetEnd(currentDir);
                    }
                    break;
                }
            }
            else if (grid.GetDetector(target) != null)
            {
                Detector detector = grid.GetDetector(target);
                if (detector.Detect(currentDir))
                {
                    detector.Cast(this);
                    currentDetector = detector;
                    break;
                }
                else //if hitting wrong side
                {
                    if (laserBeams.Count == 0) break;

                    //destroy last beam
                    Destroy(laserBeams[laserBeams.Count - 1]);
                    laserBeams.RemoveAt(laserBeams.Count - 1);

                    //change beam to end
                    GameObject newLaser = Instantiate(laserTemplate, (Vector3Int)lastPos + new Vector3(0.5f, 0.5f), Quaternion.identity, transform);
                    laserBeams.Add(newLaser);
                    SpriteRenderer spriteRenderer = newLaser.GetComponent<SpriteRenderer>();
                    if (lastDirection != Direction.Null) //reflected beam
                    {
                        spriteRenderer.sprite = GetReflectedEnd(Reverse(lastDirection), currentDir);
                    }
                    else //normal beam
                    {
                        spriteRenderer.sprite = GetEnd(currentDir);
                    }
                    break;
                }
            }
            else //normal casting
            {
                GameObject newLaser = Instantiate(laserTemplate, (Vector3Int)target + new Vector3(0.5f, 0.5f), Quaternion.identity, transform);
                laserBeams.Add(newLaser);
                SpriteRenderer spriteRenderer = newLaser.GetComponent<SpriteRenderer>();

                if((int)currentDir <= 1)spriteRenderer.sprite = verticalBeam;
                else spriteRenderer.sprite = horizontalBeam;

                lastPos = target;
                lastDirection = Direction.Null;
            }
            //yield return null;
        }
        yield return null;
    }

    #region Utility
    public void ClearAll()
    {
        foreach (GameObject laserBeam in laserBeams)
        {
            Destroy(laserBeam);
        }
        if(currentDetector != null)
        {
            currentDetector.DeCast();
            currentDetector = null;
        }
        reflectingMirrors.Clear();
        laserBeams.Clear();
    }

    private Vector2Int DirectionToVector(Direction direction)
    {
        switch (direction)
        {
            case Direction.Up:
                return Vector2Int.up;
            case Direction.Down:
                return Vector2Int.down;
            case Direction.Left:
                return Vector2Int.left;
            case Direction.Right:
                return Vector2Int.right;
        }
        return default;
    }

    private Sprite GetEnd(Direction direction)
    {
        switch (direction)
        {
            case Direction.Up:
                return upEnd;
            case Direction.Down:
                return downEnd;
            case Direction.Left:
                return leftEnd;
            case Direction.Right:
                return rightEnd;
        }
        return null;
    }

    private Sprite GetReflectedBeam(Direction incident, Direction reflected)
    {
        if(incident == Direction.Up && reflected == Direction.Left)
        {
            return upLeft;
        }
        else if (incident == Direction.Left && reflected == Direction.Up)
        {
            return upLeft;
        }
        else if (incident == Direction.Up && reflected == Direction.Right)
        {
            return upRight;
        }
        else if (incident == Direction.Right && reflected == Direction.Up)
        {
            return upRight;
        }
        else if (incident == Direction.Down && reflected == Direction.Left)
        {
            return downLeft;
        }
        else if (incident == Direction.Left && reflected == Direction.Down)
        {
            return downLeft;
        }
        else if (incident == Direction.Down && reflected == Direction.Right)
        {
            return downRight;
        }
        else if (incident == Direction.Right && reflected == Direction.Down)
        {
            return downRight;
        }
        return null;
    }

    private Sprite GetReflectedEnd(Direction incident, Direction reflected)
    {
        if (incident == Direction.Up && reflected == Direction.Left)
        {
            return upLeftEnd;
        }
        else if (incident == Direction.Left && reflected == Direction.Up)
        {
            return upEndLeft;
        }
        else if (incident == Direction.Up && reflected == Direction.Right)
        {
            return upRightEnd;
        }
        else if (incident == Direction.Right && reflected == Direction.Up)
        {
            return upEndRight;
        }
        else if (incident == Direction.Down && reflected == Direction.Left)
        {
            return downLeftEnd;
        }
        else if (incident == Direction.Left && reflected == Direction.Down)
        {
            return downEndLeft;
        }
        else if (incident == Direction.Down && reflected == Direction.Right)
        {
            return downRightEnd;
        }
        else if (incident == Direction.Right && reflected == Direction.Down)
        {
            return downEndRight;
        }
        return null;
    }
    #endregion
}
