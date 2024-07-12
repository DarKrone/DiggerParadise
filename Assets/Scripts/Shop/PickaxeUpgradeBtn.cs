using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PickaxeUpgradeBtn : MonoBehaviour
{
    [SerializeField] private ResourceType _resourceCostType;
    [SerializeField] private float _resourceCost;
    [SerializeField] private ResourceType _upgradeType;
    [SerializeField] private bool _upgradingAmount;
    [SerializeField] private float _upgradeAmountValue;
    [SerializeField] private bool _upgradingSpeed;
    [SerializeField] private float _upgradeSpeedValue;

    [Header("Images and text for button")]
    [SerializeField] private Image _resourceCostImageContainer;
    [SerializeField] private TextMeshProUGUI _resourceCostTextContainer;
    [SerializeField] private Image _resourceUpgradeImageContainer;
    [SerializeField] private TextMeshProUGUI _resourceUpgradeTextContainer;
    [SerializeField] private Image _upgradeTypeImageContainer;
    [SerializeField] private Sprite _upgradeTypeImage;

    private void Start()
    {
        _resourceCostImageContainer.sprite = ResourceManager.Instance.GetResourceSpriteByType(_resourceCostType);
        _resourceCostTextContainer.text = $"-{_resourceCost}";
        _resourceCostTextContainer.color = ResourceManager.Instance.GetResourceColorByType(_resourceCostType);
        _resourceUpgradeImageContainer.sprite = ResourceManager.Instance.GetOreSpriteByType(_upgradeType);
        _resourceUpgradeTextContainer.text = $"+{_upgradeAmountValue}";
        _resourceUpgradeTextContainer.color = ResourceManager.Instance.GetResourceColorByType(_upgradeType);
        _upgradeTypeImageContainer.GetComponent<Image>().sprite = _upgradeTypeImage;
    }

    private void OnEnable()
    {
        this.gameObject.GetComponent<Button>().onClick.AddListener(() => UpdatePickaxe());
    }

    private void UpdatePickaxe()
    {
        float resourceCostAmount = ResourceManager.Instance.CheckResourceAmount(_resourceCostType);

        if (resourceCostAmount < _resourceCost)
        {
            return;
        }

        ResourceManager.Instance.RemoveFromStorage(_resourceCost, _resourceCostType);

        if (_upgradingAmount)
            ResourceManager.Instance.UpgradeExtractionAmountByType(_upgradeAmountValue, _upgradeType);

        if (_upgradingSpeed)
            ResourceManager.Instance.UpgradeExtractionSpeedByType(_upgradeSpeedValue, _upgradeType);

        EventSystem.current.currentSelectedGameObject.GetComponent<Button>().interactable = false;
    }

    private void OnDisable()
    {
        this.gameObject.GetComponent<Button>().onClick.RemoveAllListeners();
    }
}
