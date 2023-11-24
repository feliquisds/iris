using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleProjectile : MonoBehaviour
{
    internal Collider2D colli => GetComponent<Collider2D>();
    internal Rigidbody2D rb => GetComponent<Rigidbody2D>();
    internal SpriteRenderer sprite => GetComponent<SpriteRenderer>();
    private Color spriteColor => sprite.color;
    private TrailRenderer trail;
    private float fadeAmount;
    private int childCount => transform.childCount;
    private bool fadeOut = false, hasCollided = false;
    public bool fromPlayer = false, fromFinalBoss = false, meteor = false, ignoreCollision = false;
    internal Collider2D collisionToIgnore => GameObject.FindWithTag("Enemy").GetComponent<FinalBoss>().attackCollider;

    void Awake()
    {
        if (childCount > 0) trail = this.gameObject.transform.GetChild(0).gameObject.GetComponent<TrailRenderer>();
        if (ignoreCollision) Physics2D.IgnoreCollision(colli, collisionToIgnore, true);
        if (meteor) transform.Rotate(0, 0, 45, Space.Self);
        StartCoroutine(Initiate());
    }
    void Update()
    {
        sprite.flipX = (rb.velocity.x < 0) ? true : false;
        if (meteor) rb.velocity = new Vector2(0, -10);
        if (fadeOut) FadeOut();
        if (hasCollided) Disappear();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!fromPlayer && collision.gameObject.tag == "Player")
        {
            GameObject.FindWithTag("Player").GetComponent<PlayerControl>().Hurt();
            rb.simulated = sprite.enabled = false;
            hasCollided = true;
        }

        if ((fromFinalBoss && meteor && collision.gameObject.tag == "Ground") ||
            (!fromFinalBoss && (collision.gameObject.tag == "PlayerAttack" || collision.gameObject.tag == "Ground")) ||
            (collision.gameObject.tag == "Enemy") ||
            (collision.gameObject.tag == "Limits"))
        {
            rb.simulated = sprite.enabled = false;
            hasCollided = true;
        }
    }

    IEnumerator Initiate()
    {
        if (TryGetComponent(out AudioSource audioSource))
        {
            yield return new WaitForSeconds(0.3f);
            audioSource.Play();
        }
        yield return new WaitForSeconds(!fromPlayer ? 2.7f : 1.2f);
        fadeOut = true;
    }
    void FadeOut()
    {
        fadeAmount = spriteColor.a - (3.5f * Time.deltaTime);
        sprite.color = new Color(spriteColor.r, spriteColor.g, spriteColor.b, fadeAmount);
        if (childCount > 0) trail.widthMultiplier = trail.widthMultiplier <= 0 ? 0 : trail.widthMultiplier - (3.5f * Time.deltaTime);

        if (spriteColor.a <= 0)
        {
            rb.simulated = sprite.enabled = false;
            hasCollided = true;
        }
    }
    void Disappear()
    {
        if (childCount < 1) Destroy(gameObject);
        else
        {
            trail.startWidth = trail.startWidth <= 0 ? 0 : trail.startWidth - (3.5f * Time.deltaTime);
            trail.endWidth = trail.endWidth <= 0 ? 0 : trail.endWidth - (3.5f * Time.deltaTime);
            trail.widthMultiplier = trail.widthMultiplier <= 0 ? 0 : trail.widthMultiplier - (3.5f * Time.deltaTime);
        }
    }
}