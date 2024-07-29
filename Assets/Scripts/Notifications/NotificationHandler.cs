using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NotificationHandler : MonoBehaviour
{
    public static NotificationHandler Instance;
    [SerializeField] private bool _debugMode = false;
    [SerializeField] private GameObject _notificationPrefab;
    [SerializeField] private float _maxNotificationsCount = 50f;
    
    private void Awake()
    {
        Instance = this;
    }

    public void ShowNotification (GameObject callingObject, ResourceType resourceType, float deltaNumber)
    {
        if (_debugMode)
            Debug.Log("Extract notification called, resource type - " + resourceType);
        
        if(this.transform.childCount >= _maxNotificationsCount)
        {
            return;
        }

        Vector3 _spawnOffset = ConfigureSpawnOffset();
        ConfigureNotificationText(deltaNumber, resourceType);
        ConfigureNotficationMiniIcon(resourceType);
        if (_notificationPrefab != null)
            Instantiate(_notificationPrefab, callingObject.transform.position + _spawnOffset, _notificationPrefab.transform.rotation, this.transform);
    }

    private Vector3 ConfigureSpawnOffset()
    {
        return new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f));
    }

    private void ConfigureNotficationMiniIcon(ResourceType resourceType)
    {
        Image resourceImage = _notificationPrefab.GetComponent<ResourceAddedNotification>().ResourceImage;
        resourceImage.sprite = ResourceManager.Instance.GetResourceByType(resourceType).ResourceMiniSprite;
    }

    private void ConfigureNotificationText(float deltaNumber, ResourceType resourceType)
    {
        TextMeshProUGUI notificationText = _notificationPrefab.GetComponent<ResourceAddedNotification>().NotificationText;
        notificationText.text = $"{deltaNumber}";
        notificationText.color = ResourceManager.Instance.GetResourceByType(resourceType).ResourceColor;
    }
}
