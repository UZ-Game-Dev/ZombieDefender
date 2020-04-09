using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    private static SaveData _data;
    public static bool isGameLoaded;

    public static void SaveGame(int HP, Main main, Shop shop, Weapon weapon)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/save01.save";

        FileStream stream = new FileStream(path, FileMode.Create);

        SaveData data = new SaveData(HP, main, shop, weapon);

        formatter.Serialize(stream, data);
        stream.Close();
        Debug.Log("Game Saved");
    }

    public static void LoadGame()
    {
        string path = Application.persistentDataPath + "/save01.save";

        if(File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            SaveData data = formatter.Deserialize(stream) as SaveData;
            stream.Close();

            Debug.Log("Data loaded");
            _data =  data;
        }
        else
        {
            Debug.LogError("Save file not found in " + path);
            _data = null;
        }
    }

    public static SaveData GetData()
    {
        return _data;
    }
}
