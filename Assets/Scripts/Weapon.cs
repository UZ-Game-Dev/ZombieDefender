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
    private bool isReloading = false, _triggerReleased = false;
    private WeaponDefinition weapon;
    private LineRenderer tracer;
    private UI _ui;
    private int _nextShot = 8;
    public AudioClip gunShotEffect;
    public AudioClip gunReloadEffect;
    public AudioClip semiShotEffect;
    public AudioClip autoShotEffect;
    public AudioClip semiReloadEffect;
    public AudioClip triggerReleased;
    public AudioSource audioSource;


    //--------------------------------------------------

    public abstract class WeaponDefinition
    {
        protected int currentAmmo, ammo, capacity, level, maxFireRate, fireRate, moneyForUpgrade, buyingPrice;
        protected float reloadSpeed, damage;
        protected string name;
        protected WeaponType type;

        public abstract void Upgrade();
        public int GetAmmo() { return ammo; }
        public int GetCurrentAmmo() { return currentAmmo; }
        public int GetCapacity() { return capacity; }
        public int GetFireRate() { return fireRate; }
        public int GetMaxFireRate() { return maxFireRate; }
        public int GetBuyingPrice() { return buyingPrice; }
        public int GetMoneyForUpgrade() { return moneyForUpgrade; }
        public float GetReloadSpeed() { return reloadSpeed; }
        public float GetDamage() { return damage; }
        public string GetName() { return name; }
        public new WeaponType GetType() { return type; }
        
        public void SetFireRate(int a) { fireRate = a; }
        public void SetCurrentAmmo(int a) { currentAmmo = a; }
        public void SetAmmo(int a) { ammo = a; }
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
            if (Main.S.gold >= moneyForUpgrade)
            {
                Main.S.gold -= moneyForUpgrade;
                level++;
                damage += 1f;
                reloadSpeed -= 0.05f;
                moneyForUpgrade += 2;
            }
        }
    }

    public class SemiAutomatic : WeaponDefinition
    {
        public SemiAutomatic()
        {
            capacity = 24;
            currentAmmo = capacity;
            ammo = capacity * 2;
            reloadSpeed = 1.7f;
            damage = 7f;
            name = "Semi M.G.";
            type = WeaponType.eSemiAutomatic;
            maxFireRate = 4;
            fireRate = maxFireRate;
            moneyForUpgrade = 18;
            buyingPrice = 30;
        }

        public override void Upgrade()
        {
            if(Main.S.gold >= moneyForUpgrade)
            {
                Main.S.gold -= moneyForUpgrade;
                level++;
                damage += 1.5f;
                reloadSpeed -= 0.05f;
                moneyForUpgrade += 4;
            }
        }
    }

    public class Automatic : WeaponDefinition
    {
        public Automatic()
        {
            capacity = 30;
            currentAmmo = capacity;
            ammo = capacity * 2;
            reloadSpeed = 2f;
            damage = 10f;
            name = "AK-47";
            type = WeaponType.eAutomatic;
            maxFireRate = 1;
            fireRate = maxFireRate;
            moneyForUpgrade = 25;
            buyingPrice = 40;
        }

        public override void Upgrade()
        {
            if (Main.S.gold >= moneyForUpgrade)
            {
                Main.S.gold -= moneyForUpgrade;
                level++;
                damage += 2f;
                reloadSpeed -= 0.05f;
                moneyForUpgrade += 6;
                Debug.Log("ULEPSZONO KARABIN AUTOMATYCZNY");
            }
            else Debug.Log("NIE STAĆ MNIE");
        }
    }



    //-------------------------------------------------

    public bool isReloadingActive() { return isReloading; }
    public void turnOffReloading() { isReloading = false; }

    private void Start()
    {
        tracer = tracerBox.GetComponent<LineRenderer>();
        Pistol pistol=new Pistol();
        weapon = pistol;
        weapons.Add(pistol);
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
        if (Input.GetButton("Fire1") && weapon.GetCurrentAmmo() > 0 && !isReloading && weapon.GetFireRate() > 0 && _nextShot == 0 && Main.S.isEnableToShoot)
        {
            if (weapon.GetType() == WeaponType.ePistol) audioSource.clip = gunShotEffect;
            if (weapon.GetType() == WeaponType.eSemiAutomatic) audioSource.clip = semiShotEffect;
            if (weapon.GetType() == WeaponType.eAutomatic) audioSource.clip = autoShotEffect;

            Shoot();
            if(weapon.GetType() != WeaponType.eAutomatic) weapon.SetFireRate(weapon.GetFireRate() - 1);
            _nextShot = 8;
        }

        if (PauseMenu.S.GetIsPaused() && isReloading) audioSource.Pause();
        else if (!PauseMenu.S.GetIsPaused() && isReloading) audioSource.UnPause();
        if (Input.GetButtonUp("Fire1") && Main.S.isEnableToShoot)
        {
            weapon.SetFireRate(weapon.GetMaxFireRate());

        if (!PauseMenu.S.GetIsPaused())
            {
                if (Input.GetButton("Fire1") && weapon.GetCurrentAmmo() > 0 && !isReloading && weapon.GetFireRate() > 0 && _nextShot == 0 && Main.S.isEnableToShoot)
                {
                    Shoot();
                    if (weapon.GetType() != WeaponType.eAutomatic) weapon.SetFireRate(weapon.GetFireRate() - 1);
                    _nextShot = 8;
                }
            if (weapon.GetType() != WeaponType.ePistol)
            {
                _triggerReleased = true;
                audioSource.Stop();
                audioSource.clip = triggerReleased;
                audioSource.Play();
            }
        }

        if (Input.GetButtonDown("R") && !isReloading && weapon.GetAmmo() != 0 && weapon.GetCurrentAmmo() != weapon.GetCapacity())
        {
            if (weapon.GetType() == WeaponType.ePistol) audioSource.clip = gunReloadEffect;
            else audioSource.clip = semiReloadEffect;
            audioSource.Play();
            StartCoroutine("Reload");
            isReloading = true;
        }

        if(!Main.S.isEnableToShoot || (weapon.GetCurrentAmmo() <= 0 && !isReloading && weapon.GetType() != WeaponType.ePistol))
        {
            if (_triggerReleased)
            {
                audioSource.Stop();
                audioSource.clip = triggerReleased;
                audioSource.Play();
                _triggerReleased = false;
            }
        }

        /*if (weapon.GetCurrentAmmo() <= 0 && !isReloading && weapon.GetType() != WeaponType.ePistol)
        {
            audioSource.Stop();
            audioSource.clip = triggerReleased;
            audioSource.Play();
        }*/
        
        if (Input.GetButtonDown("E") && !isReloading)
            SwapWeapons(1);

                if (Input.GetButtonUp("Fire1")) weapon.SetFireRate(weapon.GetMaxFireRate());

                if (Input.GetButtonDown("R") && !isReloading && weapon.GetAmmo() != 0 && weapon.GetCurrentAmmo() != weapon.GetCapacity())
                {
                    audioSource.clip = gunReloadEffect;
                    audioSource.Play();
                    StartCoroutine("Reload");
                    isReloading = true;
                }

                if (Input.GetButtonDown("E") && !isReloading)
                    SwapWeapons(1);

                if (Input.GetButtonDown("Q") && !isReloading)
                    SwapWeapons(-1);
            }
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
        yield return new WaitForSeconds(weapon.GetReloadSpeed());
        int amount = weapon.GetCapacity() - weapon.GetCurrentAmmo();
        if (weapon.GetAmmo() >= amount)
        {
            weapon.SetCurrentAmmo(weapon.GetCapacity());
            if(weapon.GetType() != WeaponType.ePistol) weapon.SetAmmo(weapon.GetAmmo()-amount);
        }
        else
        {
            weapon.SetCurrentAmmo(weapon.GetCurrentAmmo()+weapon.GetAmmo());
            weapon.SetAmmo(0);
        }
        audioSource.clip = gunShotEffect;
    }

    public void Shoot()
    {
        if (audioSource.clip == gunShotEffect) audioSource.Play();
        if(!audioSource.isPlaying) audioSource.Play();
        weapon.SetCurrentAmmo(weapon.GetCurrentAmmo()-1);
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
                enemy.TakeDamage(weapon.GetDamage());
            }
        }

        //Debug.DrawLine(weaponModel.transform.position, weaponModel.transform.position + ray.direction * shotDistance);

        Transform sladBox = Instantiate(slad, weaponModel.transform.position, weaponModel.transform.rotation);
        sladBox.GetComponent<Trace>().waypoint = weaponModel.transform.position + ray.direction * shotDistance;
    }
}