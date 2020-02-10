using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI : MonoBehaviour
{
    public Slider hp, reloadingProgress;
    public TextMeshProUGUI ammo, gold, weaponName, wave;
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
        if(!_weapon.isReloadingActive()) reloadingProgress.maxValue = _weapon.GetWeapon().reloadSpeed;
    }

    private void setTexts()
    {
        ammo.text = _weapon.GetWeapon().currentAmmo + "/" + _weapon.GetWeapon().capacity + "  [" + _weapon.GetWeapon().ammo + "]";
        weaponName.text = _weapon.GetWeapon().name;
        gold.text = "Złoto: " + Main.S.gold;
        wave.text = "Fala: " + Main.S.currentLevel;
    }

    private void setHP()
    {
        hp.value = _player.GetHP();
    }

    public IEnumerator ShowReloadingBar()
    {
        reloadingProgress.gameObject.SetActive(true);

        if (reloadingProgress.value < reloadingProgress.maxValue)
        {

            yield return new WaitForSeconds(0.01f);
            reloadingProgress.value += 0.022f;
            StartCoroutine("ShowReloadingBar");
        }
        else
        {
            reloadingProgress.value = 0;
            reloadingProgress.gameObject.SetActive(false);
        }
    }
}
