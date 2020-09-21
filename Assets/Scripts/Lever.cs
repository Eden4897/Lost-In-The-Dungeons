using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour
{
    [SerializeField] private GridManager grid;
    [SerializeField] private Belt linkedBelt;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite onSprite;
    [SerializeField] private Sprite offSprite;

    private bool isOn = false;
    private void Start()
    {
        grid.levers.Add(transform.position - new Vector3(0.5f,0.5f), this);
    }

    public void Flick()
    {
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
