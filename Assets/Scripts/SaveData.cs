using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    //Player
    public int health;
    public int maxHP;
    public int hpLevel;
    public int maxHpLevel;
    public int hpBonusPerLevel;
    public int cost;
    //Global
    public int gold;
    public int currentLevel;
    //______WEAPONS________
    public int riffleAmmo;
    //Pistolet
    public int PisCurrentAmmo;
    public int PisAmmo;
    public int PisCapacity;
    public int PisLevel;
    public int PisFireRate;
    public int PisMoneyForUpgrade;
    public int PisBuyingPrice;
    public float PisReloadSpeed;
    public float PisDamage;
    //Półautomat
    public int SemiCurrentAmmo;
    public int SemiAmmo;
    public int SemiCapacity;
    public int SemiLevel;
    public int SemiFireRate;
    public int SemiMoneyForUpgrade;
    public int SemiBuyingPrice;
    public float SemiReloadSpeed;
    public float SemiDamage;
    public bool isSemiExist = false;
    //Automat
    public int AutoCurrentAmmo;
    public int AutoAmmo;
    public int AutoCapacity;
    public int AutoLevel;
    public int AutoFireRate;
    public int AutoMoneyForUpgrade;
    public int AutoBuyingPrice;
    public float AutoReloadSpeed;
    public float AutoDamage;
    public bool isAutoExist = false;
    //Rifle
    public int RifleCurrentAmmo;
    public int RifleAmmo;
    public int RifleCapacity;
    public int RifleLevel;
    public int RifleFireRate;
    public int RifleMoneyForUpgrade;
    public int RifleBuyingPrice;
    public float RifleReloadSpeed;
    public float RifleDamage;
    public bool isRifleExist = false;
    //______DEFENSIVE________
    //Płot
    public int FencyCurrentLevel;
    public float FencyHealth;
    public int FencyUpgradePrice;
    //Kolce
    public int SpikeCurrentLevel;
    public float SpikeHealth;
    public float SpikeDamageEnemy;
    public int SpikeUpgradePrice;
    public float SpikeTakeDamage;

    public SaveData(int HP, Main main, Shop shop, Weapon _weapon)
    {
        //Player
        health = Player.S.GetHP();
        maxHP = Player.S.GetMaxHP();
        hpLevel = Player.S.GetHpLevel();
        maxHpLevel = Player.S.GetMaxHpLevel();
        hpBonusPerLevel = Player.S.GetHpBonusPerLevel();
        cost = Player.S.GetCost();
        //Global
        gold = Main.S.gold;
        currentLevel = Main.S.currentLevel;
        //______WEAPONS________
        riffleAmmo = _weapon.GetRifleAmmo();
        //Pistolet
        PisCurrentAmmo = _weapon.weapons.Find(gun => gun.GetType() == Weapon.WeaponType.ePistol).GetCurrentAmmo();
        PisAmmo = _weapon.weapons.Find(gun => gun.GetType() == Weapon.WeaponType.ePistol).GetAmmo();
        PisCapacity = _weapon.weapons.Find(gun => gun.GetType() == Weapon.WeaponType.ePistol).GetCapacity();
        PisLevel = _weapon.weapons.Find(gun => gun.GetType() == Weapon.WeaponType.ePistol).GetLevel();
        PisFireRate = _weapon.weapons.Find(gun => gun.GetType() == Weapon.WeaponType.ePistol).GetFireRate();
        PisMoneyForUpgrade = _weapon.weapons.Find(gun => gun.GetType() == Weapon.WeaponType.ePistol).GetMoneyForUpgrade();
        PisBuyingPrice = _weapon.weapons.Find(gun => gun.GetType() == Weapon.WeaponType.ePistol).GetBuyingPrice();
        PisReloadSpeed = _weapon.weapons.Find(gun => gun.GetType() == Weapon.WeaponType.ePistol).GetReloadSpeed();
        PisDamage = _weapon.weapons.Find(gun => gun.GetType() == Weapon.WeaponType.ePistol).GetDamage();
        //Półautomat
        if (_weapon.weapons.Find(gun => gun.GetType() == Weapon.WeaponType.eSemiAutomatic) != null)
        {
            SemiCurrentAmmo = _weapon.weapons.Find(gun => gun.GetType() == Weapon.WeaponType.eSemiAutomatic).GetCurrentAmmo();
            SemiAmmo = _weapon.weapons.Find(gun => gun.GetType() == Weapon.WeaponType.eSemiAutomatic).GetAmmo();
            SemiCapacity = _weapon.weapons.Find(gun => gun.GetType() == Weapon.WeaponType.eSemiAutomatic).GetCapacity();
            SemiLevel = _weapon.weapons.Find(gun => gun.GetType() == Weapon.WeaponType.eSemiAutomatic).GetLevel();
            SemiFireRate = _weapon.weapons.Find(gun => gun.GetType() == Weapon.WeaponType.eSemiAutomatic).GetFireRate();
            SemiMoneyForUpgrade = _weapon.weapons.Find(gun => gun.GetType() == Weapon.WeaponType.eSemiAutomatic).GetMoneyForUpgrade();
            SemiBuyingPrice = _weapon.weapons.Find(gun => gun.GetType() == Weapon.WeaponType.eSemiAutomatic).GetBuyingPrice();
            SemiReloadSpeed = _weapon.weapons.Find(gun => gun.GetType() == Weapon.WeaponType.eSemiAutomatic).GetReloadSpeed();
            SemiDamage = _weapon.weapons.Find(gun => gun.GetType() == Weapon.WeaponType.eSemiAutomatic).GetDamage();
            isSemiExist = true;
        }
        
        //Automat
        if(_weapon.weapons.Find(gun => gun.GetType() == Weapon.WeaponType.eAutomatic) != null)
        {
            AutoCurrentAmmo = _weapon.weapons.Find(gun => gun.GetType() == Weapon.WeaponType.eAutomatic).GetCurrentAmmo();
            AutoAmmo = _weapon.weapons.Find(gun => gun.GetType() == Weapon.WeaponType.eAutomatic).GetAmmo();
            AutoCapacity = _weapon.weapons.Find(gun => gun.GetType() == Weapon.WeaponType.eAutomatic).GetCapacity();
            AutoLevel = _weapon.weapons.Find(gun => gun.GetType() == Weapon.WeaponType.eAutomatic).GetLevel();
            AutoFireRate = _weapon.weapons.Find(gun => gun.GetType() == Weapon.WeaponType.eAutomatic).GetFireRate();
            AutoMoneyForUpgrade = _weapon.weapons.Find(gun => gun.GetType() == Weapon.WeaponType.eAutomatic).GetMoneyForUpgrade();
            AutoBuyingPrice = _weapon.weapons.Find(gun => gun.GetType() == Weapon.WeaponType.eAutomatic).GetBuyingPrice();
            AutoReloadSpeed = _weapon.weapons.Find(gun => gun.GetType() == Weapon.WeaponType.eAutomatic).GetReloadSpeed();
            AutoDamage = _weapon.weapons.Find(gun => gun.GetType() == Weapon.WeaponType.eAutomatic).GetDamage();
            isAutoExist = true;
        }

        //Rifle
        if (_weapon.weapons.Find(gun => gun.GetType() == Weapon.WeaponType.eSniperRifle) != null)
        {
            RifleCurrentAmmo =       _weapon.weapons.Find(gun => gun.GetType() == Weapon.WeaponType.eSniperRifle).GetCurrentAmmo();
            RifleAmmo =              _weapon.weapons.Find(gun => gun.GetType() == Weapon.WeaponType.eSniperRifle).GetAmmo();
            RifleCapacity =          _weapon.weapons.Find(gun => gun.GetType() == Weapon.WeaponType.eSniperRifle).GetCapacity();
            RifleLevel =             _weapon.weapons.Find(gun => gun.GetType() == Weapon.WeaponType.eSniperRifle).GetLevel();
            RifleFireRate =          _weapon.weapons.Find(gun => gun.GetType() == Weapon.WeaponType.eSniperRifle).GetFireRate();
            RifleMoneyForUpgrade =   _weapon.weapons.Find(gun => gun.GetType() == Weapon.WeaponType.eSniperRifle).GetMoneyForUpgrade();
            RifleBuyingPrice =       _weapon.weapons.Find(gun => gun.GetType() == Weapon.WeaponType.eSniperRifle).GetBuyingPrice();
            RifleReloadSpeed =       _weapon.weapons.Find(gun => gun.GetType() == Weapon.WeaponType.eSniperRifle).GetReloadSpeed();
            RifleDamage =            _weapon.weapons.Find(gun => gun.GetType() == Weapon.WeaponType.eSniperRifle).GetDamage();
            isRifleExist = true;
        }

        //______DEFENSIVE________
        //Płot
        FencyCurrentLevel = shop.DefensiveObjectsArray[1].prefabs.GetComponent<DefensiveObject>().currentLevel;
        FencyHealth = shop.DefensiveObjectsArray[1].prefabs.GetComponent<DefensiveObject>().health;
        FencyUpgradePrice = shop.DefensiveObjectsArray[1].prefabs.GetComponent<DefensiveObject>().upgradePrice;
        //Kolce
        SpikeCurrentLevel = shop.DefensiveObjectsArray[0].prefabs.GetComponent<DefensiveSpikes>().currentLevel;
        SpikeHealth = shop.DefensiveObjectsArray[0].prefabs.GetComponent<DefensiveSpikes>().health;
        SpikeDamageEnemy = shop.DefensiveObjectsArray[0].prefabs.GetComponent<DefensiveSpikes>().damageEnemy;
        SpikeUpgradePrice = shop.DefensiveObjectsArray[0].prefabs.GetComponent<DefensiveSpikes>().upgradePrice;
        SpikeTakeDamage = shop.DefensiveObjectsArray[0].prefabs.GetComponent<DefensiveSpikes>().takeDamage;
    }
}