using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArchProjectile : MonoBehaviour
{
    private Rigidbody2D rb => GetComponent<Rigidbody2D>();
    private Collider2D colli => GetComponent<Collider2D>();
    private SpriteRenderer sprite => GetComponent<SpriteRenderer>();
    private int childCount => transform.childCount;
    private Color spriteColor => sprite.color;
    private bool hasCollided;
    public bool fadesOut = false;
    public float fadeSpeed, audioVolume = 1;
    public AudioClip hitGround;
    internal float angle, fadeAmount;

    void Awake() => StartCoroutine(ActivateHitbox());

    void Update()
    {
        if (!hasCollided)
        {
            angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
        else Disappear();
    }

    void Disappear()
    {
        if (fadesOut)
        {
            fadeAmount = spriteColor.a - (fadeSpeed * Time.deltaTime);
            sprite.color = new Color(spriteColor.r, spriteColor.g, spriteColor.b, fadeAmount);

            if (spriteColor.a <= 0) Destroy(gameObject);
        }
        else if (childCount < 1) Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag != "Ground")
            fadesOut = sprite.enabled = false;
        else sprite.enabled = fadesOut ? true : false;

        if (collision.gameObject.tag == "Player")
            GameObject.FindWithTag("Player").GetComponent<PlayerControl>().Hurt();

        if (TryGetComponent<AudioSource>(out AudioSource audioSource) && GetComponent<Renderer>().isVisible)
            audioSource.PlayOneShot(hitGround, audioVolume);

        rb.simulated = false;
        hasCollided = true;
    }

    IEnumerator ActivateHitbox()
    {
        yield return new WaitForSeconds(0.5f);
        colli.enabled = true;
        yield return new WaitForSeconds(3);
        fadesOut = sprite.enabled = rb.simulated = false;
        hasCollided = true;
    }
}