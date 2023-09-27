using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Platformer.Core;
using Platformer.Model;
using Platformer.Gameplay;
using static Platformer.Core.Simulation;

namespace Platformer.Mechanics
{
public class EnemyWalk : MonoBehaviour
{
    public float speed = 5;
    internal float lastSpeed;
    internal bool mute;
    internal bool freeze = false;
    internal bool attack;
    internal Animator animator;
    internal SpriteRenderer spriteRenderer;
    internal Rigidbody2D body;
    internal Collider2D _collider;
    internal Bounds Bounds => _collider.bounds;
    internal PlatformerModel model = Simulation.GetModel<PlatformerModel>();

    protected virtual void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        _collider = GetComponent<Collider2D>();
    }

    void Update()
    {
        body.velocity = new Vector2(speed, 0);

        if (speed > 0f) spriteRenderer.flipX = false;
        else if (speed < 0f) spriteRenderer.flipX = true;

        if (GetComponent<Renderer>().isVisible) mute = false;
        else mute = true;

        var player = model.player;
        if (player.transform.position.x > transform.position.x)
        {
            if (spriteRenderer.flipX == false) attack = true;
            else if (spriteRenderer.flipX == true) attack = false;
        }
        else if (player.transform.position.x < transform.position.x)
        {
            if (spriteRenderer.flipX == false) attack = false;
            else if (spriteRenderer.flipX == true) attack = true;     
        }

        animator.SetBool("attack", attack);
        animator.SetBool("mute", mute);
        animator.SetFloat("velocityX", speed);
    }

    void OnCollisionEnter2D(Collision2D _collider)
    {
        var player = model.player;
        var willHurtEnemy = player.Bounds.center.y >= Bounds.max.y;

        if (_collider.gameObject.tag == "Player")
        {
            if (willHurtEnemy)
            {
                player.Bounce(7);
                Freeze();
            }
            else
            {
                Schedule<PlayerHurt>();
                Freeze();
            }
        }
    }

    void OnTriggerEnter2D(Collider2D _collider)
    {
        if (_collider.gameObject.tag == "Path")
        {
            speed = speed * -1;
        }
    }

    public void Freeze()
    {
        freeze = true;
        lastSpeed = speed;
        speed = 0;
        StartCoroutine(Unfreeze());
    }

    public IEnumerator Unfreeze()
    {
        yield return new WaitForSeconds(1.5f);
        freeze = false;
        speed = lastSpeed;
    }
}
}