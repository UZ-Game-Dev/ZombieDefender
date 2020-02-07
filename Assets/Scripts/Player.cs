using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public GameObject player;
    public Camera mainCamera;
    Vector3 shootingDirection;
    private int _maxHP, _hp;

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
        float angle = Mathf.Atan2(shootingDirection.y, shootingDirection.x) * Mathf.Rad2Deg;
        //angle = Mathf.Clamp(angle, -50, 50);
        player.transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    public int GetHP()
    {
        return _hp;
    }
}
