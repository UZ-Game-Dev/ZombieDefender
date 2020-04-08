using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.IO;
using UnityEngine.Audio;

public class SettingsMenu : MonoBehaviour
{
    public static SettingsMenu S;

    [Header("Definiowane w inspektorze")]
    public AudioMixer musicVolumeMixer;
    public AudioMixer effectVolumeMixer;

    public delegate void NewSettingsChange();
    public static event NewSettingsChange SettingsChange;

    [Header("Zmienna tymczasowa")]
    [SerializeField]
    private bool isScenesMenu = true;

    [Header("Panel ustawień wideo")]
    public TMP_Dropdown resolutionDropdown;
    public TMP_Dropdown graphicsQualityDropdown, antiAliassingDropdown, shadowQualityDropdown;
    public Toggle fullScreenToggle, ambientOcclusionToggle, depthOfFieldToggle, bloomToggle, smokeToggle, vSyncToggle;

    [Header("Panel ustawień dzwieku")]
    public Slider musicVolumeSlider;
    public Slider effectVolumeSlider;

    public bool GetAmbientOcclusion() { return _ambientOcclusion; }
    public bool GetBloom() { return _bloom; }
    public bool GetDepthOfField() { return _depthOfField; }
    public bool GetSmoke() { return _smoke; }

    private string _resolution;
    private bool _fullscreen;
    private int _graphicsQuality;
    private int _antiAliassing, _shadowQuality;
    private bool _ambientOcclusion, _bloom, _depthOfField, _smoke, _vSync;
    private float _musicVolume, _effectVolume;

    private Resolution[] _resolutionList;

    void Awake()
    {
        if (S != null)
            Debug.LogError("Sigleton SettingsMenu juz istnieje");
        S = this;
    }

    void Start()
    {
        _resolutionList = Screen.resolutions;

        loadSetting();
    }

    //Aktualizacja panelu ustawień wideo
    void UpdatePanelSettingVideo()
    {
        antiAliassingDropdown.value = _antiAliassing;
        ambientOcclusionToggle.isOn = _ambientOcclusion;
        depthOfFieldToggle.isOn = _depthOfField;
        bloomToggle.isOn = _bloom;
        shadowQualityDropdown.value = _shadowQuality;
        smokeToggle.isOn = _smoke;
        vSyncToggle.isOn = _vSync;
    }

    //Aktualizacja panelu ustawień dzwieku
    void UpdatePanelSettingSound()
    {
        musicVolumeSlider.value = _musicVolume;
        effectVolumeSlider.value = _effectVolume;
    }

    //Wczytywanie ustawień z pliku
    void loadSetting()
    {
        string json;

        if (File.Exists(Application.dataPath + "/Settings.json"))
        {
            json = File.ReadAllText(Application.dataPath + "/Settings.json");
        }
        else
        {
            SettingsData settingsData = new SettingsData();
            json = JsonUtility.ToJson(settingsData);
            File.WriteAllText(Application.dataPath + "/Settings.json", json);
        }

        SettingsData loadSettingsData = JsonUtility.FromJson<SettingsData>(json);

        _resolution = loadSettingsData.resolution;
        _fullscreen = loadSettingsData.fullscreen;

        if (isScenesMenu)//Tymczasowy if aby nie było błędów w scenie _MainScene
        {
            resolutionDropdown.ClearOptions();

            List<string> options = new List<string>();

            int currentResolutionindex = 0;
            for (int i = 0; i < _resolutionList.Length; i++)
            {
                string option = _resolutionList[i].width + " x " + _resolutionList[i].height + " " + _resolutionList[i].refreshRate + "Hz";
                options.Add(option);

                if (_resolution.Equals(_resolutionList[i].width + "x" + _resolutionList[i].height))
                {
                    currentResolutionindex = i;
                }
            }

            resolutionDropdown.AddOptions(options);
            resolutionDropdown.value = currentResolutionindex;
            resolutionDropdown.RefreshShownValue();
        }//

        _graphicsQuality = loadSettingsData.graphicsQuality;

        if (isScenesMenu)//Tymczasowy if aby nie było błędów w scenie _MainScene
        {
            graphicsQualityDropdown.value = _graphicsQuality;
            fullScreenToggle.isOn = _fullscreen;
        }

        _antiAliassing = loadSettingsData.antiAliassing;
        _ambientOcclusion = loadSettingsData.ambientOcclusion;
        _depthOfField = loadSettingsData.depthOfField;
        _bloom = loadSettingsData.bloom;
        _shadowQuality = loadSettingsData.shadowQuality;
        _smoke = loadSettingsData.smoke;
        _vSync = loadSettingsData.vSync;

        _musicVolume = loadSettingsData.musicVolume;
        _effectVolume = loadSettingsData.effectVolume;

        if (isScenesMenu)//Tymczasowy if aby nie było błędów w scenie _MainScene
        {
            UpdatePanelSettingVideo();
            UpdatePanelSettingSound();
        }

        UpdateSettings();
        UpdateOtherSettings();
    }

