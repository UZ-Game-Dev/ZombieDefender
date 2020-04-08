using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class SettingsManager : MonoBehaviour
{

    public GameObject[] tableSpecialEffects;

    public GameObject ambientOcclusion;
    public GameObject depthOfField;
    public GameObject bloom;

    void OnEnable()
    {
        SettingsMenu.SettingsChange += UpdateScene;
    }
    
    void OnDisable()
    {
        SettingsMenu.SettingsChange -= UpdateScene;
    }
    
    void UpdateScene()
    {
        if (tableSpecialEffects != null)
        {
            for (int i = 0; i < tableSpecialEffects.Length; i++)
            {
                tableSpecialEffects[i].SetActive(SettingsMenu.S.GetSmoke());
            }
        }

        if (ambientOcclusion != null) ambientOcclusion.SetActive(SettingsMenu.S.GetAmbientOcclusion());
        if (depthOfField != null) depthOfField.SetActive(SettingsMenu.S.GetDepthOfField());
        if (bloom != null) bloom.SetActive(SettingsMenu.S.GetBloom());
    }
}