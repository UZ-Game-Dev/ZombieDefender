using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject player;
    public Camera mainCamera;
    Vector3 shootingDirection;
    private int hp;

    public Player()
    {
        hp = 100;
    }

    public void Heal(int amount)
    {
        hp += amount;
    }

    public void TakeDamage(int amount)
    {
        hp -= amount;
        if (hp <= 0)
        {
            //ekran game over itd.
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
}
