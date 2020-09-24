using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Platform : MonoBehaviour
{
    [SerializeField] public Vector2Int position;
    [HideInInspector] public Player player = null;
    [HideInInspector] public bool isStationary = true;
    [SerializeField] public List<GameElement> gameElements = new List<GameElement>();
    private GameManager grid;
    private AudioClip sliding;
    private AudioClip slidingEnd;

    private void Start()
    {
        grid = GameManager.instance;
        grid.platforms.Add(this);

        position = new Vector2Int(Mathf.FloorToInt(transform.position.x), Mathf.FloorToInt(transform.position.y));
        sliding = Resources.Load<AudioClip>("Audio/PlatformSliding");
        slidingEnd = Resources.Load<AudioClip>("Audio/PlatformSlidingEnd");
    }
    public void StartMove()
    {
        AudioManager.instance.PlaySound(sliding);
        foreach(GameElement gameElement in gameElements)
        {
            gameElement.OnStartMove();
        }
    }
    public void Moved()
    {
        AudioManager.instance.PlaySound(slidingEnd);
        if (player != null)
        {
            player.position = position;
        }
        foreach (GameElement gameElement in gameElements)
        {
            gameElement.position = position;
            gameElement.OnStartMove();
        }
        foreach (Laser laser in grid.lasers)
        {
            StartCoroutine(laser.LaserCast());
        }
    }
}
