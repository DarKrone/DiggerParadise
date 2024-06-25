using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Iron : Resource
{
    protected override void DebugResourceAmount()
    {
        Debug.Log($"Current iron amount - {Storage.IronResource}");
    }

    protected override void ExtractResource()
    {
        Storage.IronResource += Extraction.IronExtractAmount;
    }

    protected override void ResourceNotification()
    {
        NotificationHandler.Instance.ShowNotification(ResourceType.Iron);
    }
}
