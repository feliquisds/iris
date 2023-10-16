using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    internal Rigidbody2D rigid;
    internal Vector3 spawn;
    public float fallDelay, actionDelay = 2.1f, gravity = 0.5f;
    public bool goesBack;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spawn = transform.position;
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
        rigid.bodyType = RigidbodyType2D.Dynamic;
        rigid.gravityScale = gravity;
        yield return new WaitForSeconds(actionDelay);

        if (goesBack) GoBack();
        else Destroy(gameObject);
    }

    void GoBack()
    {
        transform.position = spawn;
        rigid.bodyType = RigidbodyType2D.Static;
    }
}