using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Platform : MonoBehaviour
{
    [SerializeField] public Vector2Int position;
    [HideInInspector] public Player player = null;
    [HideInInspector] public bool isStationary = true;
    private GridManager grid;
    private void Start()
    {
        grid = GridManager.instance;
        grid.platforms.Add(this);

        position = new Vector2Int(Mathf.FloorToInt(transform.position.x), Mathf.FloorToInt(transform.position.y));
    }
    private void SetPos()
    {

    }
}
