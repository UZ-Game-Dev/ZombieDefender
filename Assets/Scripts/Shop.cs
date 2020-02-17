using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Shop : MonoBehaviour
{
    [Header("Definiowane w panelu inspekcyjnym")]
    public GameObject shopPanel;
    public TextMeshProUGUI timerToNextWave;
    public int maxGoldForSkip = 20;
    public int amunationPrice = 10;
    public int amunationPiecesToBuy = 5;

    [Header("Definiowane dynamicznie")]
    public bool isActive = false;
    private float _timer;
    private Weapon weapon;

    public void SetTimer(float time)
    {
        _timer = time;
        isActive = true;
    }

    private void Update()
    {
        if(isActive)
        {
            _timer -= Time.deltaTime;
            UpdateTimerText();

            if (_timer < 0)
                isActive = false;
        }
    }

    private void UpdateTimerText()
    {
        int seconds = (int)_timer % 60;
        float miliseconds = (_timer - seconds) * 100;
        timerToNextWave.text = seconds.ToString("00") + ":" + miliseconds.ToString("00");
    }

    //_______________SHOP_BUTTONS______________________

    public void NextWave()
    {
        int bonusGold = (int)_timer;
        Debug.Log("NEXT WAVE +zlota: " + bonusGold);
        if (bonusGold > maxGoldForSkip)
            bonusGold = maxGoldForSkip;

        Main.S.gold += (int)_timer;
        Main.S.StopWaveCoroutine();
    }

    public void BuyAmmunation()
    {
        if (Main.S.gold >= amunationPrice)
        {
            Debug.Log("KUPUJE AMMO");
            Main.S.gold -= amunationPrice;
            if (weapon == null) FindWeaponObject();
            weapon.GetWeapon().ammo += amunationPiecesToBuy;
        }
    }

    public void BuyHealth()
    {
        if(Main.S.gold >= Player.S.healthUpragdeCost)
        {
            Debug.Log("Kupuję Zdrowię");
            Main.S.gold -= Player.S.healthUpragdeCost;
            if(Player.S.healthLevel < Player.S.maxHealthLevel)
            {
                Player.S.healthLevel++;
                Player.S.maxHP += 20;
            }
            if (Player.S.GetHP() < Player.S.maxHP)
                Player.S.Heal(Player.S.maxHP);
        }
        
    }

    public void BuyPistol()
    {
        if (Main.S.gold >= 1)
        {
            Debug.Log("Kupuję Pistolet");
            if (weapon == null) FindWeaponObject();
            Main.S.gold -= 1;
            weapon.GetWeapon().Upgrade();
        }
    }

    public void BuySemiAutomaticGun()
    {
        Debug.Log("Kupuję karabin półautomatyczny");
    }

    public void BuyAutomaticGun()
    {
        Debug.Log("Kupuję karabin automatyczny");
    }

    public void BuyStealFency()
    {
        Debug.Log("Kupuję kubuje płot stalowy");
    }

    public void BuySpikes()
    {
        Debug.Log("Kupuję kupuje kolce");
    }

    private void FindWeaponObject()
    {
        GameObject gameObject = GameObject.FindGameObjectWithTag("Weapon");
        weapon = gameObject.GetComponent<Weapon>();
    }
}
