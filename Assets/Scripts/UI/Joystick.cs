﻿using UnityEngine;
using UnityEngine.EventSystems;

public class Joystick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    public float Horizontal { get { return (_snapX) ? SnapFloat(_input.x, AxisOptions.Horizontal) : _input.x; } }
    public float Vertical { get { return (_snapY) ? SnapFloat(_input.y, AxisOptions.Vertical) : _input.y; } }
    public Vector2 Direction { get { return new Vector2(Horizontal, Vertical); }}

    public float HandleRange
    {
        get { return _handleRange; }
        set { _handleRange = Mathf.Abs(value); }
    }

    public float DeadZone
    {
        get { return _deadZone; }
        set { _deadZone = Mathf.Abs(value); }
    }

    public AxisOptions AxisOptions { get { return AxisOptions; } set { _axisOptions = value; } }
    public bool SnapX { get { return _snapX; } set { _snapX = value; } }
    public bool SnapY { get { return _snapY; } set { _snapY = value; } }

    [SerializeField] private float _handleRange = 1;
    [SerializeField] private float _deadZone = 0;
    [SerializeField] private AxisOptions _axisOptions = AxisOptions.Both;
    [SerializeField] private bool _snapX = false;
    [SerializeField] private bool _snapY = false;

    [SerializeField] protected RectTransform _background = null;
    [SerializeField] private RectTransform _handle = null;
    private RectTransform _baseRect = null;

    private Canvas _canvas;
    private Camera _cam;

    private Vector2 _defaultPos = Vector2.zero;

    public Vector2 _input = Vector2.zero;

    private void Start()
    {
        _defaultPos = _background.position;
        HandleRange = _handleRange;
        DeadZone = _deadZone;
        _baseRect = GetComponent<RectTransform>();
        _canvas = GetComponentInParent<Canvas>();
        if (_canvas == null)
            Debug.LogError("The Joystick is not placed inside a canvas");

        Vector2 center = new Vector2(0.5f, 0.5f);
        _background.pivot = center;
        _handle.anchorMin = center;
        _handle.anchorMax = center;
        _handle.pivot = center;
        _handle.anchoredPosition = _background.anchoredPosition;

    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        _background.anchoredPosition = ScreenPointToAnchoredPosition(eventData.position);
        OnDrag(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        _cam = null;
        if (_canvas.renderMode == RenderMode.ScreenSpaceCamera)
            _cam = _canvas.worldCamera;

        Vector2 position = RectTransformUtility.WorldToScreenPoint(_cam, _background.position);
        Vector2 radius = _background.sizeDelta / 2;
        _input = (eventData.position - position) / (radius * _canvas.scaleFactor);
        FormatInput();
        HandleInput(_input.magnitude, _input.normalized, radius, _cam);
        _handle.anchoredPosition = _input * radius * _handleRange + _background.anchoredPosition;
    }

    protected virtual void HandleInput(float magnitude, Vector2 normalised, Vector2 radius, Camera cam)
    {
        if (magnitude > _deadZone)
        {
            if (magnitude > 1)
                _input = normalised;
        }
        else
            _input = Vector2.zero;
    }

    private void FormatInput()
    {
        if (_axisOptions == AxisOptions.Horizontal)
            _input = new Vector2(_input.x, 0f);
        else if (_axisOptions == AxisOptions.Vertical)
            _input = new Vector2(0f, _input.y);
    }

    private float SnapFloat(float value, AxisOptions snapAxis)
    {
        if (value == 0)
            return value;

        if (_axisOptions == AxisOptions.Both)
        {
            float angle = Vector2.Angle(_input, Vector2.up);
            if (snapAxis == AxisOptions.Horizontal)
            {
                if (angle < 22.5f || angle > 157.5f)
                    return 0;
                else
                    return (value > 0) ? 1 : -1;
            }
            else if (snapAxis == AxisOptions.Vertical)
            {
                if (angle > 67.5f && angle < 112.5f)
                    return 0;
                else
                    return (value > 0) ? 1 : -1;
            }
            return value;
        }
        else
        {
            if (value > 0)
                return 1;
            if (value < 0)
                return -1;
        }
        return 0;
    }

    public virtual void OnPointerUp(PointerEventData eventData)
    {
        _input = Vector2.zero;
        _background.position = _defaultPos; 
        _handle.anchoredPosition = _background.anchoredPosition;
    }

    private Vector2 ScreenPointToAnchoredPosition(Vector2 screenPosition)
    {
        Vector2 localPoint = Vector2.zero;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(_baseRect, screenPosition, _cam, out localPoint))
        {
            Vector2 pivotOffset = _baseRect.pivot * _baseRect.sizeDelta;
            return localPoint - (_background.anchorMax * _baseRect.sizeDelta) + pivotOffset;
        }
        return Vector2.zero;
    }
    //public override void OnPointerDown(PointerEventData eventData)
    //{
    //    _background.anchoredPosition = ScreenPointToAnchoredPosition(eventData.position);
    //    _background.gameObject.SetActive(true);
    //    base.OnPointerDown(eventData);
    //}

    //public override void OnPointerUp(PointerEventData eventData)
    //{
    //    //background.gameObject.SetActive(false);
    //    base.OnPointerUp(eventData);
    //}
}

public enum AxisOptions { Both, Horizontal, Vertical }