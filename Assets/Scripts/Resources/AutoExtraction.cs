using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoExtraction : MonoBehaviour
{
    [Header ("Параметры")]
    [SerializeField] private float _extractionSpeed;
    [SerializeField] private float _extractionAmount;
    [SerializeField] private ResourceType _resourceType;

    //private Coroutine _extract;
    private void Start()
    {
        StartCoroutine(Extracting());
    }
    IEnumerator Extracting()
    {
        yield return new WaitForSeconds(1 / _extractionSpeed);
        Storage.AddToStorage(_extractionAmount, _resourceType);
    }
}
