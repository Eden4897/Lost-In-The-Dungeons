using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameElement : MonoBehaviour
{
    public enum Direction
    {
        Up,
        Down,
        Left,
        Right,
        Null
    }

    [HideInInspector] public bool isStationary = true;
    public Vector2Int position;
    protected GameManager grid;
    protected virtual void Start() 
    {
        grid = GameManager.instance;
        position = new Vector2Int(Mathf.RoundToInt(transform.position.x - 0.5f), Mathf.RoundToInt(transform.position.y - 0.5f));
    }

    protected virtual void Update() { }

    protected virtual void OnCollisionEnter2D(Collision2D collider) { }

    protected virtual Direction Reverse(Direction dir)
    {
        switch (dir)
        {
            case Direction.Up:
                return Direction.Down;
            case Direction.Down:
                return Direction.Up;
            case Direction.Left:
                return Direction.Right;
            case Direction.Right:
                return Direction.Left;
        }
        return Direction.Null;
    }
}
