using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float bulletSpeed = 10f;

    private bool hasBullet;

    void Update()
    {
        if (Input.GetMouseButtonDown(1) && !hasBullet)
        {
            Shoot();
        }
    }

    void Shoot()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;

        Vector2 dir = (mousePos - transform.position).normalized;

        GameObject bulletObj =
            Instantiate(bulletPrefab, transform.position, Quaternion.identity);

        Rigidbody2D rb = bulletObj.GetComponent<Rigidbody2D>();
        rb.velocity = dir * bulletSpeed;

        //  여기 중요
        Bullet bullet = bulletObj.GetComponent<Bullet>();
        bullet.Init(this);

        hasBullet = true;
    }

    //  Bullet에서 호출됨
    public void OnBulletDestroyed()
    {
        hasBullet = false;
    }
}
