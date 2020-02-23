using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(HealthUI))]
public class DefensiveObject : MonoBehaviour
{
    [Header("Definiowane w panelu")]
    [SerializeField]
    private float _health = 10f;
    
    private HealthUI _healthUI;
    private float _maxHP;

    private void Awake()
    {
        _healthUI = GetComponent<HealthUI>();
        _maxHP = _health;
    }

    public void TakeDamage(float dmg)
    {
        _health -= 1;
        _healthUI.updateHP(_health, _maxHP);

        if (_health <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (Main.S.isEnableToShoot || Main.S.shopPanel.gameObject.activeSelf == true)
        {
            this.gameObject.transform.GetChild(0).gameObject.SetActive(false);
        }
        else
        {
            this.gameObject.transform.GetChild(0).gameObject.SetActive(true);
        }
    }
}