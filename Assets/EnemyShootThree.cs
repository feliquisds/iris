using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer.Mechanics
{
public class EnemyShootThree : MonoBehaviour
{
    public GameObject projectile;
    public float firstSpeed = 3;
    public float secondSpeed = 4;
    public float thirdSpeed = 5;
    public float verticalSpeed = 9;
    public float spawnXOffset = 0.5f;
    public float spawnYOffset = 0f;
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

    void ShootArrow()
    {
        sprite = GetComponent<SpriteRenderer>();

        if (sprite.flipX)
        {
            GameObject p1 = Instantiate(projectile, new Vector3((transform.position.x - spawnXOffset - 0.1f), (transform.position.y + spawnYOffset), transform.position.z), transform.rotation);
            var rigid1 = p1.GetComponent<Rigidbody2D>();
            rigid1.velocity = new Vector2((firstSpeed * -1), verticalSpeed);

            GameObject p2 = Instantiate(projectile, new Vector3((transform.position.x - spawnXOffset), (transform.position.y + spawnYOffset), transform.position.z), transform.rotation);
            var rigid2 = p2.GetComponent<Rigidbody2D>();
            rigid2.velocity = new Vector2((secondSpeed * -1), verticalSpeed);

            GameObject p3 = Instantiate(projectile, new Vector3((transform.position.x - spawnXOffset + 0.1f), (transform.position.y + spawnYOffset), transform.position.z), transform.rotation);
            var rigid3 = p3.GetComponent<Rigidbody2D>();
            rigid3.velocity = new Vector2((thirdSpeed * -1), verticalSpeed);
        }
        else
        {
            GameObject p1 = Instantiate(projectile, new Vector3((transform.position.x + spawnXOffset + 0.1f), (transform.position.y + spawnYOffset), transform.position.z), transform.rotation);
            var rigid1 = p1.GetComponent<Rigidbody2D>();
            rigid1.velocity = new Vector2(firstSpeed, verticalSpeed);

            GameObject p2 = Instantiate(projectile, new Vector3((transform.position.x + spawnXOffset), (transform.position.y + spawnYOffset), transform.position.z), transform.rotation);
            var rigid2 = p2.GetComponent<Rigidbody2D>();
            rigid2.velocity = new Vector2(secondSpeed, verticalSpeed);

            GameObject p3 = Instantiate(projectile, new Vector3((transform.position.x + spawnXOffset - 0.1f), (transform.position.y + spawnYOffset), transform.position.z), transform.rotation);
            var rigid3 = p3.GetComponent<Rigidbody2D>();
            rigid3.velocity = new Vector2(thirdSpeed, verticalSpeed);
        }
    }
}
}