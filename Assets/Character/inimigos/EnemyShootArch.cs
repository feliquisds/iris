using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShootArch : MonoBehaviour
{
    public GameObject projectile;
    public float firstSpeed = 3, secondSpeed = 4, thirdSpeed = 5;
    public float[] speeds;
    public float verticalSpeed = 9;
    public float spawnXOffset = 0.5f, spawnYOffset = 0f;
    internal float orderOffset = -0.1f;
    internal Animator animator => GetComponent<Animator>();
    internal SpriteRenderer sprite => GetComponent<SpriteRenderer>();

    void Awake() => speeds = new float[] {firstSpeed, secondSpeed, thirdSpeed};
    void OnTriggerEnter2D(Collider2D _collider) { if (_collider.gameObject.tag == "Player") animator.SetBool("attack", true); }
    void OnTriggerExit2D(Collider2D _collider) { if (_collider.gameObject.tag == "Player") animator.SetBool("attack", false); }

    IEnumerator ShootArrow(float delay)
    {
        orderOffset = -0.1f;
        for (int i = 0; i < 3; i++)
        {
            GameObject p = Instantiate(projectile, new Vector3(
                (transform.position.x + (sprite.flipX ? (spawnXOffset * -1) + orderOffset : spawnXOffset + (orderOffset * -1))),
                (transform.position.y + spawnYOffset), transform.position.z), transform.rotation);
            p.GetComponent<Rigidbody2D>().velocity = new Vector2(
                sprite.flipX ? (speeds[i] * -1) : speeds[i], verticalSpeed);
            orderOffset += 0.1f;
            yield return new WaitForSeconds(delay);
        }
    }
}