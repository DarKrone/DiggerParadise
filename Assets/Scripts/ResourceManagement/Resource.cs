using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ResourceType
{
    Copper,
    Iron
}
// Базовый класс ресурсов от которых все наследуются и переопределяют метод ExtractResource
public class Resource : MonoBehaviour
{
    [SerializeField] protected bool _debugMode;
    private Coroutine _extractionCoroutine;

    //public void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (!collision.CompareTag("Player"))
    //        return;
    //    if (_debugMode)
    //        Debug.Log($"Enter {gameObject.name} resource");
    //    _extractionCoroutine = StartCoroutine(Extracting());
    //}
    //public void OnTriggerExit2D(Collider2D collision)
    //{
    //    if (!collision.CompareTag("Player"))
    //        return;
    //    if (_debugMode)
    //        Debug.Log($"Exit {gameObject.name} resource");
    //    StopCoroutine(_extractionCoroutine);
    //}
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;
        if (_debugMode)
            Debug.Log($"Enter {gameObject.name} resource");
        _extractionCoroutine = StartCoroutine(Extracting());
    }
    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player"))
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
            ExtractResource();
            GameManager.Instance.UpdateUI();
            if (_debugMode)
                DebugResourceAmount();
        }
    }

    // Методы ниже следует переопределять в наследуемом классе нового ресурса

    /// <summary>
    /// Вывод в консоль сообщения о текущем количестве ресурса на складе
    /// </summary>
    virtual protected void DebugResourceAmount()
    {
        return;
    }

    /// <summary>
    /// Добавление ресурса на склад со скоростью добычи
    /// </summary>
    virtual protected void ExtractResource()
    {
        return;
    }
}

