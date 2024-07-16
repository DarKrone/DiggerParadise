using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.TextCore.Text;

public static class SaveLoad
{
    [Serializable]
    public class GameData
    {
        public float PlayerPositionX;
        public float PlayerPositionY;
        public float PlayerPositionZ;

        public List<ResourceParams> ResourceParams;
        public List<int> UpgradeMinisTiers;
        public GameData(Vector3 PlayerPosition, List<Resource> Resources)
        {
            this.PlayerPositionX = PlayerPosition.x;
            this.PlayerPositionY = PlayerPosition.y;
            this.PlayerPositionZ = PlayerPosition.z;

            this.ResourceParams.Clear();
            foreach (var el in Resources)
            {
                this.ResourceParams.Add(new ResourceParams(el));
            }
        }
        public GameData()
        {

        }
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
    }

    private static string path = Application.persistentDataPath + "/gamesave.fm"; //Путь к сохранению. Вы можете использовать любое расширение
    private static BinaryFormatter formatter = new BinaryFormatter(); //Создание сериализатора 

    public static GameData currentData = new GameData();

    public static bool Loaded{ get; private set; }
    public static void SaveGame() //Метод для сохранения
    {
        FileStream fs = new FileStream(path, FileMode.Create); //Создание файлового потока

        //GameData data = new GameData(PlayerPosition, Resources); //Получение данных

        formatter.Serialize(fs, currentData); //Сериализация данных

        fs.Close(); //Закрытие потока
        Debug.Log("Game saved");
    }
    public static bool CanLoad()
    {
        return File.Exists(path);
    }
    public static void LoadGame() //Метод загрузки
    {
        Loaded = false;
        if (File.Exists(path))
        {
            FileStream fs = new FileStream(path, FileMode.Open); //Открытие потока

            GameData data = formatter.Deserialize(fs) as GameData; //Получение данных

            currentData = data;

            Loaded = true;
            Debug.Log("Game loaded");
        }
    }
}
