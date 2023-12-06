using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Miscellaneous : MonoBehaviour
{
    internal Rigidbody2D rigid => GetComponent<Rigidbody2D>();
    internal Animator animator => GetComponent<Animator>();
    public bool flies = true;
    public float horizontalSpeed, verticalSpeed;
    public GameObject explosion;

    void OnTriggerEnter2D(Collider2D _collider)
    {
        if (_collider.gameObject.tag == "Player")
        {
            if (flies)
            {
                rigid.velocity = new Vector2(horizontalSpeed, verticalSpeed);
                StartCoroutine(SelfDestroy());
            }
            else
            {
                GameObject smoke = Instantiate(explosion, transform.position, transform.rotation);
                smoke.GetComponent<AudioSource>().mute = true;
                smoke.GetComponent<Animator>().SetTrigger("purple");
                animator.SetTrigger("change");
            }
        }
    }

    public IEnumerator SelfDestroy()
    {
        yield return new WaitForSeconds(5f);
        Destroy(gameObject);
    }
}
