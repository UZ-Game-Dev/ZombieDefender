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
    private bool isReloading = false, _playedEcho = false;
    private WeaponDefinition weapon;
    private LineRenderer tracer;
    private UI _ui;
    private int _nextShot = 8, _lastShot = 0, _rifleAmmo = 0;
    public AudioClip gunShotEffect, gunReloadEffect, semiShotEffect, autoShotEffect, semiReloadEffect, triggerReleased;
    public AudioSource audioSource;


    //--------------------------------------------------

    public abstract class WeaponDefinition
    {
        protected int currentAmmo, ammo, capacity, level, maxLevel, maxFireRate, fireRate, moneyForUpgrade, buyingPrice;
        protected float reloadSpeed, damage, maxReloadSpeed;
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
            level = 1;
            maxLevel = 10;
            capacity = 15;
            currentAmmo = capacity;
            ammo = 15;
            reloadSpeed = 1.40f;
            maxReloadSpeed = 1.00f;
            damage = 4.0f;
            name = "Beretta";
            type = WeaponType.ePistol;
            maxFireRate = 1;
            fireRate = maxFireRate;
            moneyForUpgrade = 14;
        }

        public override void Upgrade()
        {
            if (Main.S.gold >= moneyForUpgrade && level < maxLevel)
            {
                Main.S.gold -= moneyForUpgrade;
                UI.S.gold.text = "Gold: " + Main.S.gold;
                damage = (float)Math.Round(damage + 0.5f, 2);
                if(reloadSpeed > maxReloadSpeed) reloadSpeed = (float)Math.Round(reloadSpeed - 0.02f ,2);
                moneyForUpgrade += 2 + level;
                level++;

                if (level == maxLevel)
                {
                    UI.S.pistolUpgrade.text = "MAX LEVEL REACHED";
                    UI.S.gunReloadTime.text = "";
                    UI.S.gunDamage.text = "";
                }
                else
                {
                    UI.S.pistolUpgrade.text = "Cost: " + moneyForUpgrade + "$";
                    UI.S.gunReloadTime.text = "Reload Spd.: " + reloadSpeed + " -> " + (float)Math.Round(reloadSpeed - 0.02f, 2);
                    UI.S.gunDamage.text = "Damage: " + damage + " -> " + (float)Math.Round(damage + 0.5f, 2);
                }
            }
        }
    }

    public class SemiAutomatic : WeaponDefinition
    {
        public SemiAutomatic()
        {
            level = 1;
            maxLevel = 10;
            capacity = 24;
            currentAmmo = capacity;
            reloadSpeed = 1.70f;
            maxReloadSpeed = 1.0f;
            damage = 7.0f;
            name = "Semi M.G.";
            type = WeaponType.eSemiAutomatic;
            maxFireRate = 4;
            fireRate = maxFireRate;
            moneyForUpgrade = 18;
            buyingPrice = 30;
        }

        public override void Upgrade()
        {
            if(Main.S.gold >= moneyForUpgrade && level < maxLevel)
            {
                Main.S.gold -= moneyForUpgrade;
                UI.S.gold.text = "Gold: " + Main.S.gold;
                damage = (float)Math.Round(damage + 1.5f, 2);
                if (reloadSpeed > maxReloadSpeed) reloadSpeed = (float)Math.Round(reloadSpeed - 0.05f, 2);
                moneyForUpgrade += 4 + level;
                level++;

                if (level == maxLevel)
                {
                    UI.S.semiUpgrade.text = "MAX LEVEL REACHED";
                    UI.S.semiReloadTime.text = "";
                    UI.S.semiDamage.text = "";
                }
                else
                {
                    UI.S.semiUpgrade.text = "Cost: " + moneyForUpgrade + "$";
                    UI.S.semiReloadTime.text = "Reload Spd.: " + reloadSpeed + " -> " + (float)Math.Round(reloadSpeed - 0.05f, 2);
                    UI.S.semiDamage.text = "Damage: " + damage + " -> " + (float)Math.Round(damage + 1.5f, 2);
                }
            }
        }
    }

    public class Automatic : WeaponDefinition
    {
        public Automatic()
        {
            level = 1;
            maxLevel = 10;
            capacity = 30;
            currentAmmo = capacity;
            reloadSpeed = 2.00f;
            maxReloadSpeed = 1.0f;
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
            if (Main.S.gold >= moneyForUpgrade && level < maxLevel)
            {
                Main.S.gold -= moneyForUpgrade;
                UI.S.gold.text = "Gold: " + Main.S.gold;
                damage = (float)Math.Round(damage + 2f, 2);
                if (reloadSpeed > maxReloadSpeed) reloadSpeed = (float)Math.Round(reloadSpeed - 0.05f, 2);
                moneyForUpgrade += 6 + level;
                level++;

                if (level == maxLevel)
                {
                    UI.S.autoUpgrade.text = "MAX LEVEL REACHED";
                    UI.S.autoReloadTime.text = "";
                    UI.S.autoDamage.text = "";
                }
                else
                {
                    UI.S.autoUpgrade.text = "Cost: " + moneyForUpgrade + "$";
                    UI.S.autoReloadTime.text = "Reload Spd.: " + reloadSpeed + " -> " + (float)Math.Round(reloadSpeed - 0.05f, 2);
                    UI.S.autoDamage.text = "Damage: " + damage + " -> " + (float)Math.Round(damage + 2f, 2);
                }
            }
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
        if (PauseMenu.S.GetIsPaused() && isReloading) audioSource.Pause();
        else if (!PauseMenu.S.GetIsPaused() && isReloading) audioSource.UnPause();

        if (!PauseMenu.S.GetIsPaused())
        {
            if (Input.GetButton("Fire1") && weapon.GetCurrentAmmo() > 0 && !isReloading && weapon.GetFireRate() > 0 && _nextShot == 0 && Main.S.isEnableToShoot)
            {
                if (weapon.GetType() == WeaponType.ePistol) audioSource.clip = gunShotEffect;
                if (weapon.GetType() == WeaponType.eSemiAutomatic) audioSource.clip = semiShotEffect;
                if (weapon.GetType() == WeaponType.eAutomatic) audioSource.clip = autoShotEffect;

                Shoot();
                if (weapon.GetType() != WeaponType.eAutomatic) weapon.SetFireRate(weapon.GetFireRate() - 1);
                if (weapon.GetType() != WeaponType.ePistol) _lastShot++;
                _nextShot = 8;
            }
            
            if((Input.GetButton("Fire1") && !_playedEcho && !isReloading && weapon.GetType() != WeaponType.ePistol) && (weapon.GetCurrentAmmo() == 0 || (weapon.GetType() == WeaponType.eSemiAutomatic && weapon.GetFireRate() == 0)))
            {
                _playedEcho = true;
            }

            if (Input.GetButtonUp("Fire1") && Main.S.isEnableToShoot)
            {
                weapon.SetFireRate(weapon.GetMaxFireRate());
                if (weapon.GetType() != WeaponType.ePistol && !_playedEcho) StartCoroutine("PlayShotEcho");
                _playedEcho = false;
            }

            if (Input.GetButtonDown("R") && !isReloading && ((weapon.GetType() != WeaponType.ePistol && _rifleAmmo != 0) || weapon.GetType() == WeaponType.ePistol) && weapon.GetCurrentAmmo() != weapon.GetCapacity())
            {
                if (weapon.GetType() == WeaponType.ePistol) audioSource.clip = gunReloadEffect;
                else audioSource.clip = semiReloadEffect;
                audioSource.Play();
                StartCoroutine("Reload");
                isReloading = true;
            }

            if (!Main.S.isEnableToShoot || (weapon.GetCurrentAmmo() <= 0 && !isReloading && weapon.GetType() != WeaponType.ePistol))
            {
                audioSource.Stop();
                audioSource.volume = 1;
                audioSource.clip = triggerReleased;
                audioSource.Play();
                audioSource.volume = 0.5f;
            }

            if (Input.GetButtonDown("E") && !isReloading)
                SwapWeapons(1);

            if (Input.GetButtonDown("Q") && !isReloading)
                SwapWeapons(-1);
        }
    }

    private IEnumerator PlayShotEcho()
    {
        if (_lastShot < 10 && weapon.GetType() == WeaponType.eAutomatic) yield return new WaitForSeconds(0.16f);
        else if (_lastShot < 10 && weapon.GetType() == WeaponType.eSemiAutomatic) yield return new WaitForSeconds(0.10f);
        _lastShot = 0;
        
        audioSource.Stop();
        audioSource.volume = 1;
        audioSource.clip = triggerReleased;
        audioSource.Play();
    }

    public WeaponDefinition GetWeapon() { return weapon; }
    public int GetRifleAmmo() { return _rifleAmmo; }
    public void AddRifleAmmo(int a) { _rifleAmmo += a; }

    void SwapWeapons(int dir)
    {
        int index = weapons.IndexOf(weapon);

        if (index + dir < 0)
            weapon = weapons[weapons.Count-1];

        else if (index + dir > weapons.Count-1)
            weapon = weapons[0];

        else weapon = weapons[index + dir];

        UI.S.weaponName.text = weapon.GetName();
        if (weapon.GetType() != Weapon.WeaponType.ePistol) UI.S.ammo.text = weapon.GetCurrentAmmo() + "/" + weapon.GetCapacity() + "  [" + _rifleAmmo + "]";
        else UI.S.ammo.text = weapon.GetCurrentAmmo() + "/" + weapon.GetCapacity();

        if (!Main.S.isEnableToShoot) UI.S.SetAmmoTexts();
    }

    private IEnumerator Reload()
    {
        yield return new WaitForSeconds(weapon.GetReloadSpeed());
        int amount = weapon.GetCapacity() - weapon.GetCurrentAmmo();

        if (weapon.GetType() != WeaponType.ePistol)
        {
            if (_rifleAmmo >= amount)
            {
                weapon.SetCurrentAmmo(weapon.GetCapacity());
                _rifleAmmo -= amount;
            }
            else
            {
                weapon.SetCurrentAmmo(weapon.GetCurrentAmmo() + _rifleAmmo);
                _rifleAmmo = 0;
            }
        }
        else weapon.SetCurrentAmmo(weapon.GetCapacity());

        audioSource.clip = gunShotEffect;

        if (weapon.GetType() != Weapon.WeaponType.ePistol) UI.S.ammo.text = weapon.GetCurrentAmmo() + "/" + weapon.GetCapacity() + "  [" + _rifleAmmo + "]";
        else UI.S.ammo.text = weapon.GetCurrentAmmo() + "/" + weapon.GetCapacity();
    }

    public void Shoot()
    {
        audioSource.volume = 0.5f;
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

        if (weapon.GetType() != Weapon.WeaponType.ePistol) UI.S.ammo.text = weapon.GetCurrentAmmo() + "/" + weapon.GetCapacity() + "  [" + _rifleAmmo + "]";
        else UI.S.ammo.text = weapon.GetCurrentAmmo() + "/" + weapon.GetCapacity();
    }
}