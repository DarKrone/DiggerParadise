using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Minis : MonoBehaviour, IComplitedConstruction
{
    [SerializeField] private GameObject _minisObjects;
    [SerializeField] private GameObject _building;
    [SerializeField] private Sprite _nextBuildingTier;
    [SerializeField] private GameObject _upgMinisShop;
    [SerializeField] public int HouseTier = 0;
    private void Start()
    {
        
    }
    public void ConstructionCompleted()
    {
        _building.GetComponent<BuildingSpriteHandler>().UpdateBuildingSpriteToNext();
        _minisObjects.SetActive(true);
        _upgMinisShop.GetComponent<UpgradeMinisShop>().UpgradeMinisAfterConstructionCompleted();
        HouseTier += 1;
    }
    public void SetParams(Minis newMinis)
    {
        _minisObjects = newMinis._minisObjects;
        _building = newMinis._building;
        _nextBuildingTier = newMinis._nextBuildingTier;
    }
}