    //Zapisywanie informacji do pliku
    void SaveSettingsData()
    {
        SettingsData settingsData = new SettingsData();

        settingsData.resolution = _resolution;
        settingsData.fullscreen = _fullscreen;
        settingsData.graphicsQuality = _graphicsQuality;

        settingsData.antiAliassing = _antiAliassing;
        settingsData.ambientOcclusion = _ambientOcclusion;
        settingsData.depthOfField = _depthOfField;
        settingsData.bloom = _bloom;
        settingsData.shadowQuality = _shadowQuality;
        settingsData.smoke = _smoke;
        settingsData.vSync = _vSync;

        settingsData.musicVolume = _musicVolume;
        settingsData.effectVolume = _effectVolume;

        string json = JsonUtility.ToJson(settingsData);

        File.WriteAllText(Application.dataPath + "/Settings.json", json);
    }

    //Aktualizacja pozostałych ustawień na scenie, takich jak np. post processing
    void UpdateSettings()
    {
        QualitySettings.SetQualityLevel(_graphicsQuality);

        SetAntiAliassing(_antiAliassing);
        SetAmbientOcclusio(_ambientOcclusion);
        SetDepthOfField(_depthOfField);
        SetBloom(_bloom);
        SetShadowsQuality(_shadowQuality);
        SetSmoke(_smoke);
        SetvSync(_vSync);
    }

    //Aktualizacja pozostałych ustawień na scenie, takich jak np. post processing
    void UpdateOtherSettings()
    {
        if (SettingsChange != null)
        {
            SettingsChange();
        }
    }

