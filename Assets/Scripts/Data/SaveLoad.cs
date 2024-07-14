using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.TextCore.Text;

public static class SaveLoad
{
    private static string path = Application.persistentDataPath + "/gamesave.skillbox"; //���� � ����������. �� ������ ������������ ����� ����������
    private static BinaryFormatter formatter = new BinaryFormatter(); //�������� ������������� 

    public static Vector3 PlayerPosition { get; private set; }
    public static List<Minis> Minis { get; private set; }
    
    public static bool Loaded{ get; private set; }
    public static void SaveGame() //����� ��� ����������
    {
        FileStream fs = new FileStream(path, FileMode.Create); //�������� ��������� ������

        GameData data = new GameData(); //��������� ������

        formatter.Serialize(fs, data); //������������ ������

        fs.Close(); //�������� ������
    }
    public static bool CanLoad()
    {
        return File.Exists(path);
    }
    public static void LoadGame() //����� ��������
    {
        Loaded = false;
        if (File.Exists(path))
        {
            FileStream fs = new FileStream(path, FileMode.Open); //�������� ������

            GameData data = formatter.Deserialize(fs) as GameData; //��������� ������

            GameObject.Find("Player").transform.position = data.PlayerPosition;
            Minis = data.Minis;
            Loaded = true;
        }
    }
}
