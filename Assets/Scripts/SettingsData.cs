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