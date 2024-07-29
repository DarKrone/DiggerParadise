using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResourceAddedNotification : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI NotificationText;
    [SerializeField] public Image ResourceImage;
    [SerializeField] private float _flySpeed;
    private float _red, _green, _blue;
    private float _alpha = 1f;
    private float _fadeOutSpeed = 1f;
    private void Start()
    {
        _red = NotificationText.color.r;
        _green = NotificationText.color.g;
        _blue = NotificationText.color.b;
        Destroy(gameObject, _fadeOutSpeed);
    }

    private void Update()
    {
        gameObject.transform.position += Vector3.up * Time.deltaTime * _flySpeed;
        NotificationText.color = new Color(_red,_green,_blue,_alpha);
        ResourceImage.color = new Color(1, 1, 1, _alpha);
        _alpha = _alpha - Time.deltaTime * _fadeOutSpeed;
        _alpha = Mathf.Clamp01(_alpha);
    }
}
