public class PlayerResourceExtraction : Extract
{
    protected override void SetExtractParams()
    {
        Resource resource = ResourceManager.Instance.GetResourceByType(_currentResource.ResourceType);
        curResourceExtractAmount = resource.ExtractionAmount;
        curResourceExtractSpeed = resource.ExtractionSpeed;
    }
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

