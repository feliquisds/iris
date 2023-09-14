using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer.Mechanics
{
public class EnemyShootProjectile : MonoBehaviour
{
    public GameObject projectile;
    public float speed = 15;
    public float negSpeed = -15;
    internal SpriteRenderer sprite;
    //internal EnemyController enemy;

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
            rigid.velocity = new Vector2(negSpeed, 0f);
        }
    }
}
}