using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class GameData: MonoBehaviour
{
    public Vector3 PlayerPosition;

    public List<Minis> Minis;

    [SerializeField] private MinisData _minisData;
    private void Start()
    {
        SaveLoad.LoadGame();
    }
    public GameData()
    {
        PlayerPosition = GameObject.Find("Player").transform.position;
        Minis = _minisData.Minis;
    }
    private void OnApplicationQuit()
    {
        Debug.Log("Game paused");
        SaveLoad.SaveGame();
    }
}
