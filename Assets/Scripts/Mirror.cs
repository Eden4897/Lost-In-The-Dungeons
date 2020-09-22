using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mirror : GameElement
{
    [SerializeField] public Direction[] directions = new Direction[2];
    [SerializeField] private Sprite normal;
    [SerializeField] private Sprite reflecting;
    [SerializeField] private Sprite anti;

    protected override void Start()
    {
        base.Start();
        grid.mirrors.Add(this);
    }

    public Direction Reflect(Direction dir)
    {
        Debug.Log(dir);
        if (!Array.Exists(directions, direction => Reverse(direction) == dir)) return Direction.Null;
        int incidence = Array.IndexOf(directions, Reverse(dir));
        return incidence == 0 ? directions[1] : directions[0];
    }

    public void Cast(Direction dir)
    {
        GetComponent<SpriteRenderer>().sprite = Reverse(dir) == directions[0] ? reflecting : anti;
    }

    public void DeCast()
    {
        GetComponent<SpriteRenderer>().sprite = normal;
    }
}
