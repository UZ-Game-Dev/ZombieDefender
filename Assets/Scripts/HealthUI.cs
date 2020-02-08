using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    [Header("Definiowane w panelu")]
    public GameObject uiProfab;
    public Transform target;

    private Transform _ui;
    private Image _healthSlider;

    private void Start()
    {
        foreach(Canvas c in FindObjectsOfType<Canvas>())
        {
            if (c.renderMode == RenderMode.WorldSpace)
            {
                _ui = Instantiate(uiProfab,c.transform).transform;
                _healthSlider = _ui.GetChild(0).GetComponent<Image>();
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

        _healthSlider.fillAmount = HP/maxHP;
    }

    public void SetActive(bool active)
    {
        _ui.gameObject.SetActive(active);
    }
}
