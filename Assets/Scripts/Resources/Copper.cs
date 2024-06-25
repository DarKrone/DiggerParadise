using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Copper : Resource
{
    protected override void DebugResourceAmount()
    {
        Debug.Log($"Current copper amount - {Storage.CopperResource}");
    }

    protected override void ExtractResource()
    {
        Storage.AddToStorage(Extraction.CopperExtractAmount, ResourceType.Copper);
    }

    protected override void ResourceNotification()
    {
        NotificationHandler.Instance.ShowNotification(ResourceType.Copper);
    }
}
