using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(HealthUI))]
public class DefensiveObject : MonoBehaviour
{
    [Header("Wartości ustawiane na początku rozgrywki")]
    public int initialCurrentLevel = 0;
    public float initialhealth = 10f;
    public int initialUpgradePrice = 15;

    [Header("Wartości definiowane dynamicznie")]
    public int currentLevel = 0;
    public float health = 10f; //Wytrzymałość obiektu
    public int upgradePrice = 15;
    public float maxHP; //Max wytrzymałość obiektu

    [Header("Definiowane w panelu inspekcyjnym")]
    public int maxLevel = 10;
    public int bonusHealtOnLevel = 5;
    public Transform prefabsFragments;

    private HealthUI _healthUI;

    private void Awake()
    {
        _healthUI = GetComponent<HealthUI>();
        maxHP = health;
    }

    private void Start()
    {
        _healthUI.updateHP(health, maxHP);
    }

    public void Initialize()
    {
        //Wywoływane prze funkcję Start() w skrypcie Shop
        health = initialhealth;
        maxHP = health;
        currentLevel = initialCurrentLevel;
        upgradePrice = initialUpgradePrice;
    }

    public void TakeDamage(float dmg)
    {
        health -= 1;
        _healthUI.updateHP(health, maxHP);

        if (health <= 0)
        {
            Instantiate(prefabsFragments, new Vector3(transform.position.x, transform.position.y, transform.position.z), transform.rotation);
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

    public bool Upgrade()
    {
        if(currentLevel < maxLevel && Main.S.gold >= upgradePrice)
        {
            Main.S.gold -= upgradePrice;
            currentLevel++;
            health += bonusHealtOnLevel;
            maxHP += bonusHealtOnLevel;
            upgradePrice += currentLevel * 3;
            UI.S.gold.text = "Gold: " + Main.S.gold;
            return true;
        }
        return false;
    }

    public HealthUI GetHealthUI()
    {
        return _healthUI;
    }
}