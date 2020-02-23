﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI : MonoBehaviour
{
    public Slider hp, reloadingProgress;
    public TextMeshProUGUI ammo, gold, weaponName, wave, pistolUpgrade, semiUpgrade, autoUpgrade, hpPercentage, hpUpgrade;
    public TextMeshProUGUI gunReloadTime, gunDamage, semiReloadTime, semiDamage, autoReloadTime, autoDamage;

        
    public Button buyAmmoButton;
    public TextMeshProUGUI bytAmmoText;

    private Player _player;
    private Weapon _weapon;

    private void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        GameObject weapon = GameObject.FindGameObjectWithTag("Weapon");
        _player = player.GetComponentInChildren<Player>();
        _weapon = weapon.GetComponentInChildren<Weapon>();
    }

    private void Update()
    {
        setTexts();
        setHP();
        if(!_weapon.isReloadingActive()) reloadingProgress.maxValue = _weapon.GetWeapon().GetReloadSpeed();
    }

    private void FixedUpdate()
    {
        if (_weapon.isReloadingActive()) ShowReloadingBar();
    }

    private void setTexts()
    {
        if(_weapon.GetWeapon().GetType() != Weapon.WeaponType.ePistol) ammo.text = _weapon.GetWeapon().GetCurrentAmmo() + "/" + _weapon.GetWeapon().GetCapacity() + "  [" + _weapon.GetWeapon().GetAmmo() + "]";
        else ammo.text = _weapon.GetWeapon().GetCurrentAmmo() + "/" + _weapon.GetWeapon().GetCapacity();
        weaponName.text = _weapon.GetWeapon().GetName();
        gold.text = "Gold: " + Main.S.gold;
        wave.text = "Wave: " + (int)(Main.S.currentLevel+1);
        float hpPercent = (float)_player.GetHP() / (float)_player.GetMaxHP() * 100;
        hpPercentage.text = (int)hpPercent + "%";

        if(!Main.S.isEnableToShoot)
        {
            pistolUpgrade.text = "Cost: " + _weapon.weapons.Find(gun => gun.GetType() == Weapon.WeaponType.ePistol).GetMoneyForUpgrade()+"$";

            Weapon.SemiAutomatic semi = (Weapon.SemiAutomatic)_weapon.weapons.Find(w => w.GetType() == Weapon.WeaponType.eSemiAutomatic);
            if (semi == null)
            {
                semi = new Weapon.SemiAutomatic();
                semiUpgrade.text = "Cost: " + semi.GetBuyingPrice() + "$\n \n ";
            }
            else semiUpgrade.text = "Cost: " + _weapon.weapons.Find(gun => gun.GetType() == Weapon.WeaponType.eSemiAutomatic).GetMoneyForUpgrade() + "$";

            Weapon.Automatic auto = (Weapon.Automatic)_weapon.weapons.Find(w => w.GetType() == Weapon.WeaponType.eAutomatic);
            if (auto == null)
            {
                auto = new Weapon.Automatic();
                autoUpgrade.text = "Cost: " + auto.GetBuyingPrice() + "$\n \n ";
            }
            else autoUpgrade.text = "Cost: " + _weapon.weapons.Find(gun => gun.GetType() == Weapon.WeaponType.eAutomatic).GetMoneyForUpgrade() + "$";

            //Sklep - Panel HP
            if (_player.GetHpLevel() == _player.GetMaxHpLevel())
            {
                hpUpgrade.text = "Health level: " + Player.S.GetHpLevel() + " / " + Player.S.GetMaxHpLevel() +"\n" +
                    " " + 
                    "\nCost: " + Player.S.GetHpUpgradeCost() + "$\n ";
               // hpUpgrade.text = "25$\nHEAL";
            }
            else
            {
                hpUpgrade.text = "Health level: " + Player.S.GetHpLevel() + " / " + Player.S.GetMaxHpLevel() + "\n" +
                    Player.S.GetMaxHP() + " -> " + (Player.S.GetMaxHP() + Player.S.GetHpBonusPerLevel()) +
                    "\nCost: " + Player.S.GetHpUpgradeCost() + "$\n ";
               // hpUpgrade.text = "25$\nHEAL";
            }

            //Sklep - Panel Ammo
            if(_weapon.GetWeapon().GetType() == Weapon.WeaponType.ePistol)
            {
                bytAmmoText.text = "+" + Main.S.GetComponent<Shop>().amunationPiecesToBuy + "\nCost: " + Main.S.GetComponent<Shop>().amunationPrice + "$\n ";
                buyAmmoButton.interactable = false;
                buyAmmoButton.GetComponent<Image>().enabled = false;
                bytAmmoText.text += "You can not buy ammunation for " + _weapon.GetWeapon().GetName();
            }
            else
            {
                bytAmmoText.text = "+" + Main.S.GetComponent<Shop>().amunationPiecesToBuy + "\nCost: " + Main.S.GetComponent<Shop>().amunationPrice + "$\n ";
                buyAmmoButton.interactable = true;
                buyAmmoButton.GetComponent<Image>().enabled = true;
                bytAmmoText.text += "\n \n ";
            }

            gunReloadTime.text = "Reload Spd.: " + _weapon.weapons.Find(gun => gun.GetType() == Weapon.WeaponType.ePistol).GetReloadSpeed() + " -> " + (_weapon.weapons.Find(gun => gun.GetType() == Weapon.WeaponType.ePistol).GetReloadSpeed() - 0.05f);
            gunDamage.text = "Damage: " + _weapon.weapons.Find(gun => gun.GetType() == Weapon.WeaponType.ePistol).GetDamage() + " -> " + (_weapon.weapons.Find(gun => gun.GetType() == Weapon.WeaponType.ePistol).GetDamage() + 1);

            Weapon.SemiAutomatic semic = (Weapon.SemiAutomatic)_weapon.weapons.Find(w => w.GetType() == Weapon.WeaponType.eSemiAutomatic);
            if (semic != null)
            {
                semiReloadTime.text = "Reload Spd.: " + _weapon.weapons.Find(gun => gun.GetType() == Weapon.WeaponType.eSemiAutomatic).GetReloadSpeed() + " -> " + (_weapon.weapons.Find(gun => gun.GetType() == Weapon.WeaponType.eSemiAutomatic).GetReloadSpeed() - 0.05f);
                semiDamage.text = "Damage: " + _weapon.weapons.Find(gun => gun.GetType() == Weapon.WeaponType.eSemiAutomatic).GetDamage() + " -> " + (_weapon.weapons.Find(gun => gun.GetType() == Weapon.WeaponType.eSemiAutomatic).GetDamage() + 1.5f);
            }
            else
            {
                semiReloadTime.text = "";
                semiDamage.text = "";
            }

            Weapon.Automatic autom = (Weapon.Automatic)_weapon.weapons.Find(w => w.GetType() == Weapon.WeaponType.eAutomatic);
            if (autom != null)
            {
                autoReloadTime.text = "Reload Spd.: " + _weapon.weapons.Find(gun => gun.GetType() == Weapon.WeaponType.eAutomatic).GetReloadSpeed() + " -> " + (_weapon.weapons.Find(gun => gun.GetType() == Weapon.WeaponType.eAutomatic).GetReloadSpeed() - 0.05f);
                autoDamage.text = "Damage: " + _weapon.weapons.Find(gun => gun.GetType() == Weapon.WeaponType.eAutomatic).GetDamage() + " -> " + (_weapon.weapons.Find(gun => gun.GetType() == Weapon.WeaponType.eAutomatic).GetDamage() + 2);
            }
            else
            {
                autoReloadTime.text = "";
                autoDamage.text = "";
            }
        }


    }

    private void setHP()
    {
        hp.value = _player.GetHP();
        hp.maxValue = _player.GetMaxHP();
    }

    public void ShowReloadingBar()
    {
        reloadingProgress.gameObject.SetActive(true);

        if (reloadingProgress.value < reloadingProgress.maxValue)
            reloadingProgress.value += 0.01f;
        else
        {
            reloadingProgress.value = 0;
            reloadingProgress.gameObject.SetActive(false);
            _weapon.turnOffReloading();
        }
    }
}
