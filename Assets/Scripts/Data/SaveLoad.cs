using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.TextCore.Text;
using YG;

[Serializable]
public class GameData
{
    public float PlayerPositionX;
    public float PlayerPositionY;
    public float PlayerPositionZ;

    public List<ResourceParams> ResourceParams;
    public List<int> UpgradeMinisTiers;
    public List<List<NeededResource>> NeededResources = new List<List<NeededResource>>();
    public List<float> AllResourcesAmounts = new List<float>();

    public Vector3 GetVector3()
    {
        return new Vector3(this.PlayerPositionX,this.PlayerPositionY,this.PlayerPositionZ);
    }
    public void GetPos(Vector3 pos)
    {
        this.PlayerPositionX = pos.x;
        this.PlayerPositionY = pos.y;
        this.PlayerPositionZ = pos.z;
    }
    public GameData()
    {

    }
    public GameData(bool isDefault)
    {
        GetPos(Vector3.zero);

        for (int i = 0; i < ResourceParams.Count; i++) 
        {
            ResourceParams[i] = new ResourceParams();
        }

        for (int i = 0; i < UpgradeMinisTiers.Count; i++)
        {
            UpgradeMinisTiers[i] = 0;
        }

    }
}

public static class SaveLoad
{
    private static string path = Application.persistentDataPath + "/gamesave.fm"; //���� � ����������. �� ������ ������������ ����� ����������
    private static BinaryFormatter formatter = new BinaryFormatter(); //�������� ������������� 

    public static GameData currentData;

    public static bool CanLoad()
    {
        return File.Exists(path);
    }
    public static void SaveGame()
    {
        YandexGame.savesData.gameData = currentData;
        YandexGame.SaveProgress();
        Debug.Log("Game saved");
    }
    public static void LoadGame()
    {
        currentData = new GameData();
        currentData = YandexGame.savesData.gameData;
        Debug.Log("Game loaded");
    }
}
