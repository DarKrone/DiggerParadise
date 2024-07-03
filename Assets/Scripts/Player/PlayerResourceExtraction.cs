using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerResourceExtraction : MonoBehaviour
{
    [SerializeField] private bool _debugMode;
    private Coroutine _extractionCoroutine;
    private Resource _currentResource;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Resource"))
            return;
        if (_debugMode)
            Debug.Log($"Enter {collision.gameObject.name} resource");
        _currentResource = collision.gameObject.GetComponent<Resource>();
        _extractionCoroutine = StartCoroutine(Extracting());
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Resource"))
            return;
        if (_debugMode)
            Debug.Log($"Exit {collision.gameObject.name} resource");
        _currentResource = null;
        StopCoroutine(_extractionCoroutine);
        PlayerMovement.Instance.IsMining = false;
    }

    protected IEnumerator Extracting()
    {
        float extractionSpeed = Storage.Instance.GetExtractionSpeedByType(_currentResource.ResourceType);
        while (true)
        {
            yield return new WaitForSeconds(1 / extractionSpeed);
            if (PlayerMovement.Instance.IsMoving)
            {
                PlayerMovement.Instance.IsMining = false;
                continue;
            }
            if (_currentResource.isFullyExtracted)
            {
                PlayerMovement.Instance.IsMining = false;
                StopCoroutine(_extractionCoroutine);
                break;
            }
            PlayerMovement.Instance.IsMining = true;
            ExtractResource();
            ResourceNotification();
            GameManager.Instance.UpdateUI();
            if (_debugMode)
                DebugResourceAmount();
        }
    }

    protected virtual void ExtractResource()
    {
        Storage.Instance.AddToStorage(Storage.Instance.GetExtractionAmountByType(_currentResource.ResourceType), _currentResource.ResourceType);
        _currentResource.ResourceAmount -= Storage.Instance.GetExtractionAmountByType(_currentResource.ResourceType);
    }

    protected virtual void ResourceNotification()
    {
        NotificationHandler.Instance.ShowNotification(this.gameObject, _currentResource.ResourceType, true);
    }

    protected virtual void DebugResourceAmount()
    {
        Debug.Log($"Current {_currentResource.ResourceType} amount - {Storage.Instance.CheckResourceAmount(_currentResource.ResourceType)}");
    }
}

