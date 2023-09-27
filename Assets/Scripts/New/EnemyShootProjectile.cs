using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer.Mechanics
{
public class EnemyShootProjectile : MonoBehaviour
{
    public GameObject projectile;
    public float speed = 15;
    internal SpriteRenderer sprite;
    internal Animator animator;
    internal Collider2D _collider;

    protected virtual void Awake()
    {
        animator = this.GetComponent<Animator>();
        _collider = this.GetComponent<Collider2D>();
    }

    void OnTriggerEnter2D(Collider2D _collider)
    {
        if (_collider.gameObject.tag == "Player")
        {
            animator.SetBool("attack", true);
        }
    }

    void OnTriggerExit2D(Collider2D _collider)
    {
        if (_collider.gameObject.tag == "Player")
        {
            animator.SetBool("attack", false);
        }
    }
    
    void ShootProjectile()
    {
        sprite = GetComponent<SpriteRenderer>();

        if (sprite.flipX)
        {
            GameObject p = Instantiate(projectile, new Vector3(((transform.position.x) + 0.9f), transform.position.y, transform.position.z), transform.rotation);
            var rigid = p.GetComponent<Rigidbody2D>();
            rigid.velocity = new Vector2(speed, 0f);
        }
        else
        {
            GameObject p = Instantiate(projectile, new Vector3(((transform.position.x) - 0.9f), transform.position.y, transform.position.z), transform.rotation);
            var rigid = p.GetComponent<Rigidbody2D>();
            rigid.velocity = new Vector2((speed * -1), 0f);
        }
    }
}
}