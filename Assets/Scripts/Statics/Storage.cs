/// <summary>
/// Это склад всех ресурсов, сюда добавлять и отсюда брать информацию
/// </summary>
public static class Storage
{
    public static float IronResource;
    public static float CopperResource;

    public static void AddToStorage(float amount, ResourceType resourceType)
    {
        switch(resourceType)
        {
            case ResourceType.Copper:
                CopperResource += amount;
                break;
            case ResourceType.Iron:
                IronResource += amount;
                break;
        }
        GameManager.Instance.UpdateUI();
    }

    public static void RemoveFromStorage(float amount, ResourceType resourceType)
    {
        switch (resourceType)
        {
            case ResourceType.Copper:
                CopperResource -= amount;
                break;
            case ResourceType.Iron:
                IronResource -= amount;
                break;
        }
        GameManager.Instance.UpdateUI();
    }
}
