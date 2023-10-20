using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleProjectile : MonoBehaviour
{
    internal Rigidbody2D rb;
    internal SpriteRenderer sprite;
    public bool fromPlayer = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        StartCoroutine(AutoDestroy());
    }

    void Update() => sprite.flipX = (rb.velocity.x < 0) ? true : false;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" && !fromPlayer)
        GameObject.FindWithTag("Player").GetComponent<PlayerControl>().Hurt();

        if (collision.gameObject.tag == "Enemy" && fromPlayer)
        Debug.Log("hit");

        Destroy(gameObject);
    }

    IEnumerator AutoDestroy()
    {
        yield return new WaitForSeconds(fromPlayer ? 3f : 5f);
        Destroy(gameObject);
    }
}