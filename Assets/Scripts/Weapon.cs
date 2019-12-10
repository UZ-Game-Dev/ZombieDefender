using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public GameObject weaponModel;

    enum WeaponType {Bow, MachineGun}  //itp. itd.
    List<WeaponDefinition> weapons = new List<WeaponDefinition>();
    public int ammo;
    bool isReloading=false;
    WeaponDefinition weapon;

    abstract class WeaponDefinition
    {
        public int currentAmmo, maxAmmo, level;
        public float reloadSpeed, damage;
        public WeaponType type;

        public abstract void Upgrade();
    }

    class Bow: WeaponDefinition
    {
        public Bow()
        {
            maxAmmo = 1;
            currentAmmo = maxAmmo;
            reloadSpeed = 1f;
            damage = 10f;
            type = WeaponType.Bow;
        }

        public override void Upgrade()
        {
            level++;
            damage++;
            reloadSpeed--;
            // Player.money--;
        }
    }

    private void Start()
    {
        Bow bow=new Bow();
        weapons.Add(bow);
        weapon = bow;
        ammo = bow.maxAmmo;
    }

    private void Update()
    {
        if (Input.GetButtonDown("Mouse X") && ammo > 0 && !isReloading)
            Shoot();

        if (Input.GetButtonDown("R") && !isReloading)
        {
            StartCoroutine("Reload");
            isReloading = true;
        }

        if (Input.GetButtonDown("E") && !isReloading)
            SwapWeapons(1);

        if (Input.GetButtonDown("Q") && !isReloading)
            SwapWeapons(-1);
    }

    void SwapWeapons(int dir)
    {
        int index = weapons.IndexOf(weapon);

        if (index + dir < 0)
            weapon = weapons[weapons.Count-1];

        else if (index + dir > weapons.Count-1)
            weapon = weapons[0];

        else weapon = weapons[index + dir];
    }

    IEnumerator Reload()
    {
        yield return new WaitForSeconds(weapon.reloadSpeed);
        ammo = weapon.maxAmmo;
        isReloading = false;
    }

    public void Shoot()
    {
        ammo--;
        RaycastHit2D hit = Physics2D.Raycast(weaponModel.transform.position, weaponModel.transform.right);

        if(hit)
        {
            Enemy enemy=hit.transform.GetComponent<Enemy>();
            enemy.TakeDamage(weapon.damage);
        }
    }
}
