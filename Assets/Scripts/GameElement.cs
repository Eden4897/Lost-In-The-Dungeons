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

    protected GameManager grid;
    [HideInInspector] public bool isStationary = true;
    public Vector2Int position;
    [HideInInspector] public bool isPushable = false;
    [HideInInspector] public Platform currentPlatform;
    [HideInInspector] public PressurePlate currentPlate;
    private AudioClip pushSound;

    protected virtual void Start() 
    {
        grid = GameManager.instance;
        position = new Vector2Int(Mathf.RoundToInt(transform.position.x - 0.5f), Mathf.RoundToInt(transform.position.y - 0.5f));
        pushSound = Resources.Load<AudioClip>("Audio/Dragging");
        Invoke("SetUp", 0.01f);
    }

    private void SetUp()
    {
        Platform platform = grid.GetPlatform(position);
        if (platform != null)
        {
            transform.parent = platform.transform;
            platform.gameElements.Add(this);
            currentPlatform = platform;
        }
    }

    protected virtual void Update() { }

    protected virtual void OnCollisionEnter2D(Collision2D collider) { }

    public virtual void OnStartMove() { }
    public virtual void OnMoved() { }
    public virtual void OnStartPush()
    {
        AudioManager.instance.PlaySound(pushSound);
    }
    public virtual void OnPushed() { }

    protected Direction Reverse(Direction dir)
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
