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
        StopMining();
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
        float curResourceExtractAmount = Storage.Instance.GetExtractionAmountByType(_currentResource.ResourceType);
        if (curResourceExtractAmount > _currentResource.ResourceAmount)
        {
            curResourceExtractAmount = _currentResource.ResourceAmount;
        }
        Storage.Instance.AddToStorage(curResourceExtractAmount, _currentResource.ResourceType);
        _currentResource.ResourceAmount -= curResourceExtractAmount;
        ResourceNotification(curResourceExtractAmount);
    }

    protected void ResourceNotification(float curResourceExtractAmount)
    {
        NotificationHandler.Instance.ShowNotification(this.gameObject, _currentResource.ResourceType, curResourceExtractAmount);
    }

    protected void DebugResourceAmount()
    {
        Debug.Log($"Current {_currentResource.ResourceType} amount - {Storage.Instance.CheckResourceAmount(_currentResource.ResourceType)}");
    }
}
