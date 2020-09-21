using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Belt : MonoBehaviour
{
    [SerializeField] private Transform[] beltNodes;
    [SerializeField] private Platform Platform;
    [SerializeField] List<Transform> Path = new List<Transform>();

    private Transform currentNode;
    private float speed = 5;
    private bool _atEnd = false;
    private bool _isMoving = false;

    //calculations
    [SerializeField] private Vector2 _start;
    [SerializeField] private Vector2 _end;
    private float _distance;
    private float _time = 0;

    private void Start()
    {
        _start = Platform.transform.position;
        _end = Platform.transform.position;
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Move();
        }
        _time += Time.deltaTime;
        Platform.transform.position = Vector2.Lerp(_start, _end, _time / _distance * speed);
    }
    public void Move()
    {
        Path = new List<Transform>();
        if (_atEnd)
        {
            if (_isMoving)//moving from end to beginning
            {
                Path.Add(Platform.transform);
                for (int i = Array.IndexOf(beltNodes, currentNode) - 1; i >= 0; --i)
                {
                    Path.Add(beltNodes[i]);
                }
            }
            else
            {
                for (int i = beltNodes.Length - 1; i >= 0; --i)
                {
                    Path.Add(beltNodes[i]);
                }
            }
        }
        else {
            if (_isMoving)//moving from origin to end
            {
                Path.Add(Platform.transform);
                for (int i = Array.IndexOf(beltNodes, currentNode) + 1; i < beltNodes.Length; ++i)
                {
                    Path.Add(beltNodes[i]);
                }
            }
            else 
            {
                for (int i = 0; i < beltNodes.Length; ++i)
                {
                    Path.Add(beltNodes[i]);
                }
            }
        }
        _atEnd = !_atEnd;
        StartCoroutine(MoveRoutine(Path.ToArray()));
    }

    private IEnumerator MoveRoutine(Transform[] Path)
    {
        _isMoving = true;
        for (int i = 1; i < Path.Length; ++i)
        {
            _time = 0;
            currentNode = Path[i];

            _start = Path[i - 1].position;
            _end = Path[i].position;
            _distance = Vector3.Distance(_start, _end);

            while ((Vector2)Platform.transform.position != (Vector2)Path[i].position)
            {
                yield return null;
            }
        }
        _isMoving = false;
    }
}
