using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    [SerializeField] public Vector2Int position;
    [SerializeField] private Sprite broken;
    [SerializeField] private int state = 2;

    private SpriteRenderer spriteRenderer;
    private GameManager grid;
    private AudioClip crackingEffect;

    private void Start()
    {
        position.x = Mathf.RoundToInt(transform.position.x - 0.5f);
        position.y = Mathf.RoundToInt(transform.position.y - 0.5f);
        grid = GameManager.instance;
        grid.fallingPlatforms.Add(this);
        spriteRenderer = GetComponent<SpriteRenderer>();
        crackingEffect = Resources.Load<AudioClip>("Audio/Cracking");
    }
    public void Damage()
    {
        AudioManager.instance.PlaySound(crackingEffect);
        state--;
        if(state == 1)
        {
            spriteRenderer.sprite = broken;
        }
        else if (state == 0)
        {
            grid.fallingPlatforms.Remove(this);
            Destroy(gameObject);
        }
    }
}
