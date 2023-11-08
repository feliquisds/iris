using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arara : MonoBehaviour
{
    internal Rigidbody2D rigid;
    internal Collider2D _collider;
    public float horizontalSpeed;
    public float verticalSpeed;

    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();
    }

    void OnTriggerEnter2D(Collider2D _collider)
    {
        if (_collider.gameObject.tag == "Player")
        {
            rigid.velocity = new Vector2(horizontalSpeed, verticalSpeed);
            StartCoroutine(Wait());
        }
    }

    public IEnumerator Wait()
    {
        yield return new WaitForSeconds(5f);
        Destroy(gameObject);
    }
}
