using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Rigidbody2D rigid;
    private SpriteRenderer rend;

    private void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        rend = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (rigid.velocity.x > 0 && !rend.flipX) rend.flipX = true;
        else if (rigid.velocity.x < 0 && rend.flipX) rend.flipX = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        collision.GetComponentInParent<Player>().TakeDamage(0.1f);
    }

    private void OnParticleCollision(GameObject other)
    {
        GameManager.Instance.RemoveEnemy(gameObject);
        Destroy(gameObject);
    }
}
