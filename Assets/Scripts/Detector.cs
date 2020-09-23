using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detector : GameElement
{
    [SerializeField] private Direction detectingDirection;
    [SerializeField] private Belt linkedBelt;
    [SerializeField] private Sprite normal;
    [SerializeField] private Sprite detected;
    private SpriteRenderer spriteRenderer;
    private Laser currentLaser;

    protected override void Start()
    {
        base.Start();
        spriteRenderer = GetComponent<SpriteRenderer>();
        grid.detectors.Add(this);
    }

    protected override void Update()
    {
        base.Update();
    }
    public override void OnStartMove()
    {
        base.OnStartMove();
        if(currentLaser != null)
        {
            currentLaser.ClearAll();
        }
    }
    public override void OnMoved()
    {
        base.OnMoved();
        if (currentLaser != null) 
        {
            StartCoroutine(currentLaser.LaserCast());
            currentLaser = null;
        }
    }
    public bool Detect(Direction direction)
    {
        if (detectingDirection == Reverse(direction)) return true;
        return false;
    }

    public void Cast(Laser laser)
    {
        spriteRenderer.sprite = detected;
        //linkedBelt.Move();
        currentLaser = laser;
    }

    public void DeCast()
    {
        spriteRenderer.sprite = normal;
        //linkedBelt.Move();
    }
}
