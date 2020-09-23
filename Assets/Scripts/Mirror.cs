using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mirror : GameElement
{
    [SerializeField] public Direction[] directions = new Direction[2];
    [SerializeField] private Sprite normal;
    [SerializeField] private Sprite reflecting;

    protected override void Start()
    {
        base.Start();
        grid.mirrors.Add(this);
    }

    public Direction Reflect(Direction dir)
    {
        if (!Array.Exists(directions, direction => Reverse(direction) == dir)) return Direction.Null;
        int incidence = Array.IndexOf(directions, Reverse(dir));
        return incidence == 0 ? directions[1] : directions[0];
    }
}
