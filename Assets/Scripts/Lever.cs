using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : GameElement
{
    [SerializeField] private Belt linkedBelt;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite onSprite;
    [SerializeField] private Sprite offSprite;

    private GridManager grid;

    private bool isOn = false;

    protected override void Start()
    {
        base.Start();
        grid = GridManager.instance;
        grid.levers.Add(this);
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
