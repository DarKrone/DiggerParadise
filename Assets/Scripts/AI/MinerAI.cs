using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinerAI : Extract
{
    [SerializeField] private OreFinder _oreFinder;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _extractionSpeed;
    [SerializeField] private float _extractionAmount;

    private Vector2 _posToMove;
    private Rigidbody2D _rb;
    private Animator _animator;

    public bool _isMoving;
    public bool _isMining;

    private GameObject _targetObject;
    private SpriteRenderer _spriteRenderer;

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
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
                if (_targetObject != null && !_targetObject.GetComponent<ResourceOre>().isFullyExtracted)
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
        TurnMinerToMovePosition();
    }

    private void TurnMinerToMovePosition()
    {
        if (_posToMove.x < transform.position.x)
        {
            _spriteRenderer.flipX = true;
        }
        else
        {
            _spriteRenderer.flipX = false;
        }
    }

    protected override void SetExtractParams()
    {
        curResourceExtractAmount = _extractionAmount;
        curResourceExtractSpeed = _extractionSpeed;
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
