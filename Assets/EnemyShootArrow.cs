using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer.Mechanics
{
public class EnemyShootArrow : MonoBehaviour
{
    public GameObject projectile;
    public float speed1 = 3;
    public float speed2 = 4;
    public float speed3 = 5;
    public float speed4 = 9;
    internal SpriteRenderer sprite;

    void ShootArrow()
    {
        sprite = GetComponent<SpriteRenderer>();

        if (sprite.flipX)
        {
            GameObject p1 = Instantiate(projectile, new Vector3((transform.position.x - 0.1f), transform.position.y, transform.position.z), transform.rotation);
            var rigid1 = p1.GetComponent<Rigidbody2D>();
            rigid1.velocity = new Vector2((speed1 * -1), speed4);

            GameObject p2 = Instantiate(projectile, new Vector3(transform.position.x, transform.position.y, transform.position.z), transform.rotation);
            var rigid2 = p2.GetComponent<Rigidbody2D>();
            rigid2.velocity = new Vector2((speed2 * -1), speed4);

            GameObject p3 = Instantiate(projectile, new Vector3((transform.position.x + 0.1f), transform.position.y, transform.position.z), transform.rotation);
            var rigid3 = p3.GetComponent<Rigidbody2D>();
            rigid3.velocity = new Vector2((speed3 * -1), speed4);
        }
        else
        {
            GameObject p1 = Instantiate(projectile, new Vector3((transform.position.x - 0.1f), transform.position.y, transform.position.z), transform.rotation);
            var rigid1 = p1.GetComponent<Rigidbody2D>();
            rigid1.velocity = new Vector2(speed1, speed4);

            GameObject p2 = Instantiate(projectile, new Vector3(transform.position.x, transform.position.y, transform.position.z), transform.rotation);
            var rigid2 = p2.GetComponent<Rigidbody2D>();
            rigid2.velocity = new Vector2(speed2, speed4);

            GameObject p3 = Instantiate(projectile, new Vector3((transform.position.x + 0.1f), transform.position.y, transform.position.z), transform.rotation);
            var rigid3 = p3.GetComponent<Rigidbody2D>();
            rigid3.velocity = new Vector2(speed3, speed4);
        }
    }
}
}