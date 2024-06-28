using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private bool _debugMode = false;
    [SerializeField] private float _moveSpeed = 1f;
    [SerializeField] private GameObject _pointerPrefab;
    private Camera _mainCamera;
    private Vector3 _posToMove;

    void Awake()
    {
        _mainCamera = Camera.main;
        if (_debugMode)
            Debug.Log("Camera founded - " + _mainCamera.ToString());
        _posToMove = transform.position;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit = new RaycastHit();
            Physics.Raycast(ray, out hit, LayerMask.GetMask("Ground"));
            _posToMove = hit.point;
            Instantiate(_pointerPrefab, _posToMove, _pointerPrefab.transform.rotation);
            if (_debugMode)
                Debug.Log("Click 0 To pos: x - " + _posToMove.x + " y - " + _posToMove.y + " z - " + _posToMove.z);
        }
        if (Input.GetMouseButton(0))
        {
            Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit = new RaycastHit();
            Physics.Raycast(ray, out hit, LayerMask.GetMask("Ground"));
            _posToMove = hit.point;
        }
        if (Input.GetMouseButtonDown(1))
        {
            _posToMove = transform.position;
        }
    }
    void FixedUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, _posToMove, Time.fixedDeltaTime * _moveSpeed);
    }
}
