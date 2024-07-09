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

    private void Awake()
    {
        Instance = this;
        UpdateResourcesList();
        UpdateUI();
    }

    public void UpdateResourcesList()
    {
        ClearAllResourcesUI();
        foreach (ResourceManager.Resource resource in ResourceManager.Instance.Resources)
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
