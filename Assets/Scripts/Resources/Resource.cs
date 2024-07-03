using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : MonoBehaviour
{
    public ResourceType ResourceType;
    public float ResourceAmount;
    public bool isFullyExtracted = false;

    private void Update()
    {
        if (ResourceAmount <= 0 && !isFullyExtracted)
        {
            isFullyExtracted = true;
            Debug.Log($"Resource {gameObject.name} has been full extracted");
        }
    }
}

