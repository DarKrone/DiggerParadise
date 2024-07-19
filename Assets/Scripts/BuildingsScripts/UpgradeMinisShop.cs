using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[Serializable]

public class UpgradeMinisShop : UpgradeUI
{
    [SerializeField] private GameObject _minis;
    [SerializeField] private GameObject _building;

    [Header("Настройки апгрейда")]
    [SerializeField] private int _currentUpgradeTier = 0;
    [SerializeField] private ResourceType _resourceToUpgrade;
    [SerializeField] private float _currentUpgradeCost = 10;
    [SerializeField] private float _upgradeCostStep = 50;

    [Header("Ссылки на UI")]
    [SerializeField] private TextMeshProUGUI _currentTierText;
    [SerializeField] private TextMeshProUGUI _costText;
    [SerializeField] private TextMeshProUGUI _speedText;
    [SerializeField] private TextMeshProUGUI _amountText;
    [SerializeField] private Image _resourceImage;

    public int CurrentUpdateTier { get { return _currentUpgradeTier; } }

    private void Start()
    {
        UpdateUpgradeInfo();
    }

    private void UpdateUpgradeInfo()
    {
        _resourceImage.sprite = ResourceManager.Instance.GetResourceByType(_resourceToUpgrade).ResourceMiniSprite;
        _costText.text = _currentUpgradeCost.ToString();
        _costText.color = ResourceManager.Instance.GetResourceByType(_resourceToUpgrade).ResourceColor;
        _currentTierText.text =  _currentUpgradeTier.ToString();
        _speedText.text = _minis.GetComponent<MinerAI>().ExtractionSpeed.ToString();
        _amountText.text = _minis.GetComponent<MinerAI>().ExtractionAmount.ToString();
    }
    public void UpgradeMinis()
    {
        if (ResourceManager.Instance.GetResourceByType(_resourceToUpgrade).ResourceAmount < _currentUpgradeCost)
            return;
        _building.GetComponent<BuildingSpriteHandler>().UpdateBuildingSpriteToNext();
        if (_currentUpgradeTier %2 == 0)
        {
            _minis.GetComponent<MinerAI>().ExtractionSpeed += 1;
        }
        else
        {
            _minis.GetComponent<MinerAI>().ExtractionAmount += 1;
        }
        ResourceManager.Instance.GetResourceByType(_resourceToUpgrade).ResourceAmount -= _currentUpgradeCost;
        GameManager.Instance.UpdateUI();
        _currentUpgradeTier += 1;
        _currentUpgradeCost += _upgradeCostStep * _currentUpgradeTier;
        UpdateUpgradeInfo();
    }
    public void UpgradeMinisAfterConstructionCompleted()
    {
        _currentUpgradeTier += 1;
        UpdateUpgradeInfo();
    }
    public void UpgradeMinisAfterLoadSave(int N)
    {
        for(int i = 0; i< N-1;i++)
        {
            _building.GetComponent<BuildingSpriteHandler>().UpdateBuildingSpriteToNext();
            if (_currentUpgradeTier % 2 == 0)
            {
                _minis.GetComponent<MinerAI>().ExtractionSpeed += 1;
            }
            else
            {
                _minis.GetComponent<MinerAI>().ExtractionAmount += 1;
            }
            _currentUpgradeTier += 1;
            _currentUpgradeCost += _upgradeCostStep * _currentUpgradeTier;
            UpdateUpgradeInfo();
        }
    }
}
