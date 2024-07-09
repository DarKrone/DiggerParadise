using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UpgradeMinisShop : UpgradeUI
{
    [SerializeField] private GameObject _minis;
    [SerializeField] private GameObject _building;
    public void UpgradeMinisExtractSpeed(float upgradeAmount)
    {
        if (ResourceManager.Instance.CheckResourceAmount(ResourceType.Copper) < 10)
            return;

        ResourceManager.Instance.RemoveFromStorage(10, ResourceType.Copper);
        _minis.GetComponent<MinerAI>().ExtractionSpeed += upgradeAmount;
        _building.GetComponent<BuildingSpriteHandler>().UpdateBuildingSpriteToNext();
        DisableThisUpgradeBtn();
    }

    public void UpgradeMinisExtractAmount(float upgradeAmount)
    {
        if (ResourceManager.Instance.CheckResourceAmount(ResourceType.Iron) < 10)
            return;

        ResourceManager.Instance.RemoveFromStorage(10, ResourceType.Iron);
        _minis.GetComponent<MinerAI>().ExtractionAmount += upgradeAmount;
        _building.GetComponent<BuildingSpriteHandler>().UpdateBuildingSpriteToNext();
        DisableThisUpgradeBtn();
    }
}
