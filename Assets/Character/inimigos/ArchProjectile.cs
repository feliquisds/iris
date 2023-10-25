using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArchProjectile : MonoBehaviour
{
    private Rigidbody2D rb => GetComponent<Rigidbody2D>();
    private Collider2D colli => GetComponent<Collider2D>();
    private bool hasHitGround;
    public float fadeSpeed;

    void Awake() => StartCoroutine(ActivateHitbox());

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
        if (collision.gameObject.tag == "Ground")
        {
            hasHitGround = true;
            rb.simulated = false;
        }
        if (collision.gameObject.tag == "Player")
        {
            GameObject.FindWithTag("Player").GetComponent<PlayerControl>().Hurt();
            Destroy(gameObject);
        }
        if (collision.gameObject.tag == "PlayerAttack") Destroy(gameObject);
    }

    IEnumerator ActivateHitbox()
    {
        yield return new WaitForSeconds(0.5f);
        colli.enabled = true;
        yield return new WaitForSeconds(10f);
        Destroy(gameObject);
    }
}