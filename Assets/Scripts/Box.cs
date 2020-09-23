using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : GameElement
{
    public List<Laser> currentLasers = new List<Laser>();
    protected override void Start()
    {
        base.Start();
        isPushable = true;
        grid.boxes.Add(this);
    }

    public override void OnStartMove()
    {
        base.OnStartMove();
        for (int i = 0; i < currentLasers.Count; ++i)
        {
            currentLasers[i].ClearAll();
        }
    }
    public override void OnMoved()
    {
        base.OnMoved();
        for (int i = 0; i < currentLasers.Count; ++i)
        {
            StartCoroutine(currentLasers[i].LaserCast());
        }
        currentLasers.Clear();
    }
    public void Cast(Laser laser)
    {
        currentLasers.Add(laser);
    }

    public void DeCast(Laser laser)
    {
        currentLasers.Remove(laser);
    }
}
