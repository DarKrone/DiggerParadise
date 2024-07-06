using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstMinis : MonoBehaviour, IComplitedConstruction
{
    [SerializeField] GameObject _minis;
    [SerializeField] GameObject _minisArea;

    public void ConstructionCompleted()
    {
        _minis.SetActive(true);
        _minisArea.SetActive(true);
    }
}
