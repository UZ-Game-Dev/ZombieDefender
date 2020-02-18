using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(HealthUI))]
public class DefensiveObject : MonoBehaviour
{
    [Header("Definiowane w panelu")]
    [SerializeField]
    private float _health = 6f;
    
    private HealthUI _healthUI;
    private float _maxHP;

    private void Awake()
    {
        _healthUI = GetComponent<HealthUI>();
        _maxHP = _health;
    }

    public void TakeDamage(float dmg)
    {
        _health -= dmg;
        _healthUI.updateHP(_health, _maxHP);

        if (_health <= 0)
        {
            Destroy(gameObject);
        }
    }
}