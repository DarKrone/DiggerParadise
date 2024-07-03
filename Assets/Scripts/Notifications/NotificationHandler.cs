using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NotificationHandler : MonoBehaviour
{
    public static NotificationHandler Instance;
    [SerializeField] private bool _debugMode = false;
    [SerializeField] private float _notificationScale;
    [SerializeField] private GameObject _notificationPrefab;
    
    private void Awake()
    {
        Instance = this;
    }

    public void ShowNotification (GameObject callingObject, ResourceType resourceType, float deltaNumber)
    {
        if(_debugMode)
            Debug.Log("Extract notification called, resource type - " + resourceType);
        Vector3 _spawnOffset = new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f));
        GameObject notificationPrefab = _notificationPrefab;
        notificationPrefab.transform.localScale = new Vector3(_notificationScale, _notificationScale, 1);
        TextMeshProUGUI notificationText = notificationPrefab.GetComponent<ResourceAddedNotification>().NotificationText;
        notificationText.text = $"{deltaNumber}";
        notificationText.color = Storage.Instance.GetResourceColorByType(resourceType);
        if (notificationPrefab != null) 
            Instantiate(notificationPrefab, callingObject.transform.position + _spawnOffset, notificationPrefab.transform.rotation, this.transform);
    }
}
