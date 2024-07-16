using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Это склад всех ресурсов, сюда добавлять и отсюда брать информацию
/// </summary>
[Serializable]
public class ResourceParams
{
    public float ResourceAmount;
    public float ExtractionSpeed;
    public float ExtractionAmount;
    public ResourceParams(Resource Resource)
    {
        ResourceAmount = Resource.ResourceAmount;
        ExtractionSpeed = Resource.ExtractionSpeed; 
        ExtractionAmount = Resource.ExtractionAmount;
    }
}
[Serializable]
public class Resource
{
    public ResourceType ResourceType;
    public Color ResourceColor;
    public Sprite ResourceMiniSprite;
    public Sprite ResourceOreSprite;
    public float ResourceAmount;
    public float ExtractionSpeed;
    public float ExtractionAmount;
    public bool IsAvailable;

    public void SetParams(ResourceParams Params)
    {
        ResourceAmount = Params.ResourceAmount;
        ExtractionSpeed = Params.ExtractionSpeed;   
        ExtractionAmount = Params.ExtractionAmount;
    }
}
public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance;

    [SerializeField] public List<Resource> Resources;

    [SerializeField] public List<Color> Colors;
    private void Awake()
    {
        Instance = this;
    }
    public List<ResourceParams> GetParams()
    {
        List<ResourceParams> resourceParams = new List<ResourceParams>();

        for (int i = 0; i < Resources.Count; i++)
        {
            resourceParams.Add(new ResourceParams(Resources[i]));
        }
        return resourceParams;
    }
    public void SetParams(List<ResourceParams> Params)
    {
        if (Params.Count != Resources.Count)
            return;
        for (int i = 0; i < Resources.Count; i++)
        {
            Resources[i].SetParams(Params[i]);
        }
    }
    public void AddToStorage(float amount, ResourceType resourceType)
    {
        foreach (Resource resource in Resources)
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
        foreach (Resource resource in Resources)
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
        foreach (Resource resource in Resources)
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
        foreach (Resource resource in Resources)
        {
            if (resource.ResourceType == type)
            {
                return resource.ResourceColor;
            }
        }
        return Color.white;
    }

    public Sprite GetResourceSpriteByType(ResourceType type)
    {
        foreach (Resource resource in Resources)
        {
            if (resource.ResourceType == type)
            {
                return resource.ResourceMiniSprite;
            }
        }
        return null;
    }

    public Sprite GetOreSpriteByType(ResourceType type)
    {
        foreach (Resource resource in Resources)
        {
            if (resource.ResourceType == type)
            {
                return resource.ResourceOreSprite;
            }
        }
        return null;
    }

    public float GetExtractionSpeedByType(ResourceType resourceType)
    {
        foreach (Resource resource in Resources)
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
        foreach (Resource resource in Resources)
        {
            if (resource.ResourceType == resourceType)
            {
                return resource.ExtractionAmount;
            }
        }
        return -1;
    }

    public void UpgradeExtractionAmountByType(float upgradeAmount, ResourceType resourceType)
    {
        foreach (Resource resource in Resources)
        {
            if (resource.ResourceType == resourceType)
            {
                resource.ExtractionAmount += upgradeAmount;
                return;
            }
        }
    }

    public void UpgradeExtractionSpeedByType(float upgradeAmount, ResourceType resourceType)
    {
        foreach (Resource resource in Resources)
        {
            if (resource.ResourceType == resourceType)
            {
                resource.ExtractionSpeed += upgradeAmount;
                return;
            }
        }
    }
}
