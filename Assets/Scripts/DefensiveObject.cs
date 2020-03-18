using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(HealthUI))]
public class DefensiveObject : MonoBehaviour
{
    [Header("Wartości ustawiane na początku rozgrywki")]
    public int initialCurrentLevel = 0;
    public float _Initialhealth = 10f;
    public int initialUpgradePrice = 15;

    [Header("Wartości definiowane dynamicznie")]
    public int currentLevel = 0;
    private float _health = 10f; //Wytrzymałość obiektu
    public int upgradePrice = 15;
    [SerializeField]
    private float _maxHP; //Max wytrzymałość obiektu

    [Header("Definiowane w panelu inspekcyjnym")]
    public int _maxLevel = 10;
    public int _bonusHealtOnLevel = 5;

    private HealthUI _healthUI;

    private void Awake()
    {
        _healthUI = GetComponent<HealthUI>();
        _maxHP = _health;
    }

    public void Initialize()
    {
        //Wywoływane prze funkcję Start() w skryocie Shop
        _health = _Initialhealth;
        _maxHP = _health;
        currentLevel = initialCurrentLevel;
        upgradePrice = initialUpgradePrice;
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

    public void Upgrade()
    {
        if(currentLevel < _maxLevel)
        {
            currentLevel++;
            _health += _bonusHealtOnLevel;
            _maxHP += _bonusHealtOnLevel;
            upgradePrice += currentLevel * 3;
        }
    }
}