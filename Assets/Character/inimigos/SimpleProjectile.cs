using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleProjectile : MonoBehaviour
{
    internal Rigidbody2D rb => GetComponent<Rigidbody2D>();
    internal SpriteRenderer sprite => GetComponent<SpriteRenderer>();
    public bool fromPlayer = false, fromFinalBoss = false, meteor = false;

    void Awake() => StartCoroutine(AutoDestroy());
    void Update()
    {
        sprite.flipX = (rb.velocity.x < 0) ? true : false;
        if (meteor) rb.velocity = new Vector2(0, -10);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!fromPlayer && collision.gameObject.tag == "Player")
        {
            GameObject.FindWithTag("Player").GetComponent<PlayerControl>().Hurt();
            Destroy(gameObject);
        }

        if ((fromFinalBoss && meteor && collision.gameObject.tag == "Ground") ||
            (!fromFinalBoss && (collision.gameObject.tag == "PlayerAttack" || collision.gameObject.tag == "Ground")) ||
            (fromPlayer && collision.gameObject.tag == "Enemy") ||
            (collision.gameObject.tag == "Limits"))
            Destroy(gameObject);
    }

    IEnumerator AutoDestroy()
    {
        if (meteor) transform.Rotate(0, 0, 45, Space.Self);
        yield return new WaitForSeconds(!fromPlayer ? 3 : 1.5f);
        Destroy(gameObject);
    }
}