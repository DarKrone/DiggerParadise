using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : UpgradeUI
{
    public void UpgradePickaxeExtractAmount()
    {
        float ironAmount = ResourceManager.Instance.CheckResourceAmount(ResourceType.Iron);

        if (ironAmount < 5)
        {
            return;
        }
        ResourceManager.Instance.RemoveFromStorage(5, ResourceType.Iron);
        ResourceManager.Instance.UpgradeExtractionAmountByType(1, ResourceType.Copper);
        DisableThisUpgradeBtn();
    }
}
