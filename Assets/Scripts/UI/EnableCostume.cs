using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableCostume : MonoBehaviour
{
    [SerializeField] private GameObject _costumeBtn;
    [SerializeField] private GameObject _costumeBtnTemplate;

    public void SetCostumeToEnable()
    {
        _costumeBtn.SetActive(true);
        _costumeBtnTemplate.SetActive(false);
    }
}
