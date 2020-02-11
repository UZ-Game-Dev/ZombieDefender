using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public GameObject weaponModel;

    public enum WeaponType {ePistol, eSemiAutomatic, eAutomatic}  //itp. itd.
    List<WeaponDefinition> weapons = new List<WeaponDefinition>();
    //public int ammo;
    public GameObject tracerBox;

    private bool isReloading=false;
    private WeaponDefinition weapon;
    private LineRenderer tracer;
    private UI _ui;

    //--------------------------------------------------

    public abstract class WeaponDefinition
    {
        public int currentAmmo, ammo, capacity, level;
        public float reloadSpeed, damage;
        public string name;
        public WeaponType type;

        public abstract void Upgrade();
    }

    public class Pistol: WeaponDefinition
    {
        public Pistol()
        {
            capacity = 7;
            currentAmmo = capacity;
            ammo = 100;
            reloadSpeed = 1f;
            damage = 5f;
            name = "Pistolet";
            type = WeaponType.ePistol;
        }

        public override void Upgrade()
        {
            level++;
            damage++;
            reloadSpeed-=0.2f;
        }
    }

    public class SemiAutomatic : WeaponDefinition
    {
        public SemiAutomatic()
        {
            capacity = 12;
            currentAmmo = capacity;
            ammo = 100;
            reloadSpeed = 1.4f;
            damage = 7f;
            name = "Karabin półautomatyczny";
            type = WeaponType.eSemiAutomatic;
        }

        public override void Upgrade()
        {
            level++;
            damage++;
            reloadSpeed -= 0.2f;
        }
    }

    public class Automatic : WeaponDefinition
    {
        public Automatic()
        {
            capacity = 24;
            currentAmmo = capacity;
            ammo = 100;
            reloadSpeed = 2f;
            damage = 7f;
            name = "AK-47";
            type = WeaponType.eAutomatic;
        }

        public override void Upgrade()
        {
            level++;
            damage++;
            reloadSpeed -= 0.2f;
        }
    }



    //-------------------------------------------------

    public bool isReloadingActive() { return isReloading; }

    private void Start()
    {
        tracer = tracerBox.GetComponent<LineRenderer>();
        Pistol pistol=new Pistol();
        SemiAutomatic semi = new SemiAutomatic();
        Automatic auto = new Automatic();
        weapon = pistol;
        weapons.Add(pistol);
        weapons.Add(semi);
        weapons.Add(auto);
        GameObject ui = GameObject.FindGameObjectWithTag("UI");
        _ui = ui.GetComponentInChildren<UI>();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Fire1") && weapon.currentAmmo > 0 && !isReloading)
            Shoot();

        if (Input.GetButtonDown("R") && !isReloading && weapon.ammo != 0 && weapon.currentAmmo != weapon.capacity)
        {
            StartCoroutine("Reload");
            _ui.StartCoroutine("ShowReloadingBar");
            isReloading = true;
        }
        
        if (Input.GetButtonDown("E") && !isReloading)
            SwapWeapons(1);

        if (Input.GetButtonDown("Q") && !isReloading)
            SwapWeapons(-1);
    }

    public WeaponDefinition GetWeapon()
    {
        return weapon;
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
        int amount = weapon.capacity - weapon.currentAmmo;
        if (weapon.ammo >= amount)
        {
            weapon.currentAmmo = weapon.capacity;
            weapon.ammo -= amount;
        }
        else
        {
            weapon.currentAmmo += weapon.ammo;
            weapon.ammo = 0;
        }
        isReloading = false;
    }

    public void Shoot()
    {

        weapon.currentAmmo--;
        Ray ray = new Ray(weaponModel.transform.position, weaponModel.transform.right);
        RaycastHit hit;

        float shotDistance = 13f;

        if (Physics.Raycast(ray, out hit, shotDistance))
        {
            Debug.Log("Trafiłem w: " + hit.transform.name);
            shotDistance = hit.distance;

            if (hit.transform.tag.Equals("Enemy"))
            {
                Enemy enemy = hit.transform.GetComponent<Enemy>();
                enemy.TakeDamage(weapon.damage);
            }
        }

        //Debug.DrawLine(weaponModel.transform.position, weaponModel.transform.position + ray.direction * shotDistance);
        StartCoroutine("RenderTracer", ray.direction * shotDistance);
    }

    IEnumerator RenderTracer(Vector3 hitPoint)
    {
        tracerBox.SetActive(true);
        tracer.SetPosition(0, weaponModel.transform.position);
        tracer.SetPosition(1, weaponModel.transform.position + hitPoint);
        yield return null;
        tracerBox.SetActive(false);
    }
}
