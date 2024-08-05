using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PropertiesHandler : MonoBehaviour
{
    [SerializeField] private GameObject _propertiesContainer;
    private List<CurrentResPropertyUI> _resourceProperties;

    private void Start()
    {
        _resourceProperties = _propertiesContainer.gameObject.transform.GetComponentsInChildren<CurrentResPropertyUI>().ToList();
    }

    public void UpdateAllProperties()
    {
        foreach (var prop in _resourceProperties)
        {
            prop.UpdateProperties();
        }
    }
}
