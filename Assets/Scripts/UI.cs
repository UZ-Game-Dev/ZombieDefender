﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI : MonoBehaviour
{
    public Slider hp, reloadingProgress;
    public TextMeshProUGUI ammo, gold, weaponName, wave, pistolUpgrade, semiUpgrade, autoUpgrade, sniperUpgrade, hpAmount, hpUpgrade, baricadeUpgrade, baricadeCost,
        spikeUpgrade, spikeCost;
    public TextMeshProUGUI gunReloadTime, gunDamage, semiReloadTime, semiDamage, autoReloadTime, autoDamage, sniperReloadTime, sniperDamage;
        
    public Button buyAmmoButton;
    public Image hpColor;
    public TextMeshProUGUI buyAmmoText;

    private Player _player;
    private Weapon _weapon;

    public static UI S;

    private void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        GameObject weapon = GameObject.FindGameObjectWithTag("Weapon");
        _player = player.GetComponentInChildren<Player>();
        _weapon = weapon.GetComponentInChildren<Weapon>();
        S = this;

        //TEXTS

        if(_weapon.GetWeapon().GetType() != Weapon.WeaponType.ePistol) 
            ammo.text = _weapon.GetWeapon().GetCurrentAmmo() + "/" + _weapon.GetWeapon().GetCapacity() + "  [" + _weapon.GetRifleAmmo() + "]";
        else 
            ammo.text = _weapon.GetWeapon().GetCurrentAmmo() + "/" + _weapon.GetWeapon().GetCapacity();
        weaponName.text = _weapon.GetWeapon().GetName();
       
        gold.text = "Gold: " + Main.S.gold;
        wave.text = "Wave: " + (int)(Main.S.waveCounter);
        hpAmount.text = _player.GetHP().ToString();

        Weapon.Pistol pistol = (Weapon.Pistol)_weapon.weapons.Find(gun => gun.GetType() == Weapon.WeaponType.ePistol);
        pistolUpgrade.text = "Cost: " + pistol.GetMoneyForUpgrade() + "$";

        Weapon.SemiAutomatic semi = (Weapon.SemiAutomatic)_weapon.weapons.Find(w => w.GetType() == Weapon.WeaponType.eSemiAutomatic);
        if (semi == null)
        {
            semi = new Weapon.SemiAutomatic();
            semiUpgrade.text = "Cost: " + semi.GetBuyingPrice() + "$\n \n ";
        }
        else
        {
            semiUpgrade.text = "Cost: " + _weapon.weapons.Find(gun => gun.GetType() == Weapon.WeaponType.eSemiAutomatic).GetMoneyForUpgrade() + "$";
            if (semi.GetLevel() == semi.GetMaxLevel())
            {
                UI.S.semiUpgrade.text = "MAX LEVEL REACHED";
                UI.S.semiReloadTime.text = "";
                UI.S.semiDamage.text = "";
            }
        }

        Weapon.Automatic auto = (Weapon.Automatic)_weapon.weapons.Find(w => w.GetType() == Weapon.WeaponType.eAutomatic);
        if (auto == null)
        {
            auto = new Weapon.Automatic();
            autoUpgrade.text = "Cost: " + auto.GetBuyingPrice() + "$\n \n ";
        }
        else
        {
            autoUpgrade.text = "Cost: " + _weapon.weapons.Find(gun => gun.GetType() == Weapon.WeaponType.eAutomatic).GetMoneyForUpgrade() + "$";
           
        }

        Weapon.SniperRifle snip = (Weapon.SniperRifle)_weapon.weapons.Find(w => w.GetType() == Weapon.WeaponType.eSniperRifle);
        if (snip == null)
        {
            snip = new Weapon.SniperRifle();
            sniperUpgrade.text = "Cost: " + snip.GetBuyingPrice() + "$\n \n ";
        }
        else autoUpgrade.text = "Cost: " + _weapon.weapons.Find(gun => gun.GetType() == Weapon.WeaponType.eSniperRifle).GetMoneyForUpgrade() + "$";

        //Sklep - Panel HP
        if (_player.GetHpLevel() == _player.GetMaxHpLevel())
        {
            hpUpgrade.text = "Health level: " + Player.S.GetHpLevel() + " / " + Player.S.GetMaxHpLevel() + "\n" +
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
        if (_weapon.GetWeapon().GetType() == Weapon.WeaponType.ePistol)
        {
            buyAmmoText.text = "+" + Main.S.GetComponent<Shop>().rifleAmmoPiecesToBuy + "\nCost: " + Main.S.GetComponent<Shop>().rifleAmmoPrice + "$\n ";
            buyAmmoButton.interactable = false;
            buyAmmoButton.GetComponent<Image>().enabled = false;
            buyAmmoText.text += "You can not buy ammunation for " + _weapon.GetWeapon().GetName();
        }
        else if(_weapon.GetWeapon().GetType() == Weapon.WeaponType.eSniperRifle)
        {
            buyAmmoText.text = "\nAmmo for Sniper\n+" + Main.S.GetComponent<Shop>().rifleAmmoPiecesToBuy + "\nCost: " + Main.S.GetComponent<Shop>().rifleAmmoPrice + "$\n ";
            buyAmmoButton.interactable = true;
            buyAmmoButton.GetComponent<Image>().enabled = true;
            buyAmmoText.text += "\n \n ";
        }
        else
        {
            buyAmmoText.text = "\nAmmo for machine guns\n+" + Main.S.GetComponent<Shop>().rifleAmmoPiecesToBuy + "\nCost: " + Main.S.GetComponent<Shop>().rifleAmmoPrice + "$\n ";
            buyAmmoButton.interactable = true;
            buyAmmoButton.GetComponent<Image>().enabled = true;
            buyAmmoText.text += "\n \n ";
        }

        gunReloadTime.text = "Reload Spd.: " + _weapon.weapons.Find(gun => gun.GetType() == Weapon.WeaponType.ePistol).GetReloadSpeed() + " -> " + (_weapon.weapons.Find(gun => gun.GetType() == Weapon.WeaponType.ePistol).GetReloadSpeed() - 0.05f);
        gunDamage.text = "Damage: " + _weapon.weapons.Find(gun => gun.GetType() == Weapon.WeaponType.ePistol).GetDamage() + " -> " + (_weapon.weapons.Find(gun => gun.GetType() == Weapon.WeaponType.ePistol).GetDamage() + 1);
        if (pistol.GetLevel() == pistol.GetMaxLevel())
        {
            UI.S.pistolUpgrade.text = "MAX LEVEL REACHED";
            UI.S.gunReloadTime.text = "";
            UI.S.gunDamage.text = "";
        }


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
            if (auto.GetLevel() == auto.GetMaxLevel())
            {
                UI.S.autoUpgrade.text = "MAX LEVEL REACHED";
                UI.S.autoReloadTime.text = "";
                UI.S.autoDamage.text = "";
            }
        }
        else
        {
            autoReloadTime.text = "";
            autoDamage.text = "";
        }

        Weapon.SniperRifle sniper = (Weapon.SniperRifle)_weapon.weapons.Find(w => w.GetType() == Weapon.WeaponType.eSniperRifle);
        if (sniper != null)
        {
            sniperReloadTime.text = "Reload Spd.: " + sniper.GetReloadSpeed() + " -> " + (sniper.GetReloadSpeed() - 0.05f);
            sniperDamage.text = "Damage: " + sniper.GetDamage() + " -> " + (sniper.GetDamage() + 4);
            if (sniper.GetLevel() == sniper.GetMaxLevel())
            {
                UI.S.sniperUpgrade.text = "MAX LEVEL REACHED";
                UI.S.sniperReloadTime.text = "";
                UI.S.sniperDamage.text = "";
            }
        }
        else
        {
            sniperReloadTime.text = "";
            sniperDamage.text = "";
        }

            
    }

    public void SetAmmoTexts()
    {
        if (_weapon.GetWeapon().GetType() == Weapon.WeaponType.ePistol)
        {
            UI.S.buyAmmoText.text = "+" + Main.S.GetComponent<Shop>().rifleAmmoPiecesToBuy + "\nCost: " + Main.S.GetComponent<Shop>().rifleAmmoPrice + "$\n ";
            UI.S.buyAmmoButton.interactable = false;
            UI.S.buyAmmoButton.GetComponent<Image>().enabled = false;
            UI.S.buyAmmoText.text += "You can not buy ammunation for " + _weapon.GetWeapon().GetName();
        }
        else if(_weapon.GetWeapon().GetType() == Weapon.WeaponType.eSniperRifle)
        {
            UI.S.buyAmmoText.text = "\nAmmo for Sniper\n+" + Main.S.GetComponent<Shop>().sniperAmmoPiecesToBuy + "\nCost: " + Main.S.GetComponent<Shop>().sniperAmmoPrice + "$\n ";
            UI.S.buyAmmoButton.interactable = true;
            UI.S.buyAmmoButton.GetComponent<Image>().enabled = true;
            UI.S.buyAmmoText.text += "\n \n ";
        }
        else
        {
            UI.S.buyAmmoText.text = "\nAmmo for machine guns\n+" + Main.S.GetComponent<Shop>().rifleAmmoPiecesToBuy + "\nCost: " + Main.S.GetComponent<Shop>().rifleAmmoPrice + "$\n ";
            UI.S.buyAmmoButton.interactable = true;
            UI.S.buyAmmoButton.GetComponent<Image>().enabled = true;
            UI.S.buyAmmoText.text += "\n \n ";
        }
    }

    private void Update()
    {
        setHP();
        if(!_weapon.isReloadingActive()) reloadingProgress.maxValue = _weapon.GetWeapon().GetReloadSpeed();
    }

    private void FixedUpdate()
    {
        if (_weapon.isReloadingActive()) ShowReloadingBar();
    }


    private void setHP()
    {
        hp.value = _player.GetHP();
        hp.maxValue = _player.GetMaxHP();
        float hpPercent = (float)_player.GetHP() / (float)_player.GetMaxHP();
        hpPercent = Mathf.Round(hpPercent * 100)/100;
        hpColor.color = new Color(0.90f - hpPercent * 0.79f, 0.24f + hpPercent * 0.30f, 0.19f);
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
