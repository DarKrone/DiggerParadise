using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceExtraction : MonoBehaviour
{
    [SerializeField] private bool _debugMode;
    private Coroutine _extractionCoroutine;
    private Resource _resource;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Resource"))
            return;
        if (_debugMode)
            Debug.Log($"Enter {collision.gameObject.name} resource");
        _resource = collision.gameObject.GetComponent<Resource>();
        _extractionCoroutine = StartCoroutine(Extracting());
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Resource"))
            return;
        if (_debugMode)
            Debug.Log($"Exit {collision.gameObject.name} resource");
        _resource = null;
        StopCoroutine(_extractionCoroutine);
        PlayerMovement.Instance.IsMining = false;
    }

    private IEnumerator Extracting()
    {
        while (true)
        {
            yield return new WaitForSeconds(1 / Extraction.ExtractionSpeed);
            if (PlayerMovement.Instance.IsMoving)
            {
                PlayerMovement.Instance.IsMining = false;
                continue;
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
        Storage.Instance.AddToStorage(Extraction.CopperExtractAmount, _resource._resourceType);
    }

    protected virtual void ResourceNotification()
    {
        NotificationHandler.Instance.ShowNotification(_resource._resourceType, true);
    }

    protected virtual void DebugResourceAmount()
    {
        Debug.Log($"Current copper amount - {Storage.Instance.CheckResourceAmount(_resource._resourceType)}");
    }
}
