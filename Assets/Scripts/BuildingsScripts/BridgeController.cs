using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeController : MonoBehaviour, IComplitedConstruction
{
    [SerializeField] private GameObject bridge;
    [SerializeField] private GameObject beforeBorder;
    public void ConstructionCompleted()
    {
        bridge.SetActive(true);
        beforeBorder.SetActive(false);    
    }
}
