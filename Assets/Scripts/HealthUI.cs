using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class DestructionEffect
{
    public float hp;
    public GameObject[] gameObjectFragments;
    public GameObject[] gameObjectFragmentsPrefabs;
}

public class HealthUI : MonoBehaviour
{
    [Header("Definiowane w panelu")]
    public GameObject uiProfab;
    public Transform target;
    public DestructionEffect[] destructionEffectArray;

    private Transform _ui;
    private Slider _healthSlider;

    private void Awake()
    {
        foreach(Canvas c in FindObjectsOfType<Canvas>())
        {
            if (c.renderMode == RenderMode.WorldSpace)
            {
                _ui = Instantiate(uiProfab,c.transform).transform;
                _healthSlider = _ui.GetComponent<Slider>();
                break;
            }
        }
    }

    private void LateUpdate()
    {
        if (_ui != null)
        {
            _ui.position = target.position;
            _ui.forward = -Camera.main.transform.forward;
        }
    }

    public void updateHP(float HP,float maxHP)
    {
        if (HP <= 0)
        {
            Destroy(_ui.gameObject);
        }

        _healthSlider.value = HP/maxHP;
        
        if (destructionEffectArray != null)
        {
            for (int i = 0; i < destructionEffectArray.Length; i++)
            {
                if ((HP / maxHP) * 100 < destructionEffectArray[i].hp)
                {
                    for (int i2 = 0; i2 < destructionEffectArray[i].gameObjectFragments.Length; i2++)
                    {
                        destructionEffectArray[i].gameObjectFragments[i2].GetComponent<MeshFilter>().sharedMesh = destructionEffectArray[i].gameObjectFragmentsPrefabs[i2].GetComponent<MeshFilter>().sharedMesh;
                    }
                }
            }
        }
    }

    public void SetActive(bool active)
    {
        _ui.gameObject.SetActive(active);
    }
}