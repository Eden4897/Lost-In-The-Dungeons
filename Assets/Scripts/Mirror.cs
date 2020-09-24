using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mirror : GameElement
{
    [SerializeField] public Direction[] directions = new Direction[2];

    public List<Laser> currentLasers = new List<Laser>();

    protected override void Start()
    {
        base.Start();
        isPushable = true;
        grid.mirrors.Add(this);
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
        transform.parent = null;
        if (currentPlatform != null)
        {
            currentPlatform.gameElements.Remove(this);
        }
        if(currentPlate != null)
        {
            currentPlate.Release();
        }
        currentPlatform = null;
        currentPlate = null;
        for (int i = 0; i < currentLasers.Count; ++i)
        {
            StartCoroutine(currentLasers[i].LaserCast());
        }
        currentLasers.Clear();
        Platform platform = grid.GetPlatform(position);
        if (platform != null)
        {
            transform.parent = platform.transform;
            platform.gameElements.Add(this);
            currentPlatform = platform;
        }

        PressurePlate plate = grid.GetPlate(position);
        if (plate != null)
        {
            plate.Step();
            currentPlate = plate;
        }
    }

    public Direction Reflect(Direction dir)
    {
        if (!Array.Exists(directions, direction => Reverse(direction) == dir)) return Direction.Null;
        int incidence = Array.IndexOf(directions, Reverse(dir));
        return incidence == 0 ? directions[1] : directions[0];
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
