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
        GameManager.Instance.UpdateUI();
    }
    public void UpgradeSpeed()
    {
        ResourceAmount -= UpgradeSpeedCost;
        ExtractionSpeed += 1;
        UpgradeSpeedCost = Mathf.FloorToInt(UpgradeSpeedCost * SpeedCostModify);
        GameManager.Instance.UpdateUI();
    }
}
public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance;
    [SerializeField] public List<Resource> Resources;
    [SerializeField] private float _upgradeSpeedForAds = 2f;
    private float[] _modifiers;
    private Coroutine _upgCoroutine;

    private void Awake()
    {
        Instance = this;
        _modifiers = new float[Resources.Count];
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
            _modifiers[counter] = modifier;
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
            resource.ExtractionSpeed -= _modifiers[counter];
            counter++;
        }
        _modifiers = new float[Resources.Count];
    }

    public void SetExtractSpeedModifiersFromADSForSave()
    {
        int counter = 0;
        foreach (Resource resource in Resources)
        {
            resource.ExtractionSpeed -= _modifiers[counter];
            counter++;
        }
    }

    public void ReturnExtractSpeedModifiersFromADSBack()
    {
        int counter = 0;
        foreach (Resource resource in Resources)
        {
            resource.ExtractionSpeed += _modifiers[counter];
            counter++;
        }
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

    public bool CheckIfNewResource(Resource resource)
    {
        if (resource.ResourceAmount == 0)
            return true;
        return false;
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
