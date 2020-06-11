using System.IO;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;



public static class SaveSystem 
{
    private static string path = Application.persistentDataPath + "/free.shooting";
    
    public static bool testExist() {
        if (File.Exists(path))
        {
            return true;
        }
        else
            return false;
    }
    public static void SavePlayer()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(path, FileMode.Create);
        PlayerData data = new PlayerData();
        formatter.Serialize(stream, data);
        stream.Close();
    }
    public static PlayerData loadPlayerData()
    {
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            PlayerData data = formatter.Deserialize(stream) as PlayerData;
            stream.Close();
            return data;
        }
        else
        {
            Debug.LogError("Save file not found in " + path);
            return null;
        }
    }
        
}
