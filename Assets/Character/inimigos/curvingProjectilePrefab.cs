using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Platformer.Core.Simulation;
using Platformer.Model;
using Platformer.Core;

namespace Platformer.Gameplay
{

public class curvingProjectilePrefab : MonoBehaviour
{
    private Rigidbody2D rb;
    private Collider2D colli;
    private bool hasHitGround;
    public float fadeSpeed;

    void Start()
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
        else
        {
            fadeOut();
        }
    }
    void fadeOut()
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
        if(collision.gameObject.tag == "Ground")
        {
            hasHitGround = true;
            rb.simulated = false;
        }
        if(collision.gameObject.tag == "Player")
        {
            Simulation.Schedule<PlayerHurt>();
            Destroy(gameObject);
        }
    }

    IEnumerator ActivateHitbox()
    {
        yield return new WaitForSeconds(0.5f);
        colli.enabled = true;
    }
}
}