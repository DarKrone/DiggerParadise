using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotificationHandler : MonoBehaviour
{
    public static NotificationHandler Instance;
    [SerializeField] private bool _debugMode = false;
    [SerializeField] private GameObject _player;
    [SerializeField] private Vector3 _spawnOffset;
    [SerializeField] private List<GameObject> _notificationPrefabs;
    private void Awake()
    {
        Instance = this;
    }

    public void ShowNotification(ResourceType resourceType)
    {
        if(_debugMode)
            Debug.Log("Extract notification called, resource type - " + resourceType);
        GameObject notificationPrefab = _notificationPrefabs[0];
        switch (resourceType)
        {
            case ResourceType.Copper:
                if (_notificationPrefabs[1] != null )
                    notificationPrefab = _notificationPrefabs[1];
                break;
            case ResourceType.Iron:
                if (_notificationPrefabs[2] != null)
                    notificationPrefab = _notificationPrefabs[2];
                break;
        }
        if(notificationPrefab != null) 
            Instantiate(notificationPrefab, _player.transform.position + _spawnOffset, notificationPrefab.transform.rotation, this.transform);
    }
}
