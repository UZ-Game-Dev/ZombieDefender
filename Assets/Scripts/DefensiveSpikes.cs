using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(HealthUI))]
public class DefensiveSpikes : MonoBehaviour
{
    [Header("Wartości ustawiane na początku rozgrywki")]
    public int initialCurrentLevel = 0;
    public float _Initialhealth = 3f;
    public float _InitialdemageEnemy = 10;
    public int initialUpgradePrice = 15;

    [Header("Wartości definiowane dynamicznie")]
    public int currentLevel;
    public float _health; //Wytrzymałość obiektu
    public float _demageEnemy; //Obrazenia zadawane wrogowi
    public int upgradePrice;
    [SerializeField]
    private float _maxHP;

    [Header("Definiowane w panelu inspekcyjnym")]
    public int maxLevel = 10;
    public float _TakeDamage = 1; //Obrazenia zadawane obiektowi
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
        _health = _Initialhealth;
        _maxHP = _health;
        _demageEnemy = _InitialdemageEnemy;
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
            _collider.transform.root.GetComponent<Enemy>().TakeDamage(_demageEnemy);
        }

        _health -= _TakeDamage;
        _healthUI.updateHP(_health, _maxHP);

        if (_health <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void Upgrade()
    {
        if (currentLevel < maxLevel)
        {
            _maxHP = _health += healthUpgrade;
            _demageEnemy += damageUpgrade;
            ++currentLevel;
            upgradePrice += 5;
        }
    }
}
