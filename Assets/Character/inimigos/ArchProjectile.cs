using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArchProjectile : MonoBehaviour
{
    private Rigidbody2D rb => GetComponent<Rigidbody2D>();
    private Collider2D colli => GetComponent<Collider2D>();
    private SpriteRenderer sprite => GetComponent<SpriteRenderer>();
    private Color spriteColor => sprite.color;
    private bool hasHitGround;
    public float fadeSpeed, audioVolume = 1f;
    public AudioClip hitGround;
    internal float angle, fadeAmount;

    void Awake() => StartCoroutine(ActivateHitbox());

    void Update()
    {
        if (!hasHitGround)
        {
            angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
        else FadeOut();
    }
    
    void FadeOut()
    {
        fadeAmount = spriteColor.a - (fadeSpeed * Time.deltaTime);
        sprite.color = new Color(spriteColor.r, spriteColor.g, spriteColor.b, fadeAmount);

        if (spriteColor.a <= 0) Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            hasHitGround = true;
            rb.simulated = false;

            if (TryGetComponent<AudioSource>(out AudioSource audioSource) && GetComponent<Renderer>().isVisible)
            audioSource.PlayOneShot(hitGround, audioVolume);
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