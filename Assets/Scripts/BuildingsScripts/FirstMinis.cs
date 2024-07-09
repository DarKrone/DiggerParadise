using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FirstMinis : MonoBehaviour, IComplitedConstruction
{
    [SerializeField] private GameObject _minis;
    [SerializeField] private GameObject _minisArea;
    [SerializeField] private GameObject _building;
    [SerializeField] private Sprite _nextBuildingTier;
    [SerializeField] private ParticleSystem _particleSystem;

    public void ConstructionCompleted()
    {
        _particleSystem.Play();
        _building.GetComponent<SpriteRenderer>().sprite = _nextBuildingTier;
        _minis.SetActive(true);
        _minisArea.SetActive(true);
    }
}
