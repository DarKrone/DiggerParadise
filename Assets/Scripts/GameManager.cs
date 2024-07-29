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
    [SerializeField] private AllResourcesOnMap _allResourcesOnMap;
    [SerializeField] private List<ResourceTaker> _bridges;
    [SerializeField] private List<ResourceTaker> _artifacts;
    
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

        StartCoroutine(AutoSave());
    }

    private IEnumerator AutoSave()
    {
        while (true)
        {
            yield return new WaitForSeconds(300f);
            SaveData();
        }
    }

    public void SaveData()
    {
        ResourceManager.Instance.SetExtractSpeedModifiersFromADSForSave();
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
        List<List<NeededResource>> bridgesNeededResources = new List<List<NeededResource>>();
        foreach (var neededResourceOnTaker in _bridges)
        {
            bridgesNeededResources.Add(neededResourceOnTaker.NeededResources);
        }
        List<List<NeededResource>> artifactsNeededResources = new List<List<NeededResource>>();
        foreach (var neededResourceOnTaker in _artifacts)
        {
            artifactsNeededResources.Add(neededResourceOnTaker.NeededResources);
        }
        SaveLoad.currentData = new GameData();
        SaveLoad.currentData.GetPos(_player.transform.position);
        SaveLoad.currentData.ResourceParams = ResourceManager.Instance.GetParams();

        SaveLoad.currentData.NeededResources = neededResources;
        SaveLoad.currentData.BridgesNeededResources = bridgesNeededResources;
        SaveLoad.currentData.ArtifactsNeededResources = artifactsNeededResources;

        SaveLoad.currentData.VersionForDeleteData = Application.version;
        SaveLoad.currentData.UpgradeMinisTiers = minisTiers;
        SaveLoad.currentData.AllResourcesAmounts = _allResourcesOnMap.GetResourcesAmounts();
        SaveLoad.SaveGame();
        UpdateLeaderboardScores();
        ResourceManager.Instance.ReturnExtractSpeedModifiersFromADSBack();

    }

    private void UpdateLeaderboardScores()
    {
        Resource resource = ResourceManager.Instance.GetResourceByType(ResourceType.Copper);
        YandexGame.NewLeaderboardScores("CopperLeaderboard", (int)resource.ResourceAmount);

        resource = ResourceManager.Instance.GetResourceByType(ResourceType.Iron);
        YandexGame.NewLeaderboardScores("IronLeaderboard", (int)resource.ResourceAmount);

        resource = ResourceManager.Instance.GetResourceByType(ResourceType.Gold);
        YandexGame.NewLeaderboardScores("GoldLeaderboard", (int)resource.ResourceAmount);

        resource = ResourceManager.Instance.GetResourceByType(ResourceType.Ametist);
        YandexGame.NewLeaderboardScores("AmethystLeaderboard", (int)resource.ResourceAmount);

        resource = ResourceManager.Instance.GetResourceByType(ResourceType.Saphir);
        YandexGame.NewLeaderboardScores("SapphirLeaderboard", (int)resource.ResourceAmount);

        resource = ResourceManager.Instance.GetResourceByType(ResourceType.Topaz);
        YandexGame.NewLeaderboardScores("TopazLeaderboard", (int)resource.ResourceAmount);

        resource = ResourceManager.Instance.GetResourceByType(ResourceType.Emerald);
        YandexGame.NewLeaderboardScores("EmeraldLeaderboard", (int)resource.ResourceAmount);

        resource = ResourceManager.Instance.GetResourceByType(ResourceType.Diamond);
        YandexGame.NewLeaderboardScores("DiamondLeaderboard", (int)resource.ResourceAmount);
    }

    private void LoadData()
    {
        if (YandexGame.savesData.isFirstSession)
        {
            YandexGame.ResetSaveProgress();
            SaveLoad.SaveGame();
            return;
        }

        SaveLoad.LoadGame();

        if(SaveLoad.currentData.VersionForDeleteData != Application.version)
        {
            YandexGame.ResetSaveProgress();
            SaveData();
            return;
        }
        _player.transform.position = SaveLoad.currentData.GetVector3();
        ResourceManager.Instance.SetParams(SaveLoad.currentData.ResourceParams);
        _allResourcesOnMap.SetResourceAmounts(SaveLoad.currentData.AllResourcesAmounts);

        for (int i = 0; i < SaveLoad.currentData.NeededResources.Count; i++)
        {
            ResourceTakers[i].NeededResources = SaveLoad.currentData.NeededResources[i];
            if (SaveLoad.currentData.UpgradeMinisTiers[i] > 0)
            {
                _upgradeMinisShops[i].UpgradeMinisAfterLoadSave(SaveLoad.currentData.UpgradeMinisTiers[i]);
            }
        }
        for (int i = 0; i < SaveLoad.currentData.BridgesNeededResources.Count; i++)
        {
            _bridges[i].NeededResources = SaveLoad.currentData.BridgesNeededResources[i];
        }
        for (int i = 0; i < SaveLoad.currentData.BridgesNeededResources.Count; i++)
        {
            _artifacts[i].NeededResources = SaveLoad.currentData.ArtifactsNeededResources[i];
        }
        UpdateResourcesList();
        UpdateUI();
    }

    public void ResetData()
    {
        SaveLoad.currentData = DefaultDataStorage.Instance.GetDefaultGameData;
        SaveLoad.SaveGame();
        SaveLoad.LoadGame();
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
            resourceInfo.ResourceAmountText.text = ResourceManager.Instance.GetResourceByType(resourceInfo.ResourceType).ResourceAmount.ToString();
        }
    }
}
