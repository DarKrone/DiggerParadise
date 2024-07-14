using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.TextCore.Text;

public static class SaveLoad
{
    private static string path = Application.persistentDataPath + "/gamesave.skillbox"; //Путь к сохранению. Вы можете использовать любое расширение
    private static BinaryFormatter formatter = new BinaryFormatter(); //Создание сериализатора 

    public static Vector3 PlayerPosition { get; private set; }
    public static List<Minis> Minis { get; private set; }
    
    public static bool Loaded{ get; private set; }
    public static void SaveGame() //Метод для сохранения
    {
        FileStream fs = new FileStream(path, FileMode.Create); //Создание файлового потока

        GameData data = new GameData(); //Получение данных

        formatter.Serialize(fs, data); //Сериализация данных

        fs.Close(); //Закрытие потока
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

            GameObject.Find("Player").transform.position = data.PlayerPosition;
            Minis = data.Minis;
            Loaded = true;
        }
    }
}
