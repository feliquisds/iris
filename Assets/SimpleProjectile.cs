using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleProjectile : MonoBehaviour
{
    internal Rigidbody2D rigid;
    internal SpriteRenderer sprite;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
    }

    void Update() => sprite.flipX = (rigid.velocity.x < 0) ? true : false;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        GameObject.FindWithTag("Player").GetComponent<PlayerControl>().Hurt();

        Destroy(gameObject);
    }
}