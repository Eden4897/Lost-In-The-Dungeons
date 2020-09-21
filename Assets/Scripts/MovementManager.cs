using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MovementManager : MonoBehaviour
{
    [SerializeField] private Tilemap tm;
    [SerializeField] private Player boy;
    [SerializeField] private Player girl;

    private GridManager grid;
    private Player player;

    private void Start()
    {
        grid = GridManager.instance;
        player = boy;
    }
    private void Update()
    {
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
            Vector2Int target = new Vector2Int(player.position.x + 0, player.position.y + 1);
            if (grid.walls.Contains(target)) return;
            if (!grid.IsWalkable(target)) return;
            StartCoroutine(player.Move(player.transform.position + new Vector3(0, 1, 0)));
            player.animator.Play(player.WalkUp);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            Vector2Int target = new Vector2Int(player.position.x + 0, player.position.y - 1);
            if (grid.walls.Contains(target)) return;
            if (!grid.IsWalkable(target)) return;
            Debug.Log("S");
            StartCoroutine(player.Move(player.transform.position + new Vector3(0, -1, 0)));
            player.animator.Play(player.WalkDown);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            Vector2Int target = new Vector2Int(player.position.x + -1, player.position.y + 0);
            if (grid.walls.Contains(target)) return;
            if (!grid.IsWalkable(target)) return;
            StartCoroutine(player.Move(player.transform.position + new Vector3(-1, 0, 0)));
            player.animator.Play(player.WalkLeft);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            Vector2Int target = new Vector2Int(player.position.x + 1, player.position.y + 0);
            if (grid.walls.Contains(target)) return;
            if (!grid.IsWalkable(target)) return;
            StartCoroutine(player.Move(player.transform.position + new Vector3(1, 0, 0)));
            player.animator.Play(player.WalkRight);
        }
        #endregion
    }
}
