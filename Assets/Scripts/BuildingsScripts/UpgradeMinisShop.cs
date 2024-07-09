using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UpgradeMinisShop : UpgradeUI
{
    [SerializeField] private GameObject _minis;
    [SerializeField] private GameObject _building;

    [Header("Настройки апгрейда")]
    [SerializeField] private float _currentUpgradeTier = 0;
    [SerializeField] private ResourceType _resourceToUpgrade;
    [SerializeField] private float _currentUpgradeCost = 10;
    [SerializeField] private float _upgradeCostStep = 50;

    [Header("Ссылки на UI")]
    [SerializeField] private TextMeshProUGUI _currentTierText;
    [SerializeField] private TextMeshProUGUI _costText;
    [SerializeField] private TextMeshProUGUI _speedText;
    [SerializeField] private TextMeshProUGUI _amountText;
    [SerializeField] private Image _resourceImage;

    private void Start()
    {
        UpdateUpgradeInfo();
    }

    private void UpdateUpgradeInfo()
    {
        _resourceImage.sprite = ResourceManager.Instance.GetResourceSpriteByType(_resourceToUpgrade);
        _costText.text = _currentUpgradeCost.ToString();
        _costText.color = ResourceManager.Instance.GetResourceColorByType(_resourceToUpgrade);
        _currentTierText.text = "Tier - " + _currentUpgradeTier.ToString();
        _speedText.text = "Speed - " + _minis.GetComponent<MinerAI>().ExtractionSpeed.ToString();
        _amountText.text = "Amount - " + _minis.GetComponent<MinerAI>().ExtractionAmount.ToString();
    }

    public void UpgradeMinis()
    {
        if (ResourceManager.Instance.CheckResourceAmount(_resourceToUpgrade) < _currentUpgradeCost)
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
        ResourceManager.Instance.RemoveFromStorage(_currentUpgradeCost, _resourceToUpgrade);
        _currentUpgradeTier += 1;
        _currentUpgradeCost += _upgradeCostStep * _currentUpgradeTier;
        UpdateUpgradeInfo();
    }
}
