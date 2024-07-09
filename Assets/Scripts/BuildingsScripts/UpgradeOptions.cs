using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeOptions : MonoBehaviour
{
    [Serializable]
    private class UpgradeParameters
    {
        public ResourceType ResourceTypeNeeded;
        public float UpgradeCost;
        public string UpgradeDesc;
    }

    [SerializeField] private List<UpgradeParameters> UpgradeParametersList;


}
