using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public static Player S;
    public GameObject player;
    public Camera mainCamera;
    public int healthLevel = 0;
    public int maxHealthLevel = 10;
    public int healthUpragdeCost = 25;

    Vector3 shootingDirection;
    private int _maxHP, _hp, _hpLevel = 0, _cost = 10;
    

    public void Awake()
    {
        if (S != null) Debug.LogError("Singleton Player juz istnieje proba utworzenia w " + this.gameObject.name);
        S = this;
    }

    public Player()
    {
        _maxHP = 100;
        _hp = 100;
    }

    public void Heal(int amount)
    {
        if (_hp + amount > _maxHP) _hp = _maxHP;
        else _hp += amount;
    }

    public void UpgradeHP()
    {
        Main.S.gold -= _cost;
        _hpLevel++;
        _cost += _hpLevel * 2;
        _maxHP += 10;
    }

    public void TakeDamage(int amount)
    {
        _hp -= amount;
        if (_hp <= 0)
        {
            _hp = 0;
            SceneManager.LoadScene("DeathScene");
        }
    }

    private void Update()
    {
        Vector2 mouse = Input.mousePosition - mainCamera.WorldToScreenPoint(player.transform.position);
        shootingDirection = mouse;
        Rotate();
    }

    private void Rotate()
    {
        //float angle = Mathf.RoundToInt(Mathf.Atan2(shootingDirection.y, shootingDirection.x) * Mathf.Rad2Deg);
        float angle = Mathf.Atan2(shootingDirection.y-Camera.main.transform.eulerAngles.x, shootingDirection.x) * Mathf.Rad2Deg;
        angle = Mathf.Clamp(angle, -80, 80);
        player.transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    public int GetHP()
    {
        return _hp;
    }

    public int GetCost()
    {
        return _cost;
    }
    public int maxHP
    {
        get { return _maxHP; }
        set { _maxHP = value; }
    }
}
