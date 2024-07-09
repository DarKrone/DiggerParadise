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
        while(true)
        {
            yield return new WaitForSeconds(1f / _extractionSpeed);
            ResourceManager.Instance.AddToStorage(_extractionAmount, _resourceType);
        }
    }
}
