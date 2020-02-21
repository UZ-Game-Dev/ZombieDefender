using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI : MonoBehaviour
{
    public Slider hp, reloadingProgress;
    public TextMeshProUGUI ammo, gold, weaponName, wave, pistolUpgrade, semiUpgrade, autoUpgrade;
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

        if(!Main.S.isEnableToShoot)
        {
            pistolUpgrade.text = _weapon.weapons.Find(gun => gun.GetType() == Weapon.WeaponType.ePistol).GetMoneyForUpgrade()+"$\nUPGRADE";

            Weapon.SemiAutomatic semi = (Weapon.SemiAutomatic)_weapon.weapons.Find(w => w.GetType() == Weapon.WeaponType.eSemiAutomatic);
            if (semi == null)
            {
                semi = new Weapon.SemiAutomatic();
                semiUpgrade.text = semi.GetBuyingPrice() + "$\nBUY";
            }
            else semiUpgrade.text = _weapon.weapons.Find(gun => gun.GetType() == Weapon.WeaponType.eSemiAutomatic).GetMoneyForUpgrade() + "$\nUPGRADE";

            Weapon.Automatic auto = (Weapon.Automatic)_weapon.weapons.Find(w => w.GetType() == Weapon.WeaponType.eAutomatic);
            if (auto == null)
            {
                auto = new Weapon.Automatic();
                autoUpgrade.text = auto.GetBuyingPrice() + "$\nBUY";
            }
            else autoUpgrade.text = _weapon.weapons.Find(gun => gun.GetType() == Weapon.WeaponType.eAutomatic).GetMoneyForUpgrade() + "$\nUPGRADE";
        }
    }

    private void setHP()
    {
        hp.value = _player.GetHP();
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
