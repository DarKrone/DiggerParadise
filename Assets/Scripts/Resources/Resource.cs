using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Базовый класс ресурсов от которых все наследуются и переопределяют метод ExtractResource
public class Resource : MonoBehaviour
{
    [SerializeField] protected bool _debugMode;
    [SerializeField] private ResourceType _resourceType;
    private Coroutine _extractionCoroutine;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player"))
            return;
        if (_debugMode)
            Debug.Log($"Enter {gameObject.name} resource");
        _extractionCoroutine = StartCoroutine(Extracting());
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player"))
            return;
        if (_debugMode)
            Debug.Log($"Exit {gameObject.name} resource");
        StopCoroutine(_extractionCoroutine);
        PlayerMovement.Instance.IsMining = false;
    }

    public IEnumerator Extracting()
    {
        while (true)
        {
            yield return new WaitForSeconds(1/Extraction.ExtractionSpeed);
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
        Storage.Instance.AddToStorage(Extraction.CopperExtractAmount, _resourceType);
    }

    protected virtual void ResourceNotification()
    {
        NotificationHandler.Instance.ShowNotification(_resourceType, true);
    }

    protected virtual void DebugResourceAmount()
    {
        Debug.Log($"Current copper amount - {Storage.Instance.CheckResourceAmount(_resourceType)}");
    }

}

