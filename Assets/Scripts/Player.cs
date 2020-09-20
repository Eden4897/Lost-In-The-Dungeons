using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [HideInInspector] public bool _isMoving = false;
    [SerializeField] private Grid grid;
    [SerializeField] private float speed = 0;
    [SerializeField] public Vector2Int position;

    private void Update()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.y);
    }
    public IEnumerator Move(Vector3 target)
    {
        _isMoving = true;
        Vector3 start = transform.position;
        float t = 0;
        while ((Vector2)transform.position != (Vector2)target)
        {
            t += Time.deltaTime;
            transform.position = Vector3.Lerp(start, target, t * speed);
            yield return null;
        }
        _isMoving = false;
        position = new Vector2Int((int)(target.x - 0.5f), (int)(target.y - 1f));
    }
}
