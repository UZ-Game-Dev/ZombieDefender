﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

[System.Serializable]
public class DefensiveObjects
{
    public GameObject prefabs;
    public GameObject prefabsGhost;
    public Vector3 startingPosition;
    public int price;
}

public class Shop : MonoBehaviour
{
    [Header("Definiowane w panelu inspekcyjnym")]
    public GameObject shopPanel;
    public TextMeshProUGUI timerToNextWave;
    public int maxGoldForSkip = 20;
    public DefensiveObjects[] DefensiveObjectsArray;
    public int amunationPrice = 5;
    public int amunationPiecesToBuy = 50;

    [Header("Definiowane dynamicznie")]
    public bool isActive = false;
    private bool _isisMovingDefensiveObjects = false;
    private float _timer;
    private Weapon weapon;
    private GameObject _defensiveObjectGhost;
    private GameObject _defensiveObject;
    private int _defensiveObjectsNumber;
    private Button[] shopButtonsArray;


    //_______________DefensiveObject______________________

    private void MovingDefensiveObjects(int objectNumber)
    {
        _isisMovingDefensiveObjects = true;
        Main.S.isEnableToShoot = false;

        Vector3 toObjectVector = DefensiveObjectsArray[objectNumber].prefabs.transform.position - Camera.main.transform.position;
        Vector3 linearDistanceVector = Vector3.Project(toObjectVector, Camera.main.transform.forward);
        float actualDistance = (DefensiveObjectsArray[objectNumber].prefabs.transform.position - Camera.main.transform.position).magnitude;

        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = actualDistance;

        Vector3 transformPosition = Camera.main.ScreenToWorldPoint(mousePosition);

        _defensiveObjectGhost = Instantiate(DefensiveObjectsArray[objectNumber].prefabsGhost, new Vector3(Mathf.Clamp(transformPosition.x, -2.25f, 6), DefensiveObjectsArray[objectNumber].startingPosition.y, DefensiveObjectsArray[objectNumber].startingPosition.z), DefensiveObjectsArray[objectNumber].prefabsGhost.transform.rotation);
        _defensiveObjectGhost.GetComponent<DefensiveObjectGhost>().SetActualDistance(actualDistance);

        _defensiveObject = DefensiveObjectsArray[objectNumber].prefabs;
    }

    private void ResetInfoDefensiveObject()
    {
        Destroy(_defensiveObjectGhost);
        _defensiveObjectGhost = null;
        _defensiveObject = null;
        _isisMovingDefensiveObjects = false;
        if (!isActive)
        {
            Main.S.isEnableToShoot = true;
        }
    }
    //

    public void SetTimer(float time)
    {
        _timer = time;
        isActive = true;
    }

    private void Update()
    {
        //_______________DefensiveObject______________________

        if (!_isisMovingDefensiveObjects)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1) && Main.S.gold >= DefensiveObjectsArray[0].price)
            {
                _defensiveObjectsNumber = 0;
                MovingDefensiveObjects(_defensiveObjectsNumber);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2) && Main.S.gold >= DefensiveObjectsArray[1].price)
            {
                _defensiveObjectsNumber = 1;
                MovingDefensiveObjects(_defensiveObjectsNumber);
            }
        }

        if (Input.GetButtonUp("Fire1") && !isActive && _isisMovingDefensiveObjects && !DefensiveObjectGhost.S.GetCollision())
        {
            Debug.Log("KUMIONO OBIEKT");
            Main.S.gold -= DefensiveObjectsArray[_defensiveObjectsNumber].price;
            Instantiate(_defensiveObject, _defensiveObjectGhost.transform.position, _defensiveObjectGhost.transform.rotation);
            _defensiveObject.SetActive(true);
            ResetInfoDefensiveObject();
        }

        if (Input.GetKeyDown(KeyCode.Escape) && _isisMovingDefensiveObjects)
        {
            ResetInfoDefensiveObject();
        }

        //

        if (isActive)
        {
            if (_isisMovingDefensiveObjects)
                ResetInfoDefensiveObject();

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

    //____________SHP_PRICE_DISPLEY____________________
    private void Start()
    {
        FindWeaponObject();
        shopButtonsArray = Main.S.shopPanel.GetComponentsInChildren<Button>();
        UpdatePrice();
    }
    public void FixedUpdate()
    {
        if(isActive)
        {
            UpdatePrice();
        }
    }
    public void UpdatePrice()
    {
        foreach(Button button in shopButtonsArray)
        {
            if (button.name == "NextWaveButton") continue;
            TextMeshProUGUI buttonTxt = button.GetComponentInChildren<TextMeshProUGUI>();
            string buttonName = button.name;
            string text = "";
            
            switch(buttonName)
            {
                case "BuyAmmo":
                    text = "+" + amunationPiecesToBuy + " / $" + amunationPrice + "\nBUY";
                    break;
                case "BuyHealth":
                    text = "+" + Player.S.GetHpBonusPerLevel() + "hp / $" + Player.S.GetHpUpgradeCost() + "\nBUY";
                    break;
                case "BuyGun":
                    text = "+Damage \n-Reload Speed \n$" + weapon.weapons[0].moneyForUpgrade + "\nBUY";
                    break;
                case "BuySemiAutomaticGun":
                    text = "+Damage \n-Reload Speed \n$" + weapon.weapons[1].moneyForUpgrade + "\nBUY";
                    break;
                case "BuyAutomaticGun":
                    text = "+Damage \n-Reload Speed \n$" + weapon.weapons[2].moneyForUpgrade + "\nBUY";
                    break;
            }
            if (!text.Equals("")) buttonTxt.text = text;
        }
    }
    //_______________SHOP_BUTTONS______________________

    public void NextWave()
    {
        Debug.Log("NEXT WAVE");
        isActive = false;
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
        if(Main.S.gold >= Player.S.GetHpUpgradeCost())
        {
            Debug.Log("Kupuję Zdrowię");
            Player.S.UpgradeHP();
        }
        
    }

    public void BuyPistol()
    {
        if (Main.S.gold >= weapon.weapons[0].moneyForUpgrade)
        {
            Debug.Log("Kupuję Pistolet");
            weapon.weapons[0].Upgrade();
        }
    }

    public void BuySemiAutomaticGun()
    {
        if (Main.S.gold >= weapon.weapons[0].moneyForUpgrade)
        {
            Debug.Log("Kupuję karabin półautomatyczny");
            weapon.weapons[1].Upgrade();
        }
    }

    public void BuyAutomaticGun()
    {
        if (Main.S.gold >= weapon.weapons[0].moneyForUpgrade)
        {
            Debug.Log("Kupuję karabin automatyczny");
            weapon.weapons[2].Upgrade();
        }   
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
