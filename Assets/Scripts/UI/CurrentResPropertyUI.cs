using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CurrentResPropertyUI : MonoBehaviour
{
    [SerializeField] private ResourceType _resourceType;
    [SerializeField] private Image _resourceImage;
    [SerializeField] private TextMeshProUGUI _amountText;
    [SerializeField] private TextMeshProUGUI _speedText;

    private void OnEnable()
    {
        UpdateProperties();
    }

    public void UpdateProperties()
    {
        Resource resource = ResourceManager.Instance.GetResourceByType(_resourceType);
        _resourceImage.sprite = resource.ResourceMiniSprite;
        _amountText.text = resource.ExtractionAmount.ToString();
        _speedText.text = resource.ExtractionSpeed.ToString();
    }
}
