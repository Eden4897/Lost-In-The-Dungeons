using System;
using System.Collections;
using UnityEngine;

public class Player : GameElement
{
    [SerializeField] private float speed = 0;
    [SerializeField] public string WalkUp;
    [SerializeField] public string WalkDown;
    [SerializeField] public string WalkLeft;
    [SerializeField] public string WalkRight;
    [SerializeField] public string Idle;

    [HideInInspector] public bool _isMoving = false;
    [HideInInspector] public SpriteRenderer spriteRenderer;
    [HideInInspector] public Animator animator;

    private Platform currenPlatform = null;
    private FallingPlatform currenFallingPlatform = null;
    private AudioClip walking;
    private AudioClip coin;

    protected override void Start()
    {
        base.Start();
        position = new Vector2Int(Mathf.RoundToInt(transform.position.x - 0.5f), Mathf.RoundToInt(transform.position.y - 1f));
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        walking = Resources.Load<AudioClip>("Audio/Walking");
        coin = Resources.Load<AudioClip>("Audio/Coin");
    }
    protected override void Update()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.y);
    }
    public IEnumerator Move(Vector3 target, Action endEvent = null)
    {
        //premove
        AudioManager.instance.PlaySound(walking);
        target.z = target.y;
        transform.parent = null;
        _isMoving = true;

        //setting tiles
        if (currentPlate != null)
        {
            currentPlate.Release();
            currentPlate = null;
        }
        if (currenPlatform != null)
        {
            currenPlatform.player = null;
            currenPlatform = null;
        }
        if (currenFallingPlatform != null)
        {
            currenFallingPlatform.Damage();
            currenFallingPlatform = null;
        }

        //moving
        Vector3 start = transform.position;
        float t = 0;
        while (transform.position != target)
        {
            t += Time.deltaTime;
            transform.position = Vector3.Lerp(start, target, t * speed);
            yield return null;
        }

        //promove
        _isMoving = false;
        position = new Vector2Int(Mathf.RoundToInt(target.x - 0.5f), Mathf.RoundToInt(target.y - 1f));
        animator.Play(Idle);

        //setting tiles
        Platform platform = grid.GetPlatform(position);
        if (platform != null)
        {
            transform.parent = platform.transform;
            currenPlatform = platform;
            platform.player = this;
        }
        FallingPlatform fallingPlatform = grid.GetFallingPlatform(position);
        if (fallingPlatform != null)
        {
            currenFallingPlatform = fallingPlatform;
        }
        PressurePlate plate = grid.GetPlate(position);
        if (plate != null)
        {
            currentPlate = plate;
            currentPlate.Step();
        }
        endEvent?.Invoke();
        foreach (Laser laser in grid.lasers)
        {
            StartCoroutine(laser.LaserCast());
        }
    }

    protected override void OnCollisionEnter2D(Collision2D collider)
    {
        if (collider.gameObject.CompareTag("Collectables"))
        {
            AudioManager.instance.PlaySound(coin);
            grid.activeCollectables--;
            Destroy(collider.gameObject);            
        }
    }
}
