using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerResourceExtraction : Extract
{
    protected override void SetExtractParams()
    {
        curResourceExtractAmount = ResourceManager.Instance.GetExtractionAmountByType(_currentResource.ResourceType);
        curResourceExtractSpeed = ResourceManager.Instance.GetExtractionSpeedByType(_currentResource.ResourceType);
    }
    protected override bool CheckIfMinerMoving()
    {
        return PlayerMovement.Instance.IsMoving;
    }

    protected override void StopMining()
    {
        PlayerMovement.Instance.IsMining = false;
        //RewardedAds.Instance.TryADSAfterResourceOreExtracting();
    }

    protected override void ExtractResource()
    {
        PlayerMovement.Instance.IsMining = true;
        base.ExtractResource();
    }
}

