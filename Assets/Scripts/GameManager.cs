using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using YG;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [SerializeField] private List<GameObject> _resourcesList;
    [SerializeField] private GameObject _resourceInfoPrefab;
    [SerializeField] private GameObject _parentToSpawnResourceUI;
    [SerializeField] private GameObject _player;

    [Header ("Save params")]
    [SerializeField] private ResourceManager _resourceManager;
    [SerializeField] private List<UpgradeMinisShop> _upgradeMinisShops;
    [SerializeField] private List<ResourceTaker> ResourceTakers;

    private void OnEnable()
    {
        YandexGame.GetDataEvent += LoadData;
    }

    private void OnDisable()
    {
        YandexGame.GetDataEvent -= LoadData;
    }

    private void Awake()
    {
        Instance = this;

        if(YandexGame.SDKEnabled)
        {
            LoadData();
        }

        UpdateResourcesList();
        UpdateUI();
    }
    public void SaveData()
    {
        ResourceManager.Instance.StopRewardedAdsUpgrade();
        List<int> minisTiers = new List<int>();
        foreach(var el in _upgradeMinisShops)
        {
            minisTiers.Add(el.CurrentUpdateTier);
        }
        List<List<NeededResource>> neededResources = new List<List<NeededResource>>();
        foreach(var neededResourceOnTaker in ResourceTakers)
        {
            neededResources.Add(neededResourceOnTaker.NeededResources);
        }
        SaveLoad.currentData = new GameData();
        SaveLoad.currentData.GetPos(_player.transform.position);
        SaveLoad.currentData.ResourceParams = ResourceManager.Instance.GetParams();
        SaveLoad.currentData.NeededResources = neededResources;
        SaveLoad.currentData.UpgradeMinisTiers = minisTiers;

        SaveLoad.SaveGame();
    }
    private void LoadData()
    {
        SaveLoad.LoadGame();
        _player.transform.position = SaveLoad.currentData.GetVector3();
        ResourceManager.Instance.SetParams(SaveLoad.currentData.ResourceParams);

        for (int i = 0; i < ResourceTakers.Count; i++)
        {
            ResourceTakers[i].NeededResources = SaveLoad.currentData.NeededResources[i];
            if (SaveLoad.currentData.UpgradeMinisTiers[i] > 0)
            {
                _upgradeMinisShops[i].UpgradeMinisAfterLoadSave(SaveLoad.currentData.UpgradeMinisTiers[i]);
            }
        }
        UpdateResourcesList();
        UpdateUI();
    }

    public void UpdateResourcesList()
    {
        ClearAllResourcesUI();
        _resourcesList.Clear();
        foreach (Resource resource in ResourceManager.Instance.Resources)
        {
            if (resource.IsAvailable)
            {
                GameObject resourceUI = Instantiate(_resourceInfoPrefab, Vector3.zero, _resourceInfoPrefab.transform.rotation, _parentToSpawnResourceUI.transform);
                _resourcesList.Add(resourceUI);
                ResourceInfoUI resourceInfo = resourceUI.GetComponent<ResourceInfoUI>();
                resourceInfo.ResourceImage.sprite = resource.ResourceMiniSprite;
                resourceInfo.ResourceAmountText.text = resource.ResourceAmount.ToString();
                resourceInfo.ResourceAmountText.color = resource.ResourceColor;
                resourceInfo.ResourceType = resource.ResourceType;
            }
        }
    }
    private void ClearAllResourcesUI()
    {
        while (_parentToSpawnResourceUI.transform.childCount > 0)
        {
            DestroyImmediate(_parentToSpawnResourceUI.transform.GetChild(0).gameObject);
        }
    }
    
    public void UpdateUI()
    {
        foreach(GameObject resource in _resourcesList)
        {
            ResourceInfoUI resourceInfo = resource.GetComponent<ResourceInfoUI>();
            resourceInfo.ResourceAmountText.text = ResourceManager.Instance.CheckResourceAmount(resourceInfo.ResourceType).ToString();
        }
    }
}
