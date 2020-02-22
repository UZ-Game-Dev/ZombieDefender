using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType { ePistol, eSemiAutomatic, eAutomatic } 

public class Weapon : MonoBehaviour
{
    public GameObject weaponModel;
    public Transform slad;
    
    public enum WeaponType {ePistol, eSemiAutomatic, eAutomatic}
    public List<WeaponDefinition> weapons = new List<WeaponDefinition>();
    public GameObject tracerBox;
    private bool isReloading=false;
    private WeaponDefinition weapon;
    private LineRenderer tracer;
    private UI _ui;
    private int _nextShot = 4;
    public AudioClip gunShotEffect;
    public AudioClip gunReloadEffect;
    public AudioSource audioSource;
    

    //--------------------------------------------------

    public abstract class WeaponDefinition
    {
        public int currentAmmo, ammo, capacity, level, maxFireRate, fireRate, moneyForUpgrade;
        public float reloadSpeed, damage;
        public string name;
        public WeaponType type;

        public abstract void Upgrade();
    }

    public class Pistol: WeaponDefinition
    {
        public Pistol()
        {
            capacity = 15;
            currentAmmo = capacity;
            ammo = 15;
            reloadSpeed = 1.4f;
            damage = 5f;
            name = "Beretta";
            type = WeaponType.ePistol;
            maxFireRate = 1;
            fireRate = maxFireRate;
            moneyForUpgrade = 10;
        }

        public override void Upgrade()
        {
            level++;
            damage += 1f;
            reloadSpeed-=0.05f;
            Main.S.gold -= moneyForUpgrade;
            moneyForUpgrade += 2;
        }
    }

    public class SemiAutomatic : WeaponDefinition
    {
        public SemiAutomatic()
        {
            capacity = 24;
            currentAmmo = capacity;
            ammo = 100;
            reloadSpeed = 1.7f;
            damage = 7f;
            name = "Karabin półaut.";
            type = WeaponType.eSemiAutomatic;
            maxFireRate = 4;
            fireRate = maxFireRate;
            moneyForUpgrade = 18;
        }

        public override void Upgrade()
        {
            level++;
            damage += 1.5f;
            reloadSpeed -= 0.05f;
            Main.S.gold -= moneyForUpgrade;
            moneyForUpgrade += 4;
        }
    }

    public class Automatic : WeaponDefinition
    {
        public Automatic()
        {
            capacity = 30;
            currentAmmo = capacity;
            ammo = 100;
            reloadSpeed = 2f;
            damage = 10f;
            name = "AK-47";
            type = WeaponType.eAutomatic;
            maxFireRate = 1;
            fireRate = maxFireRate;
            moneyForUpgrade = 25;
        }

        public override void Upgrade()
        {
            level++;
            damage += 2f;
            reloadSpeed -= 0.05f;
            Main.S.gold -= moneyForUpgrade;
            moneyForUpgrade += 6;
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
        //SOUND
        audioSource = this.gameObject.GetComponent<AudioSource>();
        audioSource.clip = gunShotEffect;
    }

    private void FixedUpdate()
    {
        if (_nextShot != 0) _nextShot--;
    }

    private void Update()
    {
        if (Input.GetButton("Fire1") && weapon.currentAmmo > 0 && !isReloading && weapon.fireRate>0 && _nextShot == 0 && Main.S.isEnableToShoot)
        {
            Shoot();
            if(weapon.type != WeaponType.eAutomatic) weapon.fireRate--;
            _nextShot = 4;
        }

        if (Input.GetButtonUp("Fire1")) weapon.fireRate = weapon.maxFireRate;

        if ((Input.GetButtonDown("R") && !isReloading && weapon.ammo != 0 && weapon.currentAmmo != weapon.capacity) || (weapon.currentAmmo == 0 && !isReloading))
        {
            audioSource.clip = gunReloadEffect;
            audioSource.Play();
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
            if(weapon.type != WeaponType.ePistol) weapon.ammo -= amount;
        }
        else
        {
            weapon.currentAmmo += weapon.ammo;
            weapon.ammo = 0;
        }
        isReloading = false;
        audioSource.clip = gunShotEffect;
    }

    public void Shoot()
    {
        audioSource.Play();
        weapon.currentAmmo--;
        Ray ray = new Ray(weaponModel.transform.position, weaponModel.transform.forward);
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

        Transform sladBox = Instantiate(slad, weaponModel.transform.position, weaponModel.transform.rotation);
        sladBox.GetComponent<Trace>().waypoint = weaponModel.transform.position + ray.direction * shotDistance;
    }
}
