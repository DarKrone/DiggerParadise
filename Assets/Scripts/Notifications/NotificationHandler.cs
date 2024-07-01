using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NotificationHandler : MonoBehaviour
{
    public static NotificationHandler Instance;
    [SerializeField] private bool _debugMode = false;
    [SerializeField] private GameObject _player;
    [SerializeField] private Vector3 _spawnOffset;
    [SerializeField] private float _notificationScale;
    [SerializeField] private GameObject _notificationPrefab;
    
    private void Awake()
    {
        Instance = this;
    }

    public void ShowNotification(ResourceType resourceType, bool isAdding)
    {
        if(_debugMode)
            Debug.Log("Extract notification called, resource type - " + resourceType);
        GameObject notificationPrefab = _notificationPrefab;
        notificationPrefab.transform.localScale = new Vector3(_notificationScale, _notificationScale, 1);
        TextMeshProUGUI notificationText = notificationPrefab.GetComponent<ResourceAddedNotification>().NotificationText;
        if (isAdding)
        {
            notificationText.text = "+1";
        }
        else
        {
            notificationText.text = "-1";
        }
        notificationText.color = Storage.Instance.GetResourceColorByType(resourceType);
        if (notificationPrefab != null) 
            Instantiate(notificationPrefab, _player.transform.position + _spawnOffset, notificationPrefab.transform.rotation, this.transform);
    }
}
