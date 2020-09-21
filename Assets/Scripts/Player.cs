using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float speed = 0;
    [SerializeField] public Vector2Int position;
    [SerializeField] public string WalkUp;
    [SerializeField] public string WalkDown;
    [SerializeField] public string WalkLeft;
    [SerializeField] public string WalkRight;
    [SerializeField] public string Idle;

    [HideInInspector] public bool _isMoving = false;
    private Animator animator;
    private GridManager grid;
    private PressurePlate currentPlate = null;

    private void Start()
    {
        grid = GridManager.instance;
        animator = GetComponent<Animator>();
    }
    private void Update()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.y);
    }
    public IEnumerator Move(Vector3 target)
    {
        target.z = target.y;
        if(currentPlate != null)
        {
            currentPlate.Release();
            currentPlate = null;
        }
        transform.parent = null;
        _isMoving = true;
        Vector3 start = transform.position;
        float t = 0;
        while (transform.position != target)
        {
            t += Time.deltaTime;
            transform.position = Vector3.Lerp(start, target, t * speed);
            yield return null;
        }
        _isMoving = false;
        position = new Vector2Int(Mathf.RoundToInt(target.x - 0.5f), Mathf.RoundToInt(target.y - 1f));
        foreach (Platform platform in grid.platforms)
        {
            if(platform.position == position && platform.isStationary)
            {
                transform.parent = platform.transform;
            }
        }
        if (grid.pressurePlates.ContainsKey(position))
        {
            currentPlate = grid.pressurePlates[position];
            currentPlate.Step();
        }
        animator.Play(Idle);
    }

    public void playAnimation(string name)
    {
        animator.Play(name);
    }
}
