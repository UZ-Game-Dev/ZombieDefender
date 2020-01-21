using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public GameObject weaponModel;

    enum WeaponType {eBow, eMachineGun}  //itp. itd.
    List<WeaponDefinition> weapons = new List<WeaponDefinition>();
    public int ammo;
    public GameObject tracerBox;

    bool isReloading=false;
    WeaponDefinition weapon;
    private LineRenderer tracer;

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
            damage = 6f;
            type = WeaponType.eBow;
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
        tracer = tracerBox.GetComponent<LineRenderer>();
        Bow bow=new Bow();
        weapon = bow;
        ammo = weapon.maxAmmo;
    }

    private void Update()
    {
        if (Input.GetButtonDown("Fire1") && ammo > 0 && !isReloading)
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

        //ammo--;
        Ray ray = new Ray(weaponModel.transform.position, weaponModel.transform.right);
        RaycastHit hit;

        float shotDistance = 14;

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
