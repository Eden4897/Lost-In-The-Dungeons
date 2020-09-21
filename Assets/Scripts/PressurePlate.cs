using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    [SerializeField] private Belt linkedBelt;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite steppedSprite;
    [SerializeField] private Sprite releasedSprite;

    private GridManager grid;

    private int _occupants;

    private void Start()
    {
        grid = GridManager.instance;
        grid.pressurePlates.Add(transform.position - new Vector3(0.5f, 0.5f), this);
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
