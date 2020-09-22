using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{
    [SerializeField] private Animator animator;
    private GameManager grid;
    private bool isOpened = false;
    public void Start()
    {
        grid = GameManager.instance;
        grid.walls.Add(new Vector2Int(Mathf.RoundToInt(transform.position.x - 2f), Mathf.RoundToInt(transform.position.y - 1.5f)));
        grid.walls.Add(new Vector2Int(Mathf.RoundToInt(transform.position.x - 1f), Mathf.RoundToInt(transform.position.y - 1.5f)));
        grid.walls.Add(new Vector2Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y - 1.5f)));
        grid.walls.Add(new Vector2Int(Mathf.RoundToInt(transform.position.x + 1f), Mathf.RoundToInt(transform.position.y - 1.5f)));
        grid.targetTiles.Add(new Vector2Int(Mathf.RoundToInt(transform.position.x - 1f), Mathf.RoundToInt(transform.position.y - 1.5f)));
        grid.targetTiles.Add(new Vector2Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y - 1.5f)));
    }
    public void Open()
    {
        if (isOpened) return;
        grid.walls.Remove(new Vector2Int(Mathf.RoundToInt(transform.position.x - 1f), Mathf.RoundToInt(transform.position.y - 1.5f)));
        grid.walls.Remove(new Vector2Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y - 1.5f)));
        animator.SetBool("Open", true);
        isOpened = true;
    }
}
