using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleProjectile : MonoBehaviour
{
    internal Rigidbody2D rb => GetComponent<Rigidbody2D>();
    internal SpriteRenderer sprite => GetComponent<SpriteRenderer>();
    public bool fromPlayer = false;

    void Awake() => StartCoroutine(AutoDestroy());
    void Update() => sprite.flipX = (rb.velocity.x < 0) ? true : false;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" && !fromPlayer)
        GameObject.FindWithTag("Player").GetComponent<PlayerControl>().Hurt();

        if (collision.gameObject.tag != "Enemy") Destroy(gameObject);
        if (collision.gameObject.tag == "Enemy" && fromPlayer) Destroy(gameObject);
        if (collision.gameObject.tag == "Ground") Destroy(gameObject);
    }

    IEnumerator AutoDestroy()
    {
        yield return new WaitForSeconds(!fromPlayer ? 3 : 1.5f);
        Destroy(gameObject);
    }
}