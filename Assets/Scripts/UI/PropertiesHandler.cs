using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PropertiesHandler : MonoBehaviour
{
    private List<CurrentResPropertyUI> _resourceProperties;

    private void Start()
    {
        _resourceProperties = GetComponentsInChildren<CurrentResPropertyUI>().ToList();
    }

    public void UpdateAllProperties()
    {
        foreach (var prop in _resourceProperties)
        {
            prop.UpdateProperties();
        }
    }
}
