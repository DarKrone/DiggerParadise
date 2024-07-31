using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AllResourcesOnMap : MonoBehaviour
{
    [SerializeField] public List<ResourceOre> Resources;

    private void Awake()
    {
        Resources = GetComponentsInChildren<ResourceOre>().ToList();
    }

    public List<float> GetResourcesAmounts()
    {
        List<float> amounts = new List<float>();
        foreach (var r in Resources)
        {
            amounts.Add(r.DefaultResourceAmount);
        }
        return amounts;
    }

    public void SetResourceAmounts(List<float> amounts)
    {
        for (int i = 0; i < amounts.Count; i++)
        {
            Resources[i].DefaultResourceAmount = amounts[i];
            Resources[i].SetResourceAmount();
        }
    }
}
