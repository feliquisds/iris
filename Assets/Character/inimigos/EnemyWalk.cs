using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWalk : MonoBehaviour
{
    public float speed = 5;
    internal float lastSpeed;
    internal bool freeze = false, attack;
    internal Vector3 initialTransform;
    internal Animator animator => GetComponent<Animator>();
    internal SpriteRenderer sprite => GetComponent<SpriteRenderer>();
    internal Rigidbody2D rb => GetComponent<Rigidbody2D>();
    internal Collider2D _collider => GetComponent<Collider2D>();
    public Bounds Bounds => _collider.bounds;
    internal PlayerControl player => GameObject.FindWithTag("Player").GetComponent<PlayerControl>();
    internal bool rendering => GetComponent<Renderer>().isVisible;

    void Awake() => initialTransform = transform.position;

    void Update()
    {
        rb.velocity = freeze ? Vector2.zero : new Vector2(speed, rb.velocity.y);

        sprite.flipX = speed > 0 ? false : true;
        attack = player.transform.position.x > transform.position.x ? !sprite.flipX : sprite.flipX;

        animator.SetBool("attack", attack);
        animator.SetBool("mute", !rendering);
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
                animator.SetTrigger("attacked");
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