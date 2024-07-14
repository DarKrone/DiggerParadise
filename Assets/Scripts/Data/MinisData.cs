using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinisData : MonoBehaviour
{
    public List<Minis> Minis;

    [SerializeField] private List<Minis> _minis;

    private void Start()
    {
        if (SaveLoad.Loaded)
            Load();
    }
    private void Load()
    {
        for (int i = 0; i < _minis.Count; i++)
        {
            _minis[i] = Minis[i];
        }
    }
}
