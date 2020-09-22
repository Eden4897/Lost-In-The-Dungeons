using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameElement : MonoBehaviour
{
    [HideInInspector] public bool isStationary = true;
    public Vector2Int position;
    protected GameManager grid;
    protected virtual void Start() 
    {
        grid = GameManager.instance;
        position = new Vector2Int(Mathf.RoundToInt(transform.position.x - 0.5f), Mathf.RoundToInt(transform.position.y - 0.5f));
    }

    protected virtual void Update()
    {
        
    }

    protected virtual void OnCollisionEnter2D(Collision2D collider) { }
}
