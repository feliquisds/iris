using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShootSimple : MonoBehaviour
{
    public GameObject projectile;
    public float speed = 15, spawnXOffset = 0.9f, spawnYOffset = 0f;
    internal Animator animator => GetComponent<Animator>();
    internal SpriteRenderer sprite => GetComponent<SpriteRenderer>();
    internal bool rendering => GetComponent<Renderer>().isVisible;

    void OnTriggerEnter2D(Collider2D _collider) { if (_collider.gameObject.tag == "Player") animator.SetBool("attack", true); }
    void OnTriggerExit2D(Collider2D _collider) { if (_collider.gameObject.tag == "Player") animator.SetBool("attack", false); }
    void Update() => animator.SetBool("mute", !rendering);

    void ShootProjectile()
    {
        GameObject p = Instantiate(projectile, new Vector3(
            (transform.position.x + (sprite.flipX ? spawnXOffset : (spawnXOffset * -1))),
            (transform.position.y + spawnYOffset), transform.position.z), transform.rotation);
        var rigid = p.GetComponent<Rigidbody2D>();
        rigid.velocity = new Vector2((sprite.flipX ? speed : (speed * -1)), 0f);
    }
}