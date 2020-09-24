using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private new Camera camera;
    [SerializeField] private Player player1;
    [SerializeField] private Player player2;
    private float movementSmoothing;
    private Vector2 _target;
    private float _size;
    private float _widthMinSize;
    private float _heightMinSize;
    private Vector3 _velocity = Vector3.zero;

    private void Update()
    {
        _widthMinSize = Mathf.Abs(player2.transform.position.x - player1.transform.position.x) / 2 + 1;
        _heightMinSize = Mathf.Abs(player2.transform.position.y - player1.transform.position.y) / 2 + 1;
        _size = Mathf.Max(_heightMinSize, 1 / camera.aspect * _widthMinSize);
        camera.orthographicSize = Mathf.Clamp(_size, 5, Mathf.Infinity);

        _target = ((Vector2)player1.transform.position + (Vector2)player2.transform.position) / 2;
        Vector3 _calculatedTarget = Vector3.SmoothDamp(transform.position, _target, ref _velocity, movementSmoothing);
        _calculatedTarget.z = -10;
        transform.position = _calculatedTarget;
    }
}
