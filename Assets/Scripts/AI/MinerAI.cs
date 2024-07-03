using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinerAI : Extract
{
    [SerializeField] private OreFinder _oreFinder;
    [SerializeField] private float _moveSpeed;

    private Vector2 _posToMove;
    private Rigidbody2D _rb;
    private Animator _animator;

    private bool _isMoving;
    private bool _isMining;

    private GameObject _targetObject;

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

    protected override bool CheckIfMinerMoving()
    {
        return false;
    }

    protected override void StopMining()
    {
        _isMining = false;
        _targetObject = null;
    }

    protected override void ExtractResource()
    {
        _isMining = true;
        _isMoving = false;
        base.ExtractResource();
    }
}
