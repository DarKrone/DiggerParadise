using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinerAI : MonoBehaviour
{
    [SerializeField] private bool _debugMode = false;
    [SerializeField] private OreFinder _oreFinder;
    [SerializeField] private float _moveSpeed;

    private Vector2 _posToMove;
    private Rigidbody2D _rb;
    private Animator _animator;

    private bool _isMoving;
    private bool _isMining;

    private GameObject _targetObject;

    private Resource _currentResource;
    private Coroutine _extractionCoroutine;
    private void Start()
    {
        _animator = GetComponent<Animator>();
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
                if (_targetObject.activeSelf)
                    MoveToPoint();
                else
                    _isMoving = false;
            }
            else
            {
                _oreFinder.FindClosestPoint(ref _targetObject);
                if (_targetObject != null)
                {
                    _posToMove = _targetObject.transform.position;
                    _isMoving = true;
                }
            }
        }

        MiningAnimState();
        WalkAnimState();
    }

    private void MiningAnimState()
    {
        _animator.SetBool("Mining", _isMining);
    }

    private void WalkAnimState()
    {
        _animator.SetBool("Moving", _isMoving);
    }

    private void MoveToPoint()
    {
        _rb.MovePosition(Vector2.MoveTowards(transform.position, _posToMove, _moveSpeed * Time.fixedDeltaTime));
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Resource"))
        {
            return;
        }
        _isMining = true;
        _currentResource = collision.GetComponent<Resource>();
        _extractionCoroutine = StartCoroutine(Extracting());
        _targetObject = null;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Resource"))
        {
            return;
        }
        _isMining = false;
        _currentResource = null;
        StopCoroutine(_extractionCoroutine);
    }

    protected IEnumerator Extracting()
    {
        float extractionSpeed = Storage.Instance.GetExtractionSpeedByType(_currentResource.ResourceType);
        while (true)
        {
            yield return new WaitForSeconds(1 / extractionSpeed);
            if (_currentResource.isFullyExtracted)
            {
                StopCoroutine(_extractionCoroutine);
                break;
            }
            ExtractResource();
            ResourceNotification();
            GameManager.Instance.UpdateUI();
            if (_debugMode)
                DebugResourceAmount();
        }
    }

    protected virtual void ExtractResource()
    {
        Storage.Instance.AddToStorage(Storage.Instance.GetExtractionAmountByType(_currentResource.ResourceType), _currentResource.ResourceType);
        _currentResource.ResourceAmount -= Storage.Instance.GetExtractionAmountByType(_currentResource.ResourceType);
    }

    protected virtual void ResourceNotification()
    {
        NotificationHandler.Instance.ShowNotification(this.gameObject, _currentResource.ResourceType, true);
    }

    protected virtual void DebugResourceAmount()
    {
        Debug.Log($"Current {_currentResource.ResourceType} amount - {Storage.Instance.CheckResourceAmount(_currentResource.ResourceType)}");
    }
}
