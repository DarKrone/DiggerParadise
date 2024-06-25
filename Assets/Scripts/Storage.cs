/// <summary>
/// Это склад всех ресурсов, сюда добавлять и отсюда брать информацию
/// </summary>
public static class Storage
{
    public static float IronResource;
    public static float CopperResource;

    static void AddToStorage(float amount, ResourceType name)
    {
        switch(name)
        {
            case ResourceType.Copper:
                CopperResource += amount;
                break;
            case ResourceType.Iron:
                IronResource += amount;
                break;
        }
    }

    static void RemoveFromStorage(float amount, ResourceType name)
    {
        switch (name)
        {
            case ResourceType.Copper:
                CopperResource -= amount;
                break;
            case ResourceType.Iron:
                IronResource -= amount;
                break;
        }
    }
}
