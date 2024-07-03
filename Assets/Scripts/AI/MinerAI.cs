using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinerAI : MonoBehaviour
{
    [SerializeField] private OreFinder _oreFinder;
    [SerializeField] private float _moveSpeed;

    private Vector2 _posToMove;
    private Rigidbody2D _rb;

    private bool _isMoving;
    private bool _isMining;

    private GameObject targetObject;

    private Resource _currentResource;
    private Coroutine _extractionCoroutine;
    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }
    void FixedUpdate()
    {
        if(_isMining)
        {
            _isMoving = false;
        }
        else
        {
            if (_isMoving)
            {
                if (targetObject.activeSelf)
                    MoveToPoint();
                else
                    _isMoving = false;
            }
            else
            {
                _oreFinder.FindClosetPoint(ref targetObject);
                if (targetObject != null)
                {
                    _posToMove = targetObject.transform.position;
                    _isMoving = true;
                }
            }
        }
    }
    private void MoveToPoint()
    {
        _rb.MovePosition(Vector2.MoveTowards(transform.position, _posToMove, _moveSpeed * Time.fixedDeltaTime));
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Resource"))
        {
            _isMining = true;
            _currentResource = collision.GetComponent<Resource>();
            _extractionCoroutine = StartCoroutine(_currentResource.Extracting());
            targetObject = null;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Resource"))
        {
            _isMining = false;
            _currentResource = null;
            StopCoroutine(_extractionCoroutine);
        }
    }
}
