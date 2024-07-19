using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// ��� ����� ���� ��������, ���� ��������� � ������ ����� ����������
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

    public ResourceParams()
    {
        ResourceAmount = 0;
        ExtractionSpeed = 1;
        ExtractionAmount = 1;
        UpgradeAmountCost = 1;
        UpgradeSpeedCost = 1;
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
    public float AmountCostModify;
    public float SpeedCostModify;
    public float[] Parameters { get { return new float[] { ResourceAmount, ExtractionSpeed, ExtractionAmount, UpgradeAmountCost, UpgradeSpeedCost }; } }
    public bool IsAvailable { get { return ResourceAmount > 0; } }
    public float NextTierAmount { get { return ExtractionAmount +1; } }
    public float NextTierSpeed { get { return ExtractionSpeed +1; } }
    public void SetParams(ResourceParams parameters)
    {
        ResourceAmount = parameters.ResourceAmount;
        ExtractionSpeed = parameters.ExtractionSpeed;   
        ExtractionAmount = parameters.ExtractionAmount;
        UpgradeAmountCost = parameters.UpgradeAmountCost;
        UpgradeSpeedCost = parameters.UpgradeSpeedCost;
    }

    public void UpgradeAmount()
    {
        ResourceAmount -= UpgradeAmountCost;
        ExtractionAmount += 1;
        UpgradeAmountCost = Mathf.FloorToInt(UpgradeAmountCost * AmountCostModify);
    }
    public void UpgradeSpeed()
    {
        ResourceAmount -= UpgradeSpeedCost;
        ExtractionSpeed += 1;
        UpgradeSpeedCost = Mathf.FloorToInt(UpgradeSpeedCost * SpeedCostModify);
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
    public void SetParams(List<ResourceParams> parameters)
    {
        if (parameters.Count != Resources.Count)
            return;
        for (int i = 0; i < Resources.Count; i++)
        {
            Resources[i].SetParams(parameters[i]);
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