    //USTAWIENIA GRAFICZNE PRZYCISKI

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = _resolutionList[resolutionIndex];
        _resolution = resolution.width + "x" + resolution.height;
        Screen.SetResolution(resolution.width,resolution.height, Screen.fullScreen);
    }

    public void SetFullscreen(bool isFullscreen)
    {
        _fullscreen = isFullscreen;
        Screen.fullScreen = _fullscreen;
    }

    public void SetGraphicsQuality(int qualityIndex)
    {
        _graphicsQuality = qualityIndex;
        switch (_graphicsQuality)
        {
            case 0: _antiAliassing = 0; _ambientOcclusion = false; _depthOfField = false; _bloom = false; _shadowQuality = 0; _smoke = false; _vSync = false; break;//Potato
            case 1: _antiAliassing = 0; _ambientOcclusion = false; _depthOfField = false; _bloom = true; _shadowQuality = 0; _smoke = false; _vSync = false; break;//Low
            case 2: _antiAliassing = 0; _ambientOcclusion = false; _depthOfField = true; _bloom = true; _shadowQuality = 1; _smoke = false; _vSync = false; break;//Medium
            case 3: _antiAliassing = 2; _ambientOcclusion = true; _depthOfField = true; _bloom = true; _shadowQuality = 2; _smoke = true; _vSync = true; break;//High
            case 4: _antiAliassing = 4; _ambientOcclusion = true; _depthOfField = true; _bloom = true; _shadowQuality = 3; _smoke = true; _vSync = true; break;//Very High
            case 5: _antiAliassing = 8; _ambientOcclusion = true; _depthOfField = true; _bloom = true; _shadowQuality = 4; _smoke = true; _vSync = true; break;//Ultra
            default: _antiAliassing = 0; _ambientOcclusion = false; _depthOfField = false; _bloom = false; _shadowQuality = 0; _smoke = false; _vSync = false; break;
        }

        UpdateSettings();
        UpdatePanelSettingVideo();
        UpdateOtherSettings();
    }

    public void SetAntiAliassing(int isAntiAliassingIndex)
    {
        _antiAliassing = isAntiAliassingIndex;
        switch (_antiAliassing)
        {
            case 0: QualitySettings.antiAliasing = 0; break;
            case 1: QualitySettings.antiAliasing = 2; break;
            case 2: QualitySettings.antiAliasing = 4; break;
            case 3: QualitySettings.antiAliasing = 8; break;
            default: QualitySettings.antiAliasing = 4; break;
        }
    }

    public void SetAmbientOcclusio(bool isAmbientOcclusio)
    {
        _ambientOcclusion = isAmbientOcclusio;
        UpdateOtherSettings();
    }

    public void SetDepthOfField(bool isDepthOfField)
    {
        _depthOfField = isDepthOfField;
        UpdateOtherSettings();
    }

    public void SetBloom(bool isBloom)
    {
        _bloom = isBloom;
        UpdateOtherSettings();
    }

    public void SetShadowsQuality(int isShadowsQualityIndex)
    {
        _shadowQuality = isShadowsQualityIndex;
        switch (_shadowQuality)
        {
            case 0: QualitySettings.shadows = ShadowQuality.Disable; QualitySettings.shadowResolution = ShadowResolution.Low; break;
            case 1: QualitySettings.shadows = ShadowQuality.All; QualitySettings.shadowResolution = ShadowResolution.Low; break;
            case 2: QualitySettings.shadows = ShadowQuality.All; QualitySettings.shadowResolution = ShadowResolution.Medium; break;
            case 3: QualitySettings.shadows = ShadowQuality.All; QualitySettings.shadowResolution = ShadowResolution.High; break;
            case 4: QualitySettings.shadows = ShadowQuality.All; QualitySettings.shadowResolution = ShadowResolution.VeryHigh; break;
            default: QualitySettings.shadows = ShadowQuality.All; QualitySettings.shadowResolution = ShadowResolution.High; break;
        }
    }

    public void SetSmoke(bool isSmoke)
    {
        _smoke = isSmoke;
        UpdateOtherSettings();
    }

    public void SetvSync(bool isvSync)
    {
        _vSync = isvSync;
        switch (_vSync)
        {
            case true: QualitySettings.vSyncCount = 1; break;
            case false: QualitySettings.vSyncCount = 0; break;
            default: QualitySettings.vSyncCount = 1; break;
        }
    }

    //USTAWIENIA DŹWIĘKU PRZYCISKI

    public void SetMusicVolume(float musicVolume)
    {
        _musicVolume = musicVolume;
        musicVolumeMixer.SetFloat("MusicVolume", _musicVolume);
    }

    public void SetEffectVolume(float effectVolume)
    {
        _effectVolume = effectVolume;
        effectVolumeMixer.SetFloat("EffectVolume", _effectVolume);
    }

    public void SaveSettingButton(GameObject panel)
    {
        SaveSettingsData();
        panel.SetActive(false);
    }

    public void CancelSettingButton(GameObject panel)
    {
        loadSetting();
        UpdatePanelSettingVideo();
        UpdatePanelSettingSound();
        panel.SetActive(false);
    }
}