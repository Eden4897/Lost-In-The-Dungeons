using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct LaserSprites
{
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
}
public class Laser : GameElement
{
    [SerializeField] private Direction direction;
    [SerializeField] private LaserSprites laserSprites;
    [SerializeField] private GameObject laserTemplate;

    private List<GameObject> laserBeams = new List<GameObject>();
    private bool isLaserCasted = false;

    
    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
        if (!isStationary && isLaserCasted)
        {
            ClearAll();
            isLaserCasted = false;
        }
        else if (isStationary && !isLaserCasted)
        {
            StartCoroutine(LaserCast());
            isLaserCasted = true;
        }
    }

    private IEnumerator LaserCast()
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
                    Debug.Log("Reflected then end");
                    spriteRenderer.sprite = GetReflectedEnd(Reverse(lastDirection), currentDir);
                }
                else //normal beam
                {
                    spriteRenderer.sprite = GetEnd(currentDir);
                }
                break;
            }
            else if(grid.GetMirror(target)) //if mirror ahead
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

                    Debug.Log(newLaser.GetComponent<SpriteRenderer>().sprite.name);

                    lastDirection = currentDir;
                    lastPos = target;
                    currentDir = reflection;
                    increment = DirectionToVector(currentDir);
                }
                else //if hitting wrong side
                {
                    //Destroy last beam
                    if(laserBeams.Count == 0)
                    {
                        break;
                    }
                    Destroy(laserBeams[laserBeams.Count - 1]);
                    laserBeams.RemoveAt(laserBeams.Count - 1);

                    //change beam to end
                    GameObject newLaser = Instantiate(laserTemplate, (Vector3Int)lastPos + new Vector3(0.5f, 0.5f), Quaternion.identity, transform);
                    laserBeams.Add(newLaser);
                    SpriteRenderer spriteRenderer = newLaser.GetComponent<SpriteRenderer>();
                    if (lastDirection != Direction.Null) //reflected beam
                    {
                        spriteRenderer.sprite = GetReflectedEnd(Reverse(lastDirection), currentDir);
                        Debug.Log($"{Reverse(lastDirection)}, {currentDir}");
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

                if((int)currentDir <= 1)spriteRenderer.sprite = laserSprites.verticalBeam;
                else spriteRenderer.sprite = laserSprites.horizontalBeam;

                lastPos = target;
                lastDirection = Direction.Null;
            }
        }
        yield return null;
    }

    private void ClearAll()
    {
        foreach (GameObject laserBeam in laserBeams)
        {
            Destroy(laserBeam);
        }
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
                return laserSprites.upEnd;
            case Direction.Down:
                return laserSprites.downEnd;
            case Direction.Left:
                return laserSprites.leftEnd;
            case Direction.Right:
                return laserSprites.rightEnd;
        }
        return null;
    }

    private Sprite GetReflectedBeam(Direction incident, Direction reflected)
    {
        if(incident == Direction.Up && reflected == Direction.Left)
        {
            return laserSprites.upLeft;
        }
        else if (incident == Direction.Left && reflected == Direction.Up)
        {
            return laserSprites.upLeft;
        }
        else if (incident == Direction.Up && reflected == Direction.Right)
        {
            return laserSprites.upRight;
        }
        else if (incident == Direction.Right && reflected == Direction.Up)
        {
            return laserSprites.upRight;
        }
        else if (incident == Direction.Down && reflected == Direction.Left)
        {
            return laserSprites.downLeft;
        }
        else if (incident == Direction.Left && reflected == Direction.Down)
        {
            return laserSprites.downLeft;
        }
        else if (incident == Direction.Down && reflected == Direction.Right)
        {
            return laserSprites.downRight;
        }
        else if (incident == Direction.Right && reflected == Direction.Down)
        {
            return laserSprites.downRight;
        }
        return null;
    }

    private Sprite GetReflectedEnd(Direction incident, Direction reflected)
    {
        if (incident == Direction.Up && reflected == Direction.Left)
        {
            return laserSprites.upLeftEnd;
        }
        else if (incident == Direction.Left && reflected == Direction.Up)
        {
            return laserSprites.upEndLeft;
        }
        else if (incident == Direction.Up && reflected == Direction.Right)
        {
            return laserSprites.upRightEnd;
        }
        else if (incident == Direction.Right && reflected == Direction.Up)
        {
            return laserSprites.upEndRight;
        }
        else if (incident == Direction.Down && reflected == Direction.Left)
        {
            return laserSprites.downLeftEnd;
        }
        else if (incident == Direction.Left && reflected == Direction.Down)
        {
            return laserSprites.downEndLeft;
        }
        else if (incident == Direction.Down && reflected == Direction.Right)
        {
            return laserSprites.downRightEnd;
        }
        else if (incident == Direction.Right && reflected == Direction.Down)
        {
            return laserSprites.downEndRight;
        }
        return null;
    }
}
