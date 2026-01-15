using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;
    public float maxDistance = 20f;

    private Rigidbody2D rb;
    private Vector3 startPos;

    private PlayerShoot owner; // 추가

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        startPos = transform.position;
    }

    //  PlayerShoot이 자기 자신을 넘겨줌
    public void Init(PlayerShoot player)
    {
        owner = player;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        ContactPoint2D contact = collision.GetContact(0);
        Vector2 normal = contact.normal;

        Vector2 incoming = rb.velocity.normalized;
        Vector2 reflectDir = Vector2.Reflect(incoming, normal);

        rb.velocity = reflectDir * speed;
        Debug.DrawRay(transform.position, rb.velocity.normalized, Color.red, 0.5f);
    }

    void Update()
    {
        if (Vector3.Distance(startPos, transform.position) > maxDistance)
        {
            Destroy(gameObject);
        }
    }

    //  총알이 사라질 때 플레이어에게 알림
    void OnDestroy()
    {
        Debug.Log("Bullet Destroyed");
        owner?.OnBulletDestroyed();
        if (owner != null)
        {
            owner.OnBulletDestroyed();

        }
    }
}
