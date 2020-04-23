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
    public int rifleAmmoPrice = 5;
    public int sniperAmmoPrice = 5;
    public int rifleAmmoPiecesToBuy = 25;
    public int sniperAmmoPiecesToBuy = 10;
    public GameObject _infoText; //← obiekt z napisami accept/cancel
    public Color greenToBuy;
    public Color redToBuy;
    public Color yellowToBuy;
    public Transform objectAnchor;

    [Header("Definiowane dynamicznie")]
    public bool isActive = false;
    private bool _isisMovingDefensiveObjects = false;
    private float _timer;
    private Weapon weapon;
    private GameObject _defensiveObjectGhost;
    private GameObject _defensiveObject;
    private int _defensiveObjectsNumber;
    private List<GameObject> shopPanelsArray = new List<GameObject>();
    private AudioSource audioSourceBuing;

    private void Start()
    {
        LoadDefensiveObjectsStatus();

        FindWeaponObject();
        Transform parent = Main.S.shopPanel.transform;
        foreach(Transform obj in parent)
        {
            if(obj.tag == "ShopPanelSection")
            {
                shopPanelsArray.Add(obj.gameObject);
            }
        }

        //Czy gracz posiada juz broń?
        if (weapon.weapons.Find(gun => gun.GetType() == Weapon.WeaponType.eSemiAutomatic) != null || weapon.weapons.Find(gun => gun.GetType() == Weapon.WeaponType.eAutomatic) != null || weapon.weapons.Find(gun => gun.GetType() == Weapon.WeaponType.eSniperRifle) != null)
            this.UnlockWeaponSwapUI();

        audioSourceBuing = shopPanel.GetComponent<AudioSource>();
    }

    //_______________DefensiveObject______________________

    private void MovingDefensiveObjects(int objectNumber)
    {
        _infoText.SetActive(true);//← aktywacja napisów accept/cancel
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
        _infoText.SetActive(false);//← ukrywanie napisów accept/cancel
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

            GameObject go = Instantiate(_defensiveObject, _defensiveObjectGhost.transform.position, _defensiveObjectGhost.transform.rotation);
            go.transform.SetParent(objectAnchor, true);
            SoundsMenager.S.PlayDefensePlaced();
            _defensiveObject.SetActive(true);
            ResetInfoDefensiveObject();
        }

        //↓ Wyświetlanie napisów "accept/cancel" gdy jest wybrany jakiś obiekt
        if (_isisMovingDefensiveObjects)
        {
            _infoText.transform.position = Input.mousePosition;
        }

        if ((Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Mouse1)) && _isisMovingDefensiveObjects)
        {
            ResetInfoDefensiveObject();
        }

        if (isActive)
        {
            UpdateButtonsStatus();

            if (_isisMovingDefensiveObjects)
                ResetInfoDefensiveObject();

            _timer -= Time.deltaTime;
            UpdateTimerText();

            if (_timer < 0)
                isActive = false;
        }
    }
    private void UpdateButtonsStatus()
    {
        foreach(GameObject gameObject in shopPanelsArray)
        {
            Image image = gameObject.GetComponent<Image>();
            if(gameObject.name == "BuyAmmo")
            {
                if (Main.S.gold >= rifleAmmoPrice && weapon.GetWeapon().GetType() != Weapon.WeaponType.ePistol)
                    image.color = greenToBuy;
                else
                    image.color = redToBuy;

            }
            else if (gameObject.name == "BuyHealth")
            {
                if(Main.S.gold >= Player.S.GetHpUpgradeCost())
                    image.color = greenToBuy;
                else
                    image.color = redToBuy;
                if (Player.S.GetMaxHpLevel() == Player.S.GetHpLevel())
                    image.color = yellowToBuy;
            }
            else if(gameObject.name == "BuyGun")
            {
                Weapon.Pistol pistol = (Weapon.Pistol)weapon.weapons.Find(w => w.GetType() == Weapon.WeaponType.ePistol);
                if (Main.S.gold >= pistol.GetMoneyForUpgrade())
                    image.color = greenToBuy;
                else
                    image.color = redToBuy;
                if (pistol.GetMaxLevel() == pistol.GetLevel())
                    image.color = yellowToBuy;
            }
            else if(gameObject.name == "BuySemiAutomaticGun")
            {
                Weapon.SemiAutomatic semi = (Weapon.SemiAutomatic)weapon.weapons.Find(w => w.GetType() == Weapon.WeaponType.eSemiAutomatic);
                if (semi == null)
                {
                    semi = new Weapon.SemiAutomatic();
                    if (Main.S.gold >= semi.GetBuyingPrice())
                        image.color = greenToBuy;
                    else
                        image.color = redToBuy;
                } 
                else
                {
                    if (Main.S.gold >= semi.GetMoneyForUpgrade())
                        image.color = greenToBuy;
                    else
                        image.color = redToBuy;
                }
                
                if (semi.GetMaxLevel() == semi.GetLevel())
                    image.color = yellowToBuy;
            }
            else if(gameObject.name == "BuyAutomaticGun")
            {
                Weapon.Automatic auto = (Weapon.Automatic)weapon.weapons.Find(w => w.GetType() == Weapon.WeaponType.eAutomatic);
                if (auto == null)
                {
                    auto = new Weapon.Automatic();
                    if (Main.S.gold >= auto.GetBuyingPrice())
                        image.color = greenToBuy;
                    else
                        image.color = redToBuy;
                }
                else
                {
                    if (Main.S.gold >= auto.GetMoneyForUpgrade())
                        image.color = greenToBuy;
                    else
                        image.color = redToBuy;
                }
                
                if (auto.GetMaxLevel() == auto.GetLevel())
                    image.color = yellowToBuy;
            }
            else if(gameObject.name == "BuyFencyUpgrade")
            {                
                DefensiveObject baricadeObject = DefensiveObjectsArray[1].prefabs.GetComponent<DefensiveObject>(); //PŁOT
                if (Main.S.gold >= baricadeObject.upgradePrice)
                    image.color = greenToBuy;
                else
                    image.color = redToBuy;
                if (baricadeObject.maxLevel == baricadeObject.currentLevel)
                    image.color = yellowToBuy;
            }
            else if(gameObject.name == "BuySpikeUpgrade")
            {
                DefensiveSpikes spikesObject = DefensiveObjectsArray[0].prefabs.GetComponent<DefensiveSpikes>(); //KOLCE
                if (Main.S.gold >= spikesObject.upgradePrice)
                    image.color = greenToBuy;
                else
                    image.color = redToBuy;
                if (spikesObject.maxLevel == spikesObject.currentLevel)
                    image.color = yellowToBuy;
            }
            else if (gameObject.name == "BuySniperRifle")
            {
                Weapon.SniperRifle rifle = (Weapon.SniperRifle)weapon.weapons.Find(w => w.GetType() == Weapon.WeaponType.eSniperRifle);
                if (rifle == null)
                {
                    rifle = new Weapon.SniperRifle();
                    if (Main.S.gold >= rifle.GetBuyingPrice())
                        image.color = greenToBuy;
                    else
                        image.color = redToBuy;
                }
                else
                {
                    if (Main.S.gold >= rifle.GetMoneyForUpgrade())
                        image.color = greenToBuy;
                    else
                        image.color = redToBuy;
                }

                if (rifle.GetMaxLevel() == rifle.GetLevel())
                    image.color = yellowToBuy;
            }
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
        if (Main.S.gold >= rifleAmmoPrice && weapon.GetWeapon().GetType() != Weapon.WeaponType.ePistol && weapon.GetWeapon().GetType() != Weapon.WeaponType.eSniperRifle)
        {
            Debug.Log("KUPUJE AMMO");
            audioSourceBuing.Play();
            Main.S.gold -= rifleAmmoPrice;
            UI.S.gold.text = "Gold: " + Main.S.gold;
            if (weapon == null) FindWeaponObject();
            weapon.AddRifleAmmo(rifleAmmoPiecesToBuy);
            UI.S.ammo.text = weapon.GetWeapon().GetCurrentAmmo() + "/" + weapon.GetWeapon().GetCapacity() + "  [" + weapon.GetRifleAmmo() + "]";
        }
        else if (weapon.GetWeapon().GetType() == Weapon.WeaponType.eSniperRifle && Main.S.gold >= sniperAmmoPrice)
        {
            Debug.Log("KUPUJE AMMO");
            audioSourceBuing.Play();
            Main.S.gold -= sniperAmmoPrice;
            UI.S.gold.text = "Gold: " + Main.S.gold;
            if (weapon == null) FindWeaponObject();
            weapon.AddSniperAmmo(sniperAmmoPiecesToBuy);
            UI.S.ammo.text = weapon.GetWeapon().GetCurrentAmmo() + "/" + weapon.GetWeapon().GetCapacity() + "  [" + weapon.GetSniperAmmo() + "]";
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
        if (weapon.weapons.Find(w => w.GetType() == Weapon.WeaponType.ePistol).GetMaxLevel() > weapon.weapons.Find(w => w.GetType() == Weapon.WeaponType.ePistol).GetLevel())
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
                UnlockWeaponSwapUI();
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
            if(semi.GetMaxLevel() > semi.GetLevel()) audioSourceBuing.Play();
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
                UnlockWeaponSwapUI();
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
            if (auto.GetMaxLevel() > auto.GetLevel()) audioSourceBuing.Play();
            weapon.weapons.Find(gun => gun.GetType() == Weapon.WeaponType.eAutomatic).Upgrade();
        }
    }

    public void BuySniperRifle()
    {
        Weapon.SniperRifle snip = (Weapon.SniperRifle)weapon.weapons.Find(w => w.GetType() == Weapon.WeaponType.eSniperRifle);

        if (snip == null)
        {
            snip = new Weapon.SniperRifle();
            if (Main.S.gold >= snip.GetBuyingPrice())
            {
                audioSourceBuing.Play();
                Main.S.gold -= snip.GetBuyingPrice();
                UI.S.gold.text = "Gold: " + Main.S.gold;
                UI.S.sniperUpgrade.text = "Cost: " + snip.GetMoneyForUpgrade() + "$";
                UI.S.sniperReloadTime.text = "Reload Spd.: " + snip.GetReloadSpeed() + " -> " + (snip.GetReloadSpeed() - 0.05);
                UI.S.sniperDamage.text = "Damage: " + snip.GetDamage() + " -> " + (snip.GetDamage() + 2f);
                weapon.weapons.Add(snip);
                weapon.AddSniperAmmo(10);
            }
        }
        else
        {
            if (snip.GetMaxLevel() > snip.GetLevel()) audioSourceBuing.Play();
            weapon.weapons.Find(gun => gun.GetType() == Weapon.WeaponType.eSniperRifle).Upgrade();
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

    private void UnlockWeaponSwapUI()
    {
        UI.S.weaponName.transform.GetChild(0).gameObject.SetActive(true);
        UI.S.weaponName.transform.GetChild(1).gameObject.SetActive(true);
    }

    private void LoadDefensiveObjectsStatus()
    {
        DefensiveSpikes spikesObject = DefensiveObjectsArray[0].prefabs.GetComponent<DefensiveSpikes>(); //KOLCE
        DefensiveObject baricadeObject = DefensiveObjectsArray[1].prefabs.GetComponent<DefensiveObject>(); //PŁOT
        if(!SaveSystem.isGameLoaded)
        {
            spikesObject.Initialize();
            baricadeObject.Initialize();
        }
        

        //BARRICADE
        UI.S.baricadeUpgrade.text = "Endurance: " + baricadeObject.maxHP + " -> " + (baricadeObject.maxHP + baricadeObject.bonusHealtOnLevel);
        UI.S.baricadeCost.text = "Upgrade: " + baricadeObject.upgradePrice + "$";
        if (baricadeObject.currentLevel == baricadeObject.maxLevel)
        {
            UI.S.baricadeUpgrade.text = "Endurance: " + baricadeObject.maxHP;
            UI.S.baricadeCost.text = "MAX LEVEL REACHED";
        }
        //SPIKES 
        UI.S.spikeUpgrade.text = "Damage: " + spikesObject.damageEnemy + " -> " + (spikesObject.damageEnemy + spikesObject.damageUpgrade) + "\n" +
                "Endurance: " + spikesObject.health + " -> " + (spikesObject.health + spikesObject.healthUpgrade);
        UI.S.spikeCost.text = "Upgrade: " + spikesObject.upgradePrice + "$";
        if (spikesObject.currentLevel == spikesObject.maxLevel)
        {
            UI.S.spikeUpgrade.text = "Damage: " + spikesObject.damageEnemy + "\n" +
                "Endurance: " + spikesObject.health;
            UI.S.spikeCost.text = "MAX LEVEL REACHED";
        }
    }
}
