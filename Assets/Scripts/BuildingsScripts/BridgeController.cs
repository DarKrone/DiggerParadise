using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeController : MonoBehaviour, IComplitedConstruction
{
    [SerializeField] private GameObject _bridge;
    [SerializeField] private GameObject _border;

    public void ConstructionCompleted()
    {
        OpenBridge();
    }
    private void OpenBridge()
    {
        _border.SetActive(false);
        _bridge.SetActive(true);
    }
    private void CloseBridge()
    {
        _border.SetActive(true);
        _bridge.SetActive(false);
    }
}
