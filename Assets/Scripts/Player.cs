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
        Vector2 mouse = (Vector2)mainCamera.ScreenToViewportPoint(Input.mousePosition);
        Vector2 playerPosition = mainCamera.WorldToViewportPoint(player.transform.position);
        shootingDirection = mouse - playerPosition;
        Rotate();
    }

    private void Rotate()
    {
        float angle = Mathf.Atan2(shootingDirection.y, shootingDirection.x) * Mathf.Rad2Deg;
        player.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
    }
}
