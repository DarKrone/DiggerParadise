/// <summary>
/// Это склад всех ресурсов, сюда добавлять и отсюда брать информацию
/// </summary>
public static class Storage
{
    private static float _ironResource;
    private static float _copperResource;

    public static void AddToStorage(float amount, ResourceType resourceType)
    {
        switch(resourceType)
        {
            case ResourceType.Copper:
                _copperResource += amount;
                break;
            case ResourceType.Iron:
                _ironResource += amount;
                break;
        }
        GameManager.Instance.UpdateUI();
    }

    public static void RemoveFromStorage(float amount, ResourceType resourceType)
    {
        switch (resourceType)
        {
            case ResourceType.Copper:
                _copperResource -= amount;
                break;
            case ResourceType.Iron:
                _ironResource -= amount;
                break;
        }
        GameManager.Instance.UpdateUI();
    }

    public static float CheckResourceAmount(ResourceType resourceType)
    {
        switch (resourceType)
        {
            case ResourceType.Copper:
                return _copperResource;
            case ResourceType.Iron:
                return _ironResource;
            default:
                return -1;
        }
    }
}
