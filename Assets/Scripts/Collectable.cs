using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    private GameManager grid;
    private void Start()
    {
        grid = GameManager.instance;
        grid.activeCollectables++;
    }
}
