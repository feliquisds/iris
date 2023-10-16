using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShootArch : MonoBehaviour
{
    public GameObject projectile;
    public float firstSpeed = 3, secondSpeed = 4, thirdSpeed = 5;
    public float verticalSpeed = 9;
    public float spawnXOffset = 0.5f, spawnYOffset = 0f;
    internal Animator animator;

    protected virtual void Awake() => animator = this.GetComponent<Animator>();

    void OnTriggerEnter2D(Collider2D _collider) { if (_collider.gameObject.tag == "Player") animator.SetBool("attack", true); }
    void OnTriggerExit2D(Collider2D _collider) { if (_collider.gameObject.tag == "Player") animator.SetBool("attack", false); }

    void ShootArrow()
    {
        var sprite = GetComponent<SpriteRenderer>();

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