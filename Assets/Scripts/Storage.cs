using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Это склад всех ресурсов, сюда добавлять и отсюда брать информацию
/// </summary>
public class Storage : MonoBehaviour
{
    public static Storage Instance;

    [Serializable]
    private class Resource
    {
        public ResourceType ResourceType;
        public Color ResourceColor;
        public float ResourceAmount;
        public float ExtractionSpeed;
        public float ExtractionAmount;
    }

    [SerializeField] private List<Resource> _resources;

    private void Awake()
    {
        Instance = this;
    }
    public void AddToStorage(float amount, ResourceType resourceType)
    {
        foreach (Resource resource in _resources)
        {
            if (resource.ResourceType == resourceType)
            {
                resource.ResourceAmount += amount;
            }
        }
        GameManager.Instance.UpdateUI();
    }

    public void RemoveFromStorage(float amount, ResourceType resourceType)
    {
        foreach (Resource resource in _resources)
        {
            if (resource.ResourceType == resourceType)
            {
                resource.ResourceAmount -= amount;
            }
        }
        GameManager.Instance.UpdateUI();
    }

    public float CheckResourceAmount(ResourceType resourceType)
    {
        foreach (Resource resource in _resources)
        {
            if (resource.ResourceType == resourceType)
            {
                return resource.ResourceAmount;
            }
        }
        return -1;
    }

    public Color GetResourceColorByType(ResourceType type)
    {
        foreach (Resource resource in _resources)
        {
            if (resource.ResourceType == type)
            {
                return resource.ResourceColor;
            }
        }
        return Color.white;
    }

    public float GetExtractionSpeedByType(ResourceType resourceType)
    {
        foreach (Resource resource in _resources)
        {
            if (resource.ResourceType == resourceType)
            {
                return resource.ExtractionSpeed;
            }
        }
        return -1;
    }

    public float GetExtractionAmountByType(ResourceType resourceType)
    {
        foreach (Resource resource in _resources)
        {
            if (resource.ResourceType == resourceType)
            {
                return resource.ExtractionAmount;
            }
        }
        return -1;
    }
}
