using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    [SerializeField] GameObject _shopUI;

    private void Start()
    {
        _shopUI.SetActive(false);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
            return;

        OpenShopUI();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
            return;

        CloseShopUI();
    }

    public void OpenShopUI()
    {
        _shopUI.SetActive(true);
    }

    public void CloseShopUI()
    {
        _shopUI.SetActive(false);
    }

    public void UpgradePickaxeExtractAmount(GameObject upgradeBtn)
    {
        float ironAmount = ResourceManager.Instance.CheckResourceAmount(ResourceType.Iron);

        if (ironAmount < 5)
        {
            return;
        }
        ResourceManager.Instance.RemoveFromStorage(5, ResourceType.Iron);
        ResourceManager.Instance.UpgradeExtractionAmountByType(1, ResourceType.Copper);
        DisableThisUpgradeButton(upgradeBtn);
    }

    private void DisableThisUpgradeButton(GameObject upgradeBtn)
    {
        upgradeBtn.GetComponent<Button>().interactable = false;
    }
}
