using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleProjectile : MonoBehaviour
{
    internal Rigidbody2D rb => GetComponent<Rigidbody2D>();
    internal SpriteRenderer sprite => GetComponent<SpriteRenderer>();
    public bool fromPlayer = false, fromFinalBoss = false;

    void Awake() => StartCoroutine(AutoDestroy());
    void Update() => sprite.flipX = (rb.velocity.x < 0) ? true : false;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!fromPlayer && collision.gameObject.tag == "Player")
        {
            GameObject.FindWithTag("Player").GetComponent<PlayerControl>().Hurt();
            Destroy(gameObject);
        }

        if ((fromFinalBoss && (collision.gameObject.tag == "LimitL" || collision.gameObject.tag == "LimitR")) ||
            (!fromFinalBoss && (collision.gameObject.tag == "PlayerAttack" || collision.gameObject.tag == "Ground")) ||
            (fromPlayer && collision.gameObject.tag == "Enemy"))
            Destroy(gameObject);
    }

    IEnumerator AutoDestroy()
    {
        yield return new WaitForSeconds(!fromPlayer ? 3 : 1.5f);
        Destroy(gameObject);
    }
}