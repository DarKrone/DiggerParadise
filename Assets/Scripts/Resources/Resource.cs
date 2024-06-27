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
    }

    private IEnumerator Extracting()
    {
        while (true)
        {
            yield return new WaitForSeconds(1/Extraction.ExtractionSpeed);
            if (PlayerMovement.IsMoving)
                continue;
            ExtractResource();
            ResourceNotification();
            GameManager.Instance.UpdateUI();
            if (_debugMode)
                DebugResourceAmount();
        }
    }

    // Методы ниже следует переопределять в наследуемом классе нового ресурса

    /// <summary>
    /// Вывод в консоль сообщения о текущем количестве ресурса на складе
    /// </summary>
    protected virtual void DebugResourceAmount()
    {
        Debug.Log($"Current copper amount - {Storage.Instance.CheckResourceAmount(_resourceType)}");
    }

    /// <summary>
    /// Добавление ресурса на склад со скоростью добычи
    /// </summary>
    protected virtual void ExtractResource()
    {
        Storage.Instance.AddToStorage(Extraction.CopperExtractAmount, _resourceType);
    }

    /// <summary>
    /// Вывод оповещения о добыче в виде +1 над игроком
    /// </summary>
    protected virtual void ResourceNotification()
    {
        NotificationHandler.Instance.ShowNotification(_resourceType);
    }
}

