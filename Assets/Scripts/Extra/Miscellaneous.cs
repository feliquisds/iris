using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Miscellaneous : MonoBehaviour
{
    internal Rigidbody2D rigid => GetComponent<Rigidbody2D>();
    internal Animator animator => GetComponent<Animator>();
    public bool flies = true;
    public float horizontalSpeed, verticalSpeed, destroyTime = 5;
    public GameObject explosion;

    void OnTriggerEnter2D(Collider2D _collider)
    {
        if (_collider.gameObject.tag == "Player")
        {
            if (flies)
            {
                if (verticalSpeed == 0) animator.SetTrigger("roll");
                rigid.velocity = new Vector2(horizontalSpeed, verticalSpeed);
                StartCoroutine(SelfDestroy());
            }
            else
            {
                GameObject smoke = Instantiate(explosion, transform.position, transform.rotation);
                smoke.GetComponent<AudioSource>().mute = true;
                smoke.GetComponent<Animator>().SetTrigger("purple");
                animator.SetTrigger("change");
                GetComponent<Collider2D>().enabled = false;
            }
        }
    }

    public IEnumerator SelfDestroy()
    {
        yield return new WaitForSeconds(destroyTime);
        Destroy(gameObject);
    }
}
