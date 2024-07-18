using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using YG;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement Instance;
    public bool IsMoving = false;
    public bool IsMining = false;

    [SerializeField] private bool _debugMode = false;
    [SerializeField] private float _moveSpeed = 1f;
    [SerializeField] private GameObject _pointerPrefab;

    private Rigidbody2D _playerRB;
    private Animator _animator;
    private Camera _mainCamera;
    private SpriteRenderer _spriteRenderer;
    private Vector2 _posToMove;
    private float _startXScale;

    private Vector3 _lastPosition;
    [SerializeField]private float _minStopAnim;

    [SerializeField] private GameObject _joystick;
    private bool _isMobileDevice;

    void Awake()
    {
        Instance = this;
        _startXScale = transform.localScale.x;
        _playerRB = GetComponent<Rigidbody2D>();
        _mainCamera = Camera.main;
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        _isMobileDevice = YandexGame.EnvironmentData.isMobile;
        if (_isMobileDevice)
            _joystick.SetActive(true);
        if (_debugMode)
            Debug.Log("Camera founded - " + _mainCamera.ToString());
    }
    private void Start()
    {
        _posToMove = transform.position;
    }

    void Update()
    {
        if (_isMobileDevice)
            MobileMovement();
        else
            DesktopMovement();
    }
    private void MobileMovement()
    {
        WalkAnimState();
        IsMovingCheck();

        _posToMove = (Vector2)transform.position + _joystick.GetComponent<Joystick>().Direction;

        FlipCheck();
    }
    private void DesktopMovement()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (YandexGame.EnvironmentData.language == "ru")
                YandexGame.EnvironmentData.language = "en";
            else
                YandexGame.EnvironmentData.language = "ru";
        }
        WalkAnimState();
        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject())
                return;

            GetPosToMove();
            if (_debugMode)
            {
                Debug.Log("Click 0 To pos: x - " + _posToMove.x + " y - " + _posToMove.y);
                Instantiate(_pointerPrefab, _posToMove, _pointerPrefab.transform.rotation);
            }

        }

        IsMovingCheck();

        if (Input.GetMouseButton(0))
        {
            if (EventSystem.current.IsPointerOverGameObject())
                return;

            IsMoving = true;
            GetPosToMove();
        }
        if (Input.GetMouseButtonDown(1))
        {
            _posToMove = transform.position;
        }

        FlipCheck();
    }
    private void FixedUpdate()
    {
        MovePlayer();
        MiningAnimState();
        //WalkAnimState();
        _lastPosition = transform.position;
    }

    private void MiningAnimState()
    {
        _animator.SetBool("Mining", IsMining);
    }

    private void GetPosToMove()
    {
        Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);

        RaycastHit2D hit = Physics2D.GetRayIntersection(ray, Mathf.Infinity, LayerMask.GetMask("Ground"));

        if (hit.point != Vector2.zero)
            _posToMove = hit.point;
        else
            _posToMove = transform.position;
    }

    private void FlipCheck()
    {
        //if (_posToMove.x < transform.position.x)
        //{
        //    _spriteRenderer.flipX = true;
        //}
        //if (_posToMove.x > transform.position.x)
        //{
        //    _spriteRenderer.flipX = false;
        //}

        // Если партиклы не понадобятся, то вернуть код сверху

        if (_posToMove.x < transform.position.x)
        {
            transform.localScale = new Vector3(-_startXScale, transform.localScale.y, transform.localScale.z);
        }
        if (_posToMove.x > transform.position.x)
        {
            transform.localScale = new Vector3(_startXScale, transform.localScale.y, transform.localScale.z);
        }

    }

    private void WalkAnimState()
    {
        Vector3 delta = _lastPosition - transform.position;
        bool isWalking = false;
        if (Mathf.Abs(delta.x) > _minStopAnim || Mathf.Abs(delta.y) > _minStopAnim)
            isWalking = true;
        _animator.SetBool("Moving", isWalking);
    }
    private void IsMovingCheck()
    {
        if ((Vector2)transform.position != _posToMove)
        {
            IsMoving = true;
        }
        else
        {
            IsMoving = false;
        }
    }
    private void MovePlayer()
    {
        _playerRB.MovePosition(Vector2.MoveTowards(transform.position, _posToMove, _moveSpeed * Time.fixedDeltaTime));
        //transform.position = Vector2.MoveTowards(transform.position, _posToMove, _moveSpeed * Time.deltaTime);
    }

}
