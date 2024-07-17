using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PickaxeUpgradeBtn : MonoBehaviour
{
    //[SerializeField] private ResourceType _resourceCostType;
    //[SerializeField] private float _resourceCost;
    //[SerializeField] private float _upgradeAmountValue;
    //[SerializeField] private float _upgradeSpeedValue;
    [SerializeField] private bool _upgradingAmount;
    [SerializeField] private bool _upgradingSpeed;
    [SerializeField] private ResourceType _upgradeType;

    [Header("Images and text for button")]
    [SerializeField] private Image _resourceCostImageContainer;
    [SerializeField] private TextMeshProUGUI _resourceCostTextContainer;
    [SerializeField] private Image _resourceUpgradeImageContainer;
    [SerializeField] private TextMeshProUGUI _resourceUpgradeTextContainer;
    [SerializeField] private Image _upgradeTypeImageContainer;
    [SerializeField] private Sprite _upgradeTypeImage;

    private void Start()
    {
        UpdateUI();
    }
    private void UpdateUI()
    {
        Resource resource = ResourceManager.Instance.GetResourceByType(_upgradeType);
        _resourceCostImageContainer.sprite = resource.ResourceMiniSprite;
        _resourceCostTextContainer.color = resource.ResourceColor;
        _resourceUpgradeImageContainer.sprite = resource.ResourceOreSprite;
        _resourceUpgradeTextContainer.color = resource.ResourceColor;
        _upgradeTypeImageContainer.GetComponent<Image>().sprite = resource.ResourceOreSprite;
        if(_upgradingAmount)
        {
            _resourceCostTextContainer.text = $"-{resource.UpgradeAmountCost}";
            _resourceUpgradeTextContainer.text = $"+{resource.NextTierAmount}";
        }
        if (_upgradingSpeed)
        {
            _resourceCostTextContainer.text = $"-{resource.UpgradeSpeedCost}";
            _resourceUpgradeTextContainer.text = $"+{resource.NextTierSpeed}";
        }
    }

    private void UpdatePickaxe()
    {
        if (_upgradingAmount)
            ResourceManager.Instance.UpgradeExtractionAmountByType(_upgradeType);

        if (_upgradingSpeed)
            ResourceManager.Instance.UpgradeExtractionSpeedByType(_upgradeType);

        UpdateUI();
    }
    private void OnEnable()
    {
        UpdateUI();
        this.gameObject.GetComponent<Button>().onClick.AddListener(() => UpdatePickaxe());
    }
    private void OnDisable()
    {
        this.gameObject.GetComponent<Button>().onClick.RemoveAllListeners();
    }
}
