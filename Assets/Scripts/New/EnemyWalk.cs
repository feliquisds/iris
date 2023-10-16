using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWalk : MonoBehaviour
{
    public float speed = 5;
    internal float lastSpeed;
    internal bool mute, freeze = false, attack;
    internal Animator animator;
    internal SpriteRenderer spriteRenderer;
    internal Rigidbody2D body;
    internal Collider2D _collider;
    public Bounds Bounds => _collider.bounds;
    internal PlayerControl player;

    protected virtual void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        _collider = GetComponent<Collider2D>();
        player = GameObject.FindWithTag("Player").GetComponent<PlayerControl>();
    }

    void Update()
    {
        body.velocity = freeze ? Vector2.zero : new Vector2(speed, 0);

        if (speed > 0f) spriteRenderer.flipX = false;
        if (speed < 0f) spriteRenderer.flipX = true;

        mute = GetComponent<Renderer>().isVisible ? false : true;

        if (player.transform.position.x > transform.position.x) attack = !spriteRenderer.flipX;
        if (player.transform.position.x < transform.position.x) attack = spriteRenderer.flipX;

        animator.SetBool("attack", attack);
        animator.SetBool("mute", mute);
        animator.SetFloat("velocityX", speed);
    }

    void OnCollisionEnter2D(Collision2D _collider)
    {
        var willHurtEnemy = player.Bounds.min.y >= Bounds.max.y;

        if (_collider.gameObject.tag == "Player")
        {
            if (willHurtEnemy)
            {
                player.Bounce();
                Freeze();
            }
            else
            {
                player.Hurt();
                Freeze();
            }
        }
    }

    void OnTriggerEnter2D(Collider2D _collider)
    {
        if (_collider.gameObject.tag == "Path") speed = speed * -1;
    }

    public void Freeze()
    {
        freeze = true;
        StartCoroutine(Unfreeze());
    }

    public IEnumerator Unfreeze()
    {
        yield return new WaitForSeconds(1.5f);
        freeze = false;
    }
}