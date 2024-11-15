using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Collections.Generic;

[Serializable]
public class LevelData
{
    public string name;
    public string resource;
    public float timeSeconds;
    public bool completed;
}

[Serializable]
public class GameData
{
    public string username = "NoName";
    public float speed = 300f;
    public int damage = 25;
    public float dexterity = 5f;
    public int gold = 0;
    public List<LevelData> CompletedLevels = new List<LevelData>();
}

public static class GameDataManager
{
    public static string DataPath = "/gamedata.txt";

    public static void SaveData(GameData data)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + DataPath;
        FileStream stream = new FileStream(path, FileMode.Create);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static GameData LoadData()
    {
        string path = Application.persistentDataPath + DataPath;
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            GameData data = formatter.Deserialize(stream) as GameData;
            stream.Close();
            return data;
        }
        else
        {
            return null;
        }
    }

    public static bool DataExist()
    {
        string path = Application.persistentDataPath + DataPath;
        if (File.Exists(path)) return true;
        else return false;
    }
}