using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour
{
    [SerializeField] private Belt linkedBelt;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite onSprite;
    [SerializeField] private Sprite offSprite;

    private GridManager grid;

    private bool isOn = false;
    private void Start()
    {
        grid = GridManager.instance;
        grid.levers.Add(transform.position - new Vector3(0.5f,0.5f), this);
    }

    public void Flick()
    {
        if (isOn)
        {
            spriteRenderer.sprite = offSprite;
        }
        else
        {
            spriteRenderer.sprite = onSprite;
        }
        isOn = !isOn;
        linkedBelt.Move();
    }
}
