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
    internal bool willHurtEnemy => player.Bounds.min.y >= Bounds.max.y;

    void Awake() => initialTransform = transform.position;
    void Respawn() { transform.position = initialTransform; freeze = false; }

    void FixedUpdate() => rb.velocity = freeze ? Vector2.zero : new Vector2(speed, rb.velocity.y);
    void Update()
    {
        if (player.respawning) Respawn();
        sprite.flipX = speed > 0 ? false : true;
        attack = player.transform.position.x > transform.position.x ? !sprite.flipX : sprite.flipX;

        animator.SetBool("attack", attack);
        animator.SetBool("mute", !rendering);
        animator.SetFloat("velocityX", speed);
    }

    void OnCollisionEnter2D(Collision2D collider) => Collided(collider);
    void OnCollisionStay2D(Collision2D collider) => Collided(collider);
    void Collided(Collision2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            if (willHurtEnemy) player.Bounce();
            else
            {
                player.Hurt();
                animator.SetTrigger("attacked");
            }
            Freeze();
        }
    }

    void OnTriggerEnter2D(Collider2D collider) => speed = collider.gameObject.tag == "Path" ? speed * -1 : speed;

    public void Freeze()
    {
        if (!freeze) freeze = true;
        StartCoroutine(Unfreeze());
    }
    public IEnumerator Unfreeze()
    {
        yield return new WaitForSeconds(1.5f);
        freeze = false;
    }
}