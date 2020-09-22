using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : GameElement
{
    [SerializeField] private Belt linkedBelt;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite steppedSprite;
    [SerializeField] private Sprite releasedSprite;

    private int _occupants;

    protected override void Start()
    {
        base.Start();
        grid.pressurePlates.Add(this);
    }

    public void Step()
    {
        _occupants++;
        if (_occupants == 1)
        {
            spriteRenderer.sprite = steppedSprite;
            linkedBelt.Move();
        }     
    }
    public void Release()
    {
        _occupants--;
        if (_occupants == 0)
        {
            spriteRenderer.sprite = releasedSprite;
            linkedBelt.Move();
        }
    }
}
