using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : GameElement
{
    [SerializeField] private Belt linkedBelt;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite onSprite;
    [SerializeField] private Sprite offSprite;

    private bool isOn = false;
    private AudioClip LeverFlick;

    protected override void Start()
    {
        base.Start();
        grid.levers.Add(this);
        LeverFlick = Resources.Load<AudioClip>("Audio/LeverFlick");
    }

    public void Flick()
    {
        AudioManager.instance.PlaySound(LeverFlick);
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
