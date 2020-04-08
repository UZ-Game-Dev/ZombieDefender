using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SettingsData
{
    [Header("Ustawienia graficzne")]
    public string resolution = "1920x1080";
    public bool fullscreen = true;
    public int graphicsQuality = 3;

    [Header("Zaawansowane ustawienia graficzne")]
    public int antiAliassing = 2;
    public bool ambientOcclusion = true;
    public bool depthOfField = true;
    public bool bloom = true;
    public int shadowQuality = 3;
    public bool smoke = true;
    public bool vSync = true;

    [Header("Ustawienia dzwieku")]
    public float musicVolume = 0;
    public float effectVolume = 0;
}

/* (Przykład pobierania i zapisywania pliku json)
 * SettingsData settingsData = new SettingsData();
 * settingsData.quality = 1;
 * settingsData.shadows = false;
 * string json = JsonUtility.ToJson(settingsData);
 * Debug.Log(json);
 * File.WriteAllText(Application.dataPath + "/Settings.json",json);
 * string json = File.ReadAllText(Application.dataPath + "/Settings.json");
 * SettingsData loadSettingsData = JsonUtility.FromJson<SettingsData>(json);
 * Debug.Log(loadSettingsData.quality+", "+loadSettingsData.shadows);
 */