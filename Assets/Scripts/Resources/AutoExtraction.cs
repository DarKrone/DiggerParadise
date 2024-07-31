using System.Collections;
using UnityEngine;

public class AutoExtraction : MonoBehaviour
{
    [Header ("Параметры")]
    [SerializeField] private float _extractionSpeed;
    [SerializeField] private float _extractionAmount;
    [SerializeField] private ResourceType _resourceType;

    private void Start()
    {
        StartCoroutine(Extracting());
    }

    IEnumerator Extracting()
    {
        while(true)
        {
            yield return new WaitForSeconds(1f / _extractionSpeed);
            Resource resource = ResourceManager.Instance.GetResourceByType(_resourceType);
            resource.ResourceAmount += _extractionAmount;
        }
    }
}
