using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [SerializeField] private List<GameObject> _resourcesList;
    [SerializeField] private GameObject _resourceInfoPrefab;
    [SerializeField] private GameObject _parentToSpawnResourceUI;
    [SerializeField] private GameObject _player;

    private ResourceManager _resourceManager;

    private void Awake()
    {
        Instance = this;
        UpdateResourcesList();
        UpdateUI();
        SaveLoad.LoadGame();
        if(SaveLoad.Loaded)
        {
            LoadData();
        }
    }

    public void UpdateResourcesList()
    {
        ClearAllResourcesUI();
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
    private void LoadData()
    {
        _player.transform.position = SaveLoad.currentData.GetVector3();
        ResourceManager.Instance.SetParams(SaveLoad.currentData.ResourceParams);
    }
    public void SaveData()
    {
        SaveLoad.currentData.GetPos(_player.transform.position);
        SaveLoad.currentData.ResourceParams = ResourceManager.Instance.GetParams();
        SaveLoad.SaveGame();
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
