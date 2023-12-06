using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArchProjectile : MonoBehaviour
{
    private Rigidbody2D rb => GetComponent<Rigidbody2D>();
    private Collider2D colli => GetComponent<Collider2D>();
    private SpriteRenderer sprite => GetComponent<SpriteRenderer>();
    private int childCount => transform.childCount;
    private TrailRenderer trail;
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
            if (childCount > 0) trail.widthMultiplier = trail.widthMultiplier <= 0 ? 0 : trail.widthMultiplier - (fadeSpeed * Time.deltaTime);

            if (spriteColor.a <= 0) Destroy(gameObject);
        }
        else
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
        if (childCount > 0) trail = this.gameObject.transform.GetChild(0).gameObject.GetComponent<TrailRenderer>();
        yield return new WaitForSeconds(0.5f);
        colli.enabled = true;
        yield return new WaitForSeconds(3);
        fadesOut = sprite.enabled = rb.simulated = false;
        hasCollided = true;
    }
}