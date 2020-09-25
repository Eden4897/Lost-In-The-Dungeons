using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : GameElement
{
    [SerializeField] private Belt[] linkedBelts;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite steppedSprite;
    [SerializeField] private Sprite releasedSprite;

    private AudioClip down;
    private AudioClip up;

    private int _occupants;

    protected override void Start()
    {
        base.Start();
        grid.pressurePlates.Add(this);
        down = Resources.Load<AudioClip>("Audio/PressurePlateDown");
        up = Resources.Load<AudioClip>("Audio/PressurePlateUp");
    }

    public void Step()
    {
        AudioManager.instance.PlaySound(down);
        _occupants++;
        if (_occupants == 1)
        {
            spriteRenderer.sprite = steppedSprite;
            foreach (Belt linkedBelt in linkedBelts)
            {
                linkedBelt.Move();
            }
        }     
    }
    public void Release()
    {
        AudioManager.instance.PlaySound(up);
        _occupants--;
        if (_occupants == 0)
        {
            spriteRenderer.sprite = releasedSprite;
            foreach (Belt linkedBelt in linkedBelts)
            {
                linkedBelt.Move();
            }
        }
    }
}
