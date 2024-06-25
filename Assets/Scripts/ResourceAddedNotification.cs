using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResourceAddedNotification : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _notificationText;
    private float _red, _green, _blue;
    private float _alpha = 1f;
    private float _fadeOutSpeed = 1f;
    private void Start()
    {
        _red = _notificationText.color.r;
        _green = _notificationText.color.g;
        _blue = _notificationText.color.b;
        Destroy(gameObject, _fadeOutSpeed * 2f);
    }

    private void Update()
    {
        gameObject.transform.position += Vector3.up * Time.deltaTime;
        _notificationText.color = new Color(_red,_green,_blue,_alpha);
        _alpha = _alpha - Time.deltaTime * _fadeOutSpeed;
        _alpha = Mathf.Clamp01(_alpha);

    }
}
