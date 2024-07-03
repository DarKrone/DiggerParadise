using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Extract : MonoBehaviour
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
            if(CheckIfMinerMoving())
            {
                continue;
            }
            if (_currentResource.isFullyExtracted)
            {
                StopMining();
                StopCoroutine(_extractionCoroutine);
                break;
            }
            ExtractResource();
            ResourceNotification();
            GameManager.Instance.UpdateUI();
            if (_debugMode)
                DebugResourceAmount();
        }
    }

    protected virtual void StopMining()
    {
        throw new NotImplementedException();
    }

    protected virtual bool CheckIfMinerMoving()
    {
        throw new NotImplementedException();
    }

    protected virtual void ExtractResource()
    {
        Storage.Instance.AddToStorage(Storage.Instance.GetExtractionAmountByType(_currentResource.ResourceType), _currentResource.ResourceType);
        _currentResource.ResourceAmount -= Storage.Instance.GetExtractionAmountByType(_currentResource.ResourceType);
    }

    protected void ResourceNotification()
    {
        NotificationHandler.Instance.ShowNotification(this.gameObject, _currentResource.ResourceType, true);
    }

    protected void DebugResourceAmount()
    {
        Debug.Log($"Current {_currentResource.ResourceType} amount - {Storage.Instance.CheckResourceAmount(_currentResource.ResourceType)}");
    }
}
