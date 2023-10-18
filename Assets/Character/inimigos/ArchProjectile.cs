using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArchProjectile : MonoBehaviour
{
    private Rigidbody2D rb;
    private Collider2D colli;
    private bool hasHitGround;
    public float fadeSpeed;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        colli = GetComponent<Collider2D>();
        StartCoroutine(ActivateHitbox());
    }

    void Update()
    {
        if (!hasHitGround)
        {
            float angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
        else FadeOut();
    }
    
    void FadeOut()
    {
        Color objColor = this.GetComponent<SpriteRenderer>().color;
        float fadeAmount = objColor.a - (fadeSpeed * Time.deltaTime);

        objColor = new Color(objColor.r, objColor.g, objColor.b, fadeAmount);
        this.GetComponent<SpriteRenderer>().color = objColor;

        if (objColor.a <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground" || collision.gameObject.tag == "Slope")
        {
            hasHitGround = true;
            rb.simulated = false;
        }
        if (collision.gameObject.tag == "Player")
        {
            GameObject.FindWithTag("Player").GetComponent<PlayerControl>().Hurt();
            Destroy(gameObject);
        }
    }

    IEnumerator ActivateHitbox()
    {
        yield return new WaitForSeconds(0.5f);
        colli.enabled = true;
    }
}