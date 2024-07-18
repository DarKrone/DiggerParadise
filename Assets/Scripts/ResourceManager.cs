using System;
using System.Collections;
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

    public float UpgradeAmountCost;
    public float UpgradeSpeedCost;
    public ResourceParams(Resource Resource)
    {
        ResourceAmount = Resource.ResourceAmount;
        ExtractionSpeed = Resource.ExtractionSpeed; 
        ExtractionAmount = Resource.ExtractionAmount;
        UpgradeAmountCost = Resource.UpgradeAmountCost;
        UpgradeSpeedCost = Resource.UpgradeSpeedCost;
    }
}
[Serializable]
public class Resource
{
    public ResourceType ResourceType;
    public Color ResourceColor;
    public Sprite ResourceMiniSprite;
    public Sprite ResourceOreSprite;
    public float UpgradeAmountCost;
    public float UpgradeSpeedCost;
    public float ResourceAmount;
    public float ExtractionSpeed;
    public float ExtractionAmount;
    public float AmountTierModify;
    public float SpeedTierModify;
    public bool IsAvailable { get { return ResourceAmount > 0; } }
    public float NextTierAmount { get { return ExtractionAmount * AmountTierModify; } }
    public float NextTierSpeed { get { return ExtractionSpeed * SpeedTierModify; } }
    public void SetParams(ResourceParams Params)
    {
        ResourceAmount = Params.ResourceAmount;
        ExtractionSpeed = Params.ExtractionSpeed;   
        ExtractionAmount = Params.ExtractionAmount;
        UpgradeAmountCost = Params.UpgradeAmountCost;
        UpgradeSpeedCost = Params.UpgradeSpeedCost;
    }

    public void UpgradeAmount()
    {
        ResourceAmount -= UpgradeAmountCost;
        ExtractionAmount *= AmountTierModify;
        UpgradeAmountCost *= AmountTierModify;
    }
    public void UpgradeSpeed()
    {
        ResourceAmount -= UpgradeSpeedCost;
        ExtractionSpeed *= SpeedTierModify;
        UpgradeAmountCost *= SpeedTierModify;
    }
}
public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance;
    [SerializeField] public List<Resource> Resources;
    [SerializeField] private float _upgradeSpeedForAds = 2f;
    private float[] modifiers;
    private Coroutine _upgCoroutine;

    private void Awake()
    {
        Instance = this;
        modifiers = new float[Resources.Count];
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

    public void RewardedAdsUpgradeSpeedForPeriod(float duration)
    {
        int counter = 0;
        foreach (Resource resource in Resources)
        {
            float modifier = Mathf.Log(resource.ExtractionSpeed) + _upgradeSpeedForAds;
            modifier = Mathf.Floor(modifier);
            modifiers[counter] = modifier;
            counter++;
            resource.ExtractionSpeed += modifier;
        }
        _upgCoroutine = StartCoroutine(StartRewardAdsUpgradeTimer(duration));
    }

    private IEnumerator StartRewardAdsUpgradeTimer(float duration)
    {
        yield return new WaitForSeconds(duration);
        StopRewardedAdsUpgrade();
    }

    public void StopRewardedAdsUpgrade()
    {
        if (_upgCoroutine != null)
        {
            StopCoroutine(_upgCoroutine);
        }
        int counter = 0;
        foreach (Resource resource in Resources)
        {
            resource.ExtractionSpeed -= modifiers[counter];
            counter++;
        }
    }

    public void AddToStorage(float amount, ResourceType resourceType)
    {
        foreach (Resource resource in Resources)
        {
            if (resource.ResourceType == resourceType)
            {
                if (resource.ResourceAmount == 0)
                {
                    resource.ResourceAmount += amount;
                    GameManager.Instance.UpdateResourcesList();
                    break;
                }
                resource.ResourceAmount += amount;
                break;
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
    public Color GetResourceColorByType(ResourceType resourceType)
    {
        foreach (Resource resource in Resources)
        {
            if (resource.ResourceType == resourceType)
            {
                return resource.ResourceColor;
            }
        }
        return Color.white;
    }
    public Sprite GetResourceSpriteByType(ResourceType resourceType)
    {
        foreach (Resource resource in Resources)
        {
            if (resource.ResourceType == resourceType)
            {
                return resource.ResourceMiniSprite;
            }
        }
        return null;
    }
    public Sprite GetOreSpriteByType(ResourceType resourceType)
    {
        foreach (Resource resource in Resources)
        {
            if (resource.ResourceType == resourceType)
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
    public float GetAmountCost(ResourceType resourceType)
    {
        foreach (Resource resource in Resources)
        {
            if (resource.ResourceType == resourceType)
            {
                return resource.UpgradeAmountCost;
            }
        }
        return -1;
    }
    public float GetSpeedCost(ResourceType resourceType)
    {
        foreach (Resource resource in Resources)
        {
            if (resource.ResourceType == resourceType)
            {
                return resource.UpgradeSpeedCost;
            }
        }
        return -1;
    }
    public Resource GetResourceByType(ResourceType resourceType)
    {
        foreach (Resource resource in Resources)
        {
            if (resource.ResourceType == resourceType)
            {
                return resource;
            }
        }
        return null;
    }
    public void UpgradeExtractionAmountByType(ResourceType resourceType)
    {
        Resource resource = GetResourceByType(resourceType);
        if (resource.ResourceAmount < resource.UpgradeAmountCost)
            return;
        resource.UpgradeAmount();

    }
    public void UpgradeExtractionSpeedByType(ResourceType resourceType)
    {
        Resource resource = GetResourceByType(resourceType);
        if (resource.ResourceAmount < resource.UpgradeSpeedCost)
            return;
        resource.UpgradeSpeed();
    }
}
