using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : GameElement
{
    [SerializeField] private Direction direction;
    [SerializeField] private GameObject leftRightBeam;
    [SerializeField] private GameObject rightLeftBeam;
    [SerializeField] private GameObject upDownBeam;
    [SerializeField] private GameObject downUpBeam;
    [SerializeField] private GameObject upEnd;
    [SerializeField] private GameObject downEnd;
    [SerializeField] private GameObject leftEnd;
    [SerializeField] private GameObject rightEnd;


    private List<GameObject> laserBeams = new List<GameObject>();
    private List<Mirror> reflectingMirrors = new List<Mirror>(); 
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
        switch (direction)
        {
            case Direction.Up:
                increment = Vector2Int.up;
                break;
            case Direction.Down:
                increment = Vector2Int.down;
                break;
            case Direction.Left:
                increment = Vector2Int.left;
                break;
            case Direction.Right:
                increment = Vector2Int.right;
                break;
        }
        Vector2Int lastPos = position;
        Direction currentDir = direction;
        while (true)
        {
            Vector2Int target = lastPos + increment;
            if (grid.walls.Contains(target + increment)) //if wall ahead
            {
                switch (currentDir)
                {
                    case Direction.Up:
                        laserBeams.Add(Instantiate(upEnd, (Vector3Int)target + new Vector3(0.5f, 0.5f), Quaternion.identity, transform));
                        break;
                    case Direction.Down:
                        laserBeams.Add(Instantiate(downEnd, (Vector3Int)target + new Vector3(0.5f, 0.5f), Quaternion.identity, transform));
                        break;
                    case Direction.Left:
                        laserBeams.Add(Instantiate(leftEnd, (Vector3Int)target + new Vector3(0.5f, 0.5f), Quaternion.identity, transform));
                        break;
                    case Direction.Right:
                        laserBeams.Add(Instantiate(rightEnd, (Vector3Int)target + new Vector3(0.5f, 0.5f), Quaternion.identity, transform));
                        break;
                }
                break;
            }
            else if(grid.GetMirror(target + increment)) //if mirror ahead
            {
                //move laser one tile foward
                switch (currentDir)
                {
                    case Direction.Up:
                        laserBeams.Add(Instantiate(downUpBeam, (Vector3Int)target + new Vector3(0.5f, 0.5f), Quaternion.identity, transform));
                        break;
                    case Direction.Down:
                        laserBeams.Add(Instantiate(upDownBeam, (Vector3Int)target + new Vector3(0.5f, 0.5f), Quaternion.identity, transform));
                        break;
                    case Direction.Left:
                        laserBeams.Add(Instantiate(rightLeftBeam, (Vector3Int)target + new Vector3(0.5f, 0.5f), Quaternion.identity, transform));
                        break;
                    case Direction.Right:
                        laserBeams.Add(Instantiate(leftRightBeam, (Vector3Int)target + new Vector3(0.5f, 0.5f), Quaternion.identity, transform));
                        break;
                }
                lastPos = target;

                //check if need to reflect or wrong side
                Mirror mirror = grid.GetMirror(target + increment);
                Direction reflection = mirror.Reflect(currentDir);

                if (reflection != Direction.Null) //if need to reflect
                {
                    mirror.Cast(currentDir);
                    reflectingMirrors.Add(mirror);

                    currentDir = reflection;
                    lastPos += increment;
                    switch (currentDir)
                    {
                        case Direction.Up:
                            increment = Vector2Int.up;
                            break;
                        case Direction.Down:
                            increment = Vector2Int.down;
                            break;
                        case Direction.Left:
                            increment = Vector2Int.left;
                            break;
                        case Direction.Right:
                            increment = Vector2Int.right;
                            break;
                    }
                }
                else //if hitting wrong side
                {
                    switch (currentDir)
                    {
                        case Direction.Up:
                            laserBeams.Add(Instantiate(upEnd, (Vector3Int)target + new Vector3(0.5f, 0.5f), Quaternion.identity, transform));
                            break;
                        case Direction.Down:
                            laserBeams.Add(Instantiate(downEnd, (Vector3Int)target + new Vector3(0.5f, 0.5f), Quaternion.identity, transform));
                            break;
                        case Direction.Left:
                            laserBeams.Add(Instantiate(leftEnd, (Vector3Int)target + new Vector3(0.5f, 0.5f), Quaternion.identity, transform));
                            break;
                        case Direction.Right:
                            laserBeams.Add(Instantiate(rightEnd, (Vector3Int)target + new Vector3(0.5f, 0.5f), Quaternion.identity, transform));
                            break;
                    }
                    break;
                }
            }
            else //normal casting
            {
                switch (currentDir)
                {
                    case Direction.Up:
                        laserBeams.Add(Instantiate(downUpBeam, (Vector3Int)target + new Vector3(0.5f, 0.5f), Quaternion.identity, transform));
                        break;
                    case Direction.Down:
                        laserBeams.Add(Instantiate(upDownBeam, (Vector3Int)target + new Vector3(0.5f, 0.5f), Quaternion.identity, transform));
                        break;
                    case Direction.Left:
                        laserBeams.Add(Instantiate(rightLeftBeam, (Vector3Int)target + new Vector3(0.5f, 0.5f), Quaternion.identity, transform));
                        break;
                    case Direction.Right:
                        laserBeams.Add(Instantiate(leftRightBeam, (Vector3Int)target + new Vector3(0.5f, 0.5f), Quaternion.identity, transform));
                        break;
                }
                lastPos = target;
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
        foreach (Mirror mirror in reflectingMirrors)
        {
            mirror.DeCast();
        }
        laserBeams.Clear();
        reflectingMirrors.Clear();
    }
}
