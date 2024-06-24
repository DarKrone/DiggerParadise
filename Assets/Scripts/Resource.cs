using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ������� ����� �������� �� ������� ��� ����������� � �������������� ����� ExtractResource
public class Resource : MonoBehaviour
{
    [SerializeField] protected bool _debugMode;
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
            yield return new WaitForSeconds(Extraction.ExtractionSpeed);
            ExtractResource();
            GameManager.Instance.UpdateUI();
            if (_debugMode)
                DebugResourceAmount();
        }
    }

    // ������ ���� ������� �������������� � ����������� ������ ������ �������

    /// <summary>
    /// ����� � ������� ��������� � ������� ���������� ������� �� ������
    /// </summary>
    virtual protected void DebugResourceAmount()
    {
        return;
    }

    /// <summary>
    /// ���������� ������� �� ����� �� ��������� ������
    /// </summary>
    virtual protected void ExtractResource()
    {
        return;
    }
}
