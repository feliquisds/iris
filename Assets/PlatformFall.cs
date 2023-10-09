using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformFall : MonoBehaviour
{
    internal Rigidbody2D rb;
    internal Collider2D collider;
    public float fallDelay = 1f;
    public float disappearDelay = 1.5f;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
    }

    void OnCollisionEnter2D(Collision2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            StartCoroutine(Fall());
        }
    }

    IEnumerator Fall()
    {
        yield return new WaitForSeconds(fallDelay);
        rb.gravityScale = 2f;
        collider.isTrigger = true;
        yield return new WaitForSeconds(disappearDelay);
        collider.isTrigger = false;
    }
}

