using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public static Player S;
    public GameObject player;
    public Camera mainCamera;

    Vector3 shootingDirection;
    private int _maxHP, _hp, _hpLevel = 0, _maxHpLevel = 10, _hpBonusPerLevel = 20, _cost = 25;
    

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
    public void SetPlayer(int hp)
    {
        _maxHP = hp;
        _hp = hp;
    }
    public void Heal(int amount)
    {
        if (_hp + amount > _maxHP) _hp = _maxHP;
        else _hp += amount;
        UI.S.hpAmount.text = _hp + "";
    }

    public void UpgradeHP()
    {
        Main.S.gold -= _cost;
        _hpLevel++;
        _maxHP += _hpBonusPerLevel;
        _hp = _maxHP;
        UI.S.hpAmount.text = _hp + "";
    }

    public void TakeDamage(int amount)
    {
        _hp -= amount;

        if (_hp <= 0)
        {
            int wave = Main.S.waveCounter ;
            int bestWave = PlayerPrefs.GetInt("bestWave");
            PlayerPrefs.SetInt("wave", wave);
            if (bestWave < wave) 
                PlayerPrefs.SetInt("bestWave", wave);

            _hp = 0;
            SceneManager.LoadScene("DeathScene");
        }
        UI.S.hpAmount.text = _hp + "";
    }

    private void Update()
    {
        if (!PauseMenu.S.GetIsPaused())
        {
            Vector2 mouse = Input.mousePosition - mainCamera.WorldToScreenPoint(player.transform.position);
            shootingDirection = mouse;
            Rotate();
        }
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

    public int GetMaxHP()
    {
        return _maxHP;
    }

    public int GetHpUpgradeCost()
    {
        return _cost;
    }

    public int GetHpBonusPerLevel()
    {
        return _hpBonusPerLevel;
    }

    public int GetHpLevel()
    {
        return _hpLevel;
    }

    public int GetMaxHpLevel()
    {
        return _maxHpLevel;
    }
}
