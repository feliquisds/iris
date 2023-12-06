using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    internal Rigidbody2D rb => GetComponent<Rigidbody2D>();
    internal bool isDynamic => rb.bodyType == RigidbodyType2D.Dynamic;
    internal Vector3 spawn;
    public float fallDelay, actionDelay = 2.1f, gravity = 0.5f;
    public bool goesBack, limitSpeed;
    internal bool canFall = true;

    void Awake() => spawn = transform.position;

    void FixedUpdate() { if (isDynamic && limitSpeed && rb.velocity.y < -10f) rb.velocity = new Vector2(0f, -10f); }

    void OnCollisionEnter2D(Collision2D collider)
    {
        if (collider.gameObject.tag == "Player" && canFall) StartCoroutine(Fall());
    }
    IEnumerator Fall()
    {
        yield return new WaitForSeconds(fallDelay);
        canFall = false;
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.gravityScale = gravity;
        yield return new WaitForSeconds(actionDelay);

        if (goesBack) GoBack();
        else Destroy(gameObject);
    }
    void GoBack()
    {
        transform.position = spawn;
        rb.bodyType = RigidbodyType2D.Static;
        canFall = true;
    }
}