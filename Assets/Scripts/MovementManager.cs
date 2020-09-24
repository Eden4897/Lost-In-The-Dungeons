using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MovementManager : MonoBehaviour
{
    [SerializeField] private Player boy;
    [SerializeField] private Player girl;

    private GameManager grid;
    private Player player;

    private void Start()
    {
        grid = GameManager.instance;
        player = boy;
    }
    private void Update()
    {
        if (grid.isGamePaused) return;
        #region Switching players
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if(player == boy)
            {
                boy.spriteRenderer.color = Color.grey;
                girl.spriteRenderer.color = Color.white;
                player = girl;
            }
            else if (player == girl)
            {
                girl.spriteRenderer.color = Color.grey;
                boy.spriteRenderer.color = Color.white;
                player = boy;
            }
        }
        #endregion

        #region Movement
        if (player._isMoving) return;
        if (player.transform.parent != null) if (!player.GetComponentInParent<Platform>().isStationary) return;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Vector2Int[] neighbours = {player.position, new Vector2Int(player.position.x + 0, player.position.y + 1), new Vector2Int(player.position.x + 0, player.position.y - 1), new Vector2Int(player.position.x + 1, player.position.y + 0), new Vector2Int(player.position.x - 1, player.position.y + 0)};
            foreach(Vector2 neighbour in neighbours)
            {
                if (grid.GetLever(neighbour) != null)
                {
                    grid.GetLever(neighbour).Flick();
                    break;
                }
            }
        }
        if (Input.GetKey(KeyCode.W))
        {
            Move(GameElement.Direction.Up);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            Move(GameElement.Direction.Down);

        }
        else if (Input.GetKey(KeyCode.A))
        {
            Move(GameElement.Direction.Left);

        }
        else if (Input.GetKey(KeyCode.D))
        {
            Move(GameElement.Direction.Right);

        }
        #endregion
    }

    private void Move(GameElement.Direction dir)
    {
        Vector2Int force = Vector2Int.zero;
        string animationName = "";
        switch (dir)
        {
            case GameElement.Direction.Up:
                force = Vector2Int.up;
                animationName = player.WalkUp;
                break;
            case GameElement.Direction.Down:
                force = Vector2Int.down;
                animationName = player.WalkDown;
                break;
            case GameElement.Direction.Left:
                force = Vector2Int.left;
                animationName = player.WalkLeft;
                break;
            case GameElement.Direction.Right:
                force = Vector2Int.right;
                animationName = player.WalkRight;
                break;
        }
        Vector2Int target = new Vector2Int(player.position.x + force.x, player.position.y + force.y);
        if (!grid.IsWalkable(target))
        {
            if (grid.IsPushable(target, dir))
            {
                GameElement[] gameElements = grid.GetPushTargets(target, dir);
                foreach (GameElement gameElement in gameElements)
                {
                    gameElement.transform.parent = player.transform;
                    gameElement.OnStartMove();
                    gameElement.OnStartPush();
                }
                StartCoroutine(player.Move(player.transform.position + (Vector3Int)force, () =>
                {
                    foreach (GameElement gameElement in gameElements)
                    {
                        gameElement.transform.parent = null;
                        gameElement.position = gameElement.position + force;
                        gameElement.OnMoved();
                        gameElement.OnPushed();
                        Platform platform = grid.GetPlatform(gameElement.position);
                        if (platform != null)
                        {
                            gameElement.transform.parent = platform.transform;
                            platform.gameElements.Add(gameElement);
                            gameElement.currentPlatform = platform;
                        }
                    }
                }));
                player.animator.Play(animationName);
            }
        }
        else
        {
            StartCoroutine(player.Move(player.transform.position + (Vector3Int)force));
            player.animator.Play(animationName);
        }
    }
}
