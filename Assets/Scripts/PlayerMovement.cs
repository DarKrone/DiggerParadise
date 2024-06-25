using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private bool _debugMode = false;
    [SerializeField] private float _moveSpeed = 1f;
    [SerializeField] private GameObject _pointerPrefab;
    private Camera _mainCamera;
    private Vector2 _posToMove;

    void Awake()
    {
        _mainCamera = Camera.main;
        if (_debugMode)
            Debug.Log("Camera founded - " + _mainCamera.ToString());
    }

    void FixedUpdate()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _posToMove = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
            Instantiate(_pointerPrefab, _posToMove, _pointerPrefab.transform.rotation);
            if (_debugMode)
                Debug.Log("Click 0 To pos: x - " + _posToMove.x + " y - " + _posToMove.y);
        }
        if (Input.GetMouseButton(0))
        {
            _posToMove = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
        }
        if (Input.GetMouseButtonDown(1))
        {
            _posToMove = transform.position;
        }

        transform.position = Vector2.Lerp(transform.position, _posToMove, Time.fixedDeltaTime * _moveSpeed);
        
    }
}
