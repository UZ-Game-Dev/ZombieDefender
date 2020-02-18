using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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
    public int amunationPrice = 10;
    public int amunationPiecesToBuy = 5;

    [Header("Definiowane dynamicznie")]
    public bool isActive = false;
    private bool _isisMovingDefensiveObjects = false;
    private float _timer;
    private Weapon weapon;
    private GameObject _defensiveObjectGhost;
    private GameObject _defensiveObject;
    private int _defensiveObjectsNumber;


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
