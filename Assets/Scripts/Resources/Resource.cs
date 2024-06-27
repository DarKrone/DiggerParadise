using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ������� ����� �������� �� ������� ��� ����������� � �������������� ����� ExtractResource
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

    // ������ ���� ������� �������������� � ����������� ������ ������ �������

    /// <summary>
    /// ����� � ������� ��������� � ������� ���������� ������� �� ������
    /// </summary>
    protected virtual void DebugResourceAmount()
    {
        Debug.Log($"Current copper amount - {Storage.Instance.CheckResourceAmount(_resourceType)}");
    }

    /// <summary>
    /// ���������� ������� �� ����� �� ��������� ������
    /// </summary>
    protected virtual void ExtractResource()
    {
        Storage.Instance.AddToStorage(Extraction.CopperExtractAmount, _resourceType);
    }

    /// <summary>
    /// ����� ���������� � ������ � ���� +1 ��� �������
    /// </summary>
    protected virtual void ResourceNotification()
    {
        NotificationHandler.Instance.ShowNotification(_resourceType);
    }
}

