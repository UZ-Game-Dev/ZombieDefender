using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(HealthUI))]
public class DefensiveSpikes : MonoBehaviour
{
    [Header("Wartości ustawiane na początku rozgrywki")]
    public int initialCurrentLevel = 0;
    public float initialhealth = 3f;
    public float initialdemageEnemy = 10;
    public int initialUpgradePrice = 15;

    [Header("Wartości definiowane dynamicznie")]
    public int currentLevel;
    public float health; //Wytrzymałość obiektu
    public float damageEnemy; //Obrazenia zadawane wrogowi
    public int upgradePrice;
    public float maxHP; // ← Przemwo - "Nie wiem dlaczego ma być publiczna".

    [Header("Definiowane w panelu inspekcyjnym")]
    public int maxLevel = 10;
    public float takeDamage = 1; //Obrazenia zadawane obiektowi
    public int damageUpgrade = 1;
    public int healthUpgrade = 1; //Poprawienie wytrzymalosci obiektu

    private HealthUI _healthUI;


    private void Awake()
    {
        _healthUI = GetComponent<HealthUI>();

    }
    public void Initialize()
    {
        //Wywoływane prze funkcję Start() w skryocie Shop
        health = initialhealth;
        maxHP = health;
        damageEnemy = initialdemageEnemy;
        currentLevel = initialCurrentLevel;
        upgradePrice = initialUpgradePrice;
    }

    private void OnTriggerEnter(Collider _collider)
    {
        if (_collider.transform.root.GetComponent<Enemy>())
        {
            StartCoroutine(SpikesUp(_collider));
            GetComponent<Animator>().SetBool("Attack", true);
        }
    }

    private IEnumerator SpikesUp(Collider _collider)
    {
        yield return new WaitForSeconds(0.2f);

        if (_collider != null)
        {
            GetComponent<Animator>().SetBool("Attack", false);
            _collider.transform.root.GetComponent<Enemy>().TakeDamage(damageEnemy);
        }
        SoundsMenager.S.PlaySpikesAttack();
        health -= takeDamage;
        _healthUI.updateHP(health, maxHP);

        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    public bool Upgrade()
    {
        if (currentLevel < maxLevel && Main.S.gold >= upgradePrice)
        {
            Main.S.gold -= upgradePrice;
            maxHP = health += healthUpgrade;
            damageEnemy += damageUpgrade;
            ++currentLevel;
            upgradePrice += 5;
            UI.S.gold.text = "Gold: " + Main.S.gold;
            return true;
        }
        return false;
    }
}
