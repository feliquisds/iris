using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShootSimple : MonoBehaviour
{
    public GameObject projectile;
    public float speed = 15, spawnXOffset = 0.9f, spawnYOffset = 0f;
    internal Animator animator;

    protected virtual void Awake() => animator = this.GetComponent<Animator>();

    void OnTriggerEnter2D(Collider2D _collider) { if (_collider.gameObject.tag == "Player") animator.SetBool("attack", true); }
    void OnTriggerExit2D(Collider2D _collider) { if (_collider.gameObject.tag == "Player") animator.SetBool("attack", false); }
    
    void ShootProjectile()
    {
        var sprite = GetComponent<SpriteRenderer>();

        if (sprite.flipX)
        {
            GameObject p = Instantiate(projectile, new Vector3((transform.position.x + spawnXOffset), (transform.position.y + spawnYOffset), transform.position.z), transform.rotation);
            var rigid = p.GetComponent<Rigidbody2D>();
            rigid.velocity = new Vector2(speed, 0f);
        }
        else
        {
            GameObject p = Instantiate(projectile, new Vector3((transform.position.x - spawnXOffset), (transform.position.y + spawnYOffset), transform.position.z), transform.rotation);
            var rigid = p.GetComponent<Rigidbody2D>();
            rigid.velocity = new Vector2((speed * -1), 0f);
        }
    }
}