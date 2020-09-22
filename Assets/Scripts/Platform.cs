using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Platform : MonoBehaviour
{
    [SerializeField] public Vector2Int position;
    [HideInInspector] public Player player = null;
    [HideInInspector] public bool isStationary = true;
    [SerializeField] public GameElement gameElement = null;
    private GameManager grid;

    private void Start()
    {
        grid = GameManager.instance;
        grid.platforms.Add(this);

        position = new Vector2Int(Mathf.FloorToInt(transform.position.x), Mathf.FloorToInt(transform.position.y));
    }

    public void Moved()
    {
        if(player != null)
        {
            player.position = position;
        }
        if (gameElement != null)
        {
            gameElement.position = position;
        }
    }
}
