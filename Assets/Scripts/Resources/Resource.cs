using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : MonoBehaviour
{
    [SerializeField] private bool _debugMode;
    public ResourceType ResourceType;
    public float ResourceAmount;
    public bool isFullyExtracted = false;

    private void Update()
    {
        if (ResourceAmount <= 0 && !isFullyExtracted)
        {
            isFullyExtracted = true;
            //gameObject.GetComponent<BoxCollider2D>().enabled = false;
            gameObject.SetActive(false);
            if (_debugMode)
                Debug.Log($"Resource {gameObject.name} has been full extracted");
        }
    }
}

