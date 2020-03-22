using System.Collections;
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
    public DefensiveObjects[] DefensiveObjectsArray; // [0] -> KOLCE   [1] -> PŁOT
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
    private AudioSource audioSourceBuing;

    private void Start()
    {
        DefensiveSpikes spikesObject = DefensiveObjectsArray[0].prefabs.GetComponent<DefensiveSpikes>(); //KOLCE
        spikesObject.Initialize();
        DefensiveObject baricadeObject = DefensiveObjectsArray[1].prefabs.GetComponent<DefensiveObject>(); //PŁOT
        baricadeObject.Initialize();
        FindWeaponObject();
        shopButtonsArray = Main.S.shopPanel.GetComponentsInChildren<Button>();

        //BARRICADE
        UI.S.baricadeUpgrade.text = "Endurance: " + baricadeObject.maxHP + " -> " + (baricadeObject.maxHP + baricadeObject.bonusHealtOnLevel);
        UI.S.baricadeCost.text = "Upgrade: " + baricadeObject.upgradePrice + "$";
        //SPIKES 
        UI.S.spikeUpgrade.text = "Damage: " + spikesObject.damageEnemy + " -> " + (spikesObject.damageEnemy + spikesObject.damageUpgrade) + "\n" +
                "Endurance: " + spikesObject.health + " -> " + (spikesObject.health + spikesObject.healthUpgrade);
        UI.S.spikeCost.text = "Upgrade: " + spikesObject.upgradePrice + "$";

        audioSourceBuing = shopPanel.GetComponent<AudioSource>();
    }

    //_______________DefensiveObject______________________

    private void MovingDefensiveObjects(int objectNumber)
    {
        PauseMenu.S.enabled = false;
        _isisMovingDefensiveObjects = true;
        Main.S.isEnableToShoot = false;

        Vector3 toObjectVector = DefensiveObjectsArray[objectNumber].prefabs.transform.position - Camera.main.transform.position;
        Vector3 linearDistanceVector = Vector3.Project(toObjectVector, Camera.main.transform.forward);
        float actualDistance = (DefensiveObjectsArray[objectNumber].prefabs.transform.position - Camera.main.transform.position).magnitude;

        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = actualDistance;

        Vector3 transformPosition = Camera.main.ScreenToWorldPoint(mousePosition);

        _defensiveObjectGhost = Instantiate(DefensiveObjectsArray[objectNumber].prefabsGhost, new Vector3(Mathf.Clamp(transformPosition.x, -2.25f, 5), DefensiveObjectsArray[objectNumber].startingPosition.y, DefensiveObjectsArray[objectNumber].startingPosition.z), DefensiveObjectsArray[objectNumber].prefabsGhost.transform.rotation);
        _defensiveObjectGhost.GetComponent<DefensiveObjectGhost>().SetActualDistance(actualDistance);

        _defensiveObject = DefensiveObjectsArray[objectNumber].prefabs;
    }

    private void ResetInfoDefensiveObject()
    {
        Destroy(_defensiveObjectGhost);
        _defensiveObjectGhost = null;
        _defensiveObject = null;
        PauseMenu.S.enabled = true;
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

        if (!PauseMenu.S.GetIsPaused() && Input.GetKeyDown(KeyCode.Alpha1) && Main.S.gold >= DefensiveObjectsArray[0].price && !_isisMovingDefensiveObjects)
        {
            _defensiveObjectsNumber = 0;
            MovingDefensiveObjects(_defensiveObjectsNumber);
        }
        else if (!PauseMenu.S.GetIsPaused() && Input.GetKeyDown(KeyCode.Alpha2) && Main.S.gold >= DefensiveObjectsArray[1].price && !_isisMovingDefensiveObjects)
        {
            _defensiveObjectsNumber = 1;
            MovingDefensiveObjects(_defensiveObjectsNumber);
        }

        if (Input.GetButtonUp("Fire1") && !isActive && _isisMovingDefensiveObjects && !DefensiveObjectGhost.S.GetCollision())
        {
            Debug.Log("KUMIONO OBIEKT");
            Main.S.gold -= DefensiveObjectsArray[_defensiveObjectsNumber].price;
            UI.S.gold.text = "Gold: " + Main.S.gold;
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
        //float miliseconds = (_timer - seconds) * 100;
        timerToNextWave.text = seconds.ToString("00");
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
        UI.S.gold.text = "Gold: " + Main.S.gold;
        Main.S.StopWaveCoroutine();
    }

    public void BuyAmmunation()
    {
        if (Main.S.gold >= amunationPrice && weapon.GetWeapon().GetType() != Weapon.WeaponType.ePistol)
        {
            
            Debug.Log("KUPUJE AMMO");
            audioSourceBuing.Play();
            Main.S.gold -= amunationPrice;
            UI.S.gold.text = "Gold: " + Main.S.gold;
            if (weapon == null) FindWeaponObject();
            weapon.AddRifleAmmo(amunationPiecesToBuy);
            UI.S.ammo.text = weapon.GetWeapon().GetCurrentAmmo() + "/" + weapon.GetWeapon().GetCapacity() + "  [" + weapon.GetRifleAmmo() + "]";
        }
    }

    public void BuyHealth()
    {
        if(Main.S.gold >= Player.S.GetHpUpgradeCost() && Player.S.GetHpLevel() < Player.S.GetMaxHpLevel())
        {
            Debug.Log("Kupuję Zdrowię");
            audioSourceBuing.Play();
            Player.S.UpgradeHP();
            UI.S.gold.text = "Gold: " + Main.S.gold;

            if (Player.S.GetHpLevel() == Player.S.GetMaxHpLevel())
            {
                UI.S.hpUpgrade.text = "Health level: " + Player.S.GetHpLevel() + " / " + Player.S.GetMaxHpLevel() + "\n" + "\nCost: " + Player.S.GetHpUpgradeCost() + "$\n ";
            }
            else
            {
                UI.S.hpUpgrade.text = "Health level: " + Player.S.GetHpLevel() + " / " + Player.S.GetMaxHpLevel() + "\n" + Player.S.GetMaxHP() + " -> " +
                   (Player.S.GetMaxHP() + Player.S.GetHpBonusPerLevel()) + "\nCost: " + Player.S.GetHpUpgradeCost() + "$\n ";
            }
        }
    }

    public void BuyPistol()
    {
        weapon.weapons.Find(w => w.GetType() == Weapon.WeaponType.ePistol).Upgrade();
        audioSourceBuing.Play();
    }

    public void BuySemiAutomaticGun()
    {
        Weapon.SemiAutomatic semi = (Weapon.SemiAutomatic) weapon.weapons.Find(w => w.GetType() == Weapon.WeaponType.eSemiAutomatic);

        if (semi == null)
        {
            semi = new Weapon.SemiAutomatic();
            if (Main.S.gold >= semi.GetBuyingPrice())
            {
                audioSourceBuing.Play();
                Main.S.gold -= semi.GetBuyingPrice();
                UI.S.gold.text = "Gold: " + Main.S.gold;
                UI.S.semiUpgrade.text = "Cost: " + semi.GetMoneyForUpgrade() + "$";
                UI.S.semiReloadTime.text = "Reload Spd.: " + semi.GetReloadSpeed() + " -> " + (semi.GetReloadSpeed() - 0.05f);
                UI.S.semiDamage.text = "Damage: " + semi.GetDamage() + " -> " + (semi.GetDamage() + 1.5f);
                weapon.weapons.Add(semi);
                weapon.AddRifleAmmo(24);
            }
        }
        else
        {
            audioSourceBuing.Play();
            weapon.weapons.Find(gun => gun.GetType()==Weapon.WeaponType.eSemiAutomatic).Upgrade();
        }
    }

    public void BuyAutomaticGun()
    {
        Weapon.Automatic auto = (Weapon.Automatic)weapon.weapons.Find(w => w.GetType() == Weapon.WeaponType.eAutomatic);

        if (auto == null)
        {
            auto = new Weapon.Automatic();
            if (Main.S.gold >= auto.GetBuyingPrice())
            {
                audioSourceBuing.Play();
                Main.S.gold -= auto.GetBuyingPrice();
                UI.S.gold.text = "Gold: " + Main.S.gold;
                UI.S.autoUpgrade.text = "Cost: " + auto.GetMoneyForUpgrade() + "$";
                UI.S.autoReloadTime.text = "Reload Spd.: " + auto.GetReloadSpeed() + " -> " + (auto.GetReloadSpeed() - 0.05);
                UI.S.autoDamage.text = "Damage: " + auto.GetDamage() + " -> " + (auto.GetDamage() + 2f);
                weapon.weapons.Add(auto);
                weapon.AddRifleAmmo(30);
            }
        }
        else
        {
            audioSourceBuing.Play();
            weapon.weapons.Find(gun => gun.GetType() == Weapon.WeaponType.eAutomatic).Upgrade();
        }
    }

    public void BuyStealFency()
    {
        Debug.Log("Kupuję kubuje płot stalowy");
        DefensiveObject baricadeObject = DefensiveObjectsArray[1].prefabs.GetComponent<DefensiveObject>();
        if (baricadeObject.Upgrade())
        {
            audioSourceBuing.Play();
            UI.S.baricadeUpgrade.text = "Endurance: " + baricadeObject.maxHP + " -> " + (baricadeObject.maxHP + baricadeObject.bonusHealtOnLevel);
            UI.S.baricadeCost.text = "Upgrade: " + baricadeObject.upgradePrice + "$";
        }
        if (baricadeObject.currentLevel == baricadeObject.maxLevel)
        {
            UI.S.baricadeUpgrade.text = "Endurance: " + baricadeObject.maxHP;
            UI.S.baricadeCost.text = "MAX LEVEL REACHED";
        }
    }

    public void BuySpikes()
    {
        Debug.Log("Kupuję kupuje kolce");
        DefensiveSpikes spikesObject = DefensiveObjectsArray[0].prefabs.GetComponent<DefensiveSpikes>();
        if(spikesObject.Upgrade())
        {
            audioSourceBuing.Play();
            UI.S.spikeUpgrade.text = "Damage: " + spikesObject.damageEnemy + " -> " + (spikesObject.damageEnemy + spikesObject.damageUpgrade) + "\n" +
                "Endurance: " + spikesObject.health + " -> " + (spikesObject.health + spikesObject.healthUpgrade);
            UI.S.spikeCost.text = "Upgrade: " + spikesObject.upgradePrice + "$";
        }
        if(spikesObject.currentLevel == spikesObject.maxLevel)
        {
            UI.S.spikeUpgrade.text = "Damage: " + spikesObject.damageEnemy + "\n" +
                "Endurance: " + spikesObject.health;
            UI.S.spikeCost.text = "MAX LEVEL REACHED";
        }
    }

    private void FindWeaponObject()
    {
        GameObject gameObject = GameObject.FindGameObjectWithTag("Weapon");
        weapon = gameObject.GetComponent<Weapon>();
    }
}
