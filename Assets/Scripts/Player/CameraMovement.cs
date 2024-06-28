using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{

    private Camera _camera;
    [SerializeField]
    private float _cameraSpeed;
    [SerializeField]
    private Vector3 _cameraDistance;
    private void Start()
    {
        _camera = Camera.main;
    }
    void FixedUpdate()
    {
        _camera.transform.position = Vector3.Lerp(_camera.transform.position, transform.position + _cameraDistance,Time.fixedDeltaTime * _cameraSpeed);
    }
}
