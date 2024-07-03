using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerResourceExtraction : Extract
{
    protected override bool CheckIfMinerMoving()
    {
        return PlayerMovement.Instance.IsMoving;
    }

    protected override void StopMining()
    {
        PlayerMovement.Instance.IsMining = false;
    }

    protected override void ExtractResource()
    {
        PlayerMovement.Instance.IsMining = true;
        base.ExtractResource();
    }
}

