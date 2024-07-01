using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResourceAddedNotification : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI NotificationText;
    [SerializeField] private float _flySpeed;
    private float _red, _green, _blue;
    private float _alpha = 1f;
    private float _fadeOutSpeed = 1f;
    private void Start()
    {
        _red = NotificationText.color.r;
        _green = NotificationText.color.g;
        _blue = NotificationText.color.b;
        Destroy(gameObject, _fadeOutSpeed * 2f);
    }

    private void Update()
    {
        gameObject.transform.position += Vector3.up * Time.deltaTime * _flySpeed;
        NotificationText.color = new Color(_red,_green,_blue,_alpha);
        _alpha = _alpha - Time.deltaTime * _fadeOutSpeed;
        _alpha = Mathf.Clamp01(_alpha);
    }
}
