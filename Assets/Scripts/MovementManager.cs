using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MovementManager : MonoBehaviour
{
    [SerializeField] private Tilemap tm;
    [SerializeField] private Grid grid;
    [SerializeField] private Player boy;
    [SerializeField] private Player girl;
    private Player player;

    private void Start()
    {
        player = boy;
    }
    private void Update()
    {
        #region Switching players
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if(player == boy)
            {
                player = girl;
            }
            else if (player == girl)
            {
                player = boy;
            }
        }
        #endregion

        #region Movement
        if (player._isMoving) return;
        if (Input.GetKey(KeyCode.W))
        {
            if (grid.walls.Contains(new Vector2Int(player.position.x + 0, player.position.y + 1))) return;
            StartCoroutine(player.Move(player.transform.position + new Vector3(0, 1, 0)));
        }
        else if (Input.GetKey(KeyCode.S))
        {
            if (grid.walls.Contains(new Vector2Int(player.position.x + 0, player.position.y - 1))) return;
            StartCoroutine(player.Move(player.transform.position + new Vector3(0, -1, 0)));
        }
        else if (Input.GetKey(KeyCode.A))
        {
            if (grid.walls.Contains(new Vector2Int(player.position.x - 1, player.position.y + 0))) return;
            StartCoroutine(player.Move(player.transform.position + new Vector3(-1, 0, 0)));
        }
        else if (Input.GetKey(KeyCode.D))
        {
            if (grid.walls.Contains(new Vector2Int(player.position.x + 1, player.position.y + 0))) return;
            StartCoroutine(player.Move(player.transform.position + new Vector3(1, 0, 0)));
        }
        #endregion
    }
}
