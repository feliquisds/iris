using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;

public class FinalBoss : MonoBehaviour
{
    public int health = 20;
    public Material defaultMaterial, whiteMaterial;
    internal float velocity = 0;
    internal bool dying = false;
    internal Collider2D colli => GetComponent<Collider2D>();
    internal Rigidbody2D rb => GetComponent<Rigidbody2D>();
    internal Animator anim => GetComponent<Animator>();
    internal SpriteRenderer sprite => GetComponent<SpriteRenderer>();
    internal PlayerControl player => GameObject.FindWithTag("Player").GetComponent<PlayerControl>();
    public CinemachineVirtualCamera vcam => GameObject.FindWithTag("CameraHandler").GetComponent<CinemachineVirtualCamera>();

    void FixedUpdate()
    {
        rb.velocity = new Vector2(velocity, 0f);
    }
    void Update()
    {
        rb.velocity = new Vector2(velocity, 0f);

        if (dying)
        {
            if (vcam.m_Lens.OrthographicSize > 3.5f) vcam.m_Lens.OrthographicSize -= 0.1f;
        }
    }

    void OnCollisionEnter2D(Collision2D collider)
    {
        if (collider.gameObject.tag == "PlayerAttack")
        {
            if (health > 0)
            {
                health -= 1;
                StartCoroutine(Flicker());
            }
            else StartCoroutine(Die());
        }

        if (collider.gameObject.tag == "Player") player.Hurt();
    }

    void OnCollisionStay2D(Collision2D collider)
    {
        if (collider.gameObject.tag == "PlayerAttack")
        {
            if (health > 0)
            {
                health -= 1;
                StartCoroutine(Flicker());
            }
            else StartCoroutine(Die());
        }

        if (collider.gameObject.tag == "Player") player.Hurt();
    }

    IEnumerator Flicker()
    {
        sprite.material = whiteMaterial;
        yield return new WaitForSeconds(0.1f);
        sprite.material = defaultMaterial;
        yield return new WaitForSeconds(0.1f);
    }

    IEnumerator Die()
    {
        player.controlEnabled = player.canCrouch = false;
        anim.SetTrigger("death");
        vcam.m_Follow = transform;
        vcam.m_LookAt = transform;
        dying = true;

        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene(0);
    }
}
