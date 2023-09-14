using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Platformer.Core.Simulation;
using Platformer.Model;
using Platformer.Core;

namespace Platformer.Gameplay
{
public class Dart : MonoBehaviour
{
    internal Rigidbody2D rigid;
    internal SpriteRenderer sprite;
    internal Collider2D colli;
    PlatformerModel model = Simulation.GetModel<PlatformerModel>();

    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        colli = GetComponent<Collider2D>();
    }

    void Update()
    {
        rigid.velocity = new Vector2(rigid.velocity.x, 0f);
        if (rigid.velocity.x < 0)
        {
            sprite.flipX = true;
        }
        else
        {
            sprite.flipX = false;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Simulation.Schedule<PlayerHurt>();
            Destroy(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }   
    }
}
}