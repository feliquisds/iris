using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;
using UnityEngine.UI;

public class FinalBoss : MonoBehaviour
{
    public int health = 20;
    public Material defaultMaterial, whiteMaterial;
    internal float velocity = 0;
    internal bool dying = false, entering = true, hurtPlayer;
    public bool attacking = false, canAttack = true;
    internal Collider2D colli => GetComponent<Collider2D>();
    internal Rigidbody2D rb => GetComponent<Rigidbody2D>();
    internal Animator anim => GetComponent<Animator>();
    internal SpriteRenderer sprite => GetComponent<SpriteRenderer>();
    internal PlayerControl player => GameObject.FindWithTag("Player").GetComponent<PlayerControl>();
    internal Transform startPos => GameObject.FindWithTag("Path").transform;
    internal Transform limitL => GameObject.FindWithTag("LimitL").transform;
    internal Transform limitR => GameObject.FindWithTag("LimitR").transform;
    internal Transform playerPosition => GameObject.FindWithTag("Player").transform;
    public CinemachineVirtualCamera vcam => GameObject.FindWithTag("CameraHandler").GetComponent<CinemachineVirtualCamera>();
    public Bounds Bounds => colli.bounds;
    public GameObject playerHealth;

    void Awake() => StartCoroutine(StartFight());
    IEnumerator StartFight()
    {
        playerHealth.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0f);
        player.controlEnabled = player.canCrouch = false;
        vcam.m_Follow = startPos;
        vcam.m_LookAt = startPos;

        yield return new WaitForSeconds(0.01f);
        player.controlEnabled = player.canCrouch = false;

        yield return new WaitForSeconds(5f);
        var fightCamera = GameObject.FindWithTag("CustomCamera").transform;
        vcam.m_Follow = fightCamera;
        vcam.m_LookAt = fightCamera;
        player.controlEnabled = player.canCrouch = true;
        entering = false;
        playerHealth.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
    }


    void FixedUpdate()
    {
        rb.velocity = new Vector2(velocity, 0f);
    }
    void Update()
    {
        rb.velocity = new Vector2(velocity, 0f);

        if (entering)
        {
            if (transform.position.x > startPos.position.x) velocity = -3f;
            else velocity = 0f;
            if (vcam.m_Lens.OrthographicSize > 3.5f) vcam.m_Lens.OrthographicSize -= 0.05f;
        }

        if (!attacking && !entering && !dying && !hurtPlayer && !player.dead)
        {
            if (playerPosition.position.x < transform.position.x)
            {
                transform.localScale = new Vector3(-2.25f, 2.25f, 1f);
                velocity = -3f;
            }
            if (playerPosition.position.x > transform.position.x)
            {
                transform.localScale = new Vector3(2.25f, 2.25f, 1f);
                velocity = 3f;
            }
        }
        if (hurtPlayer || player.dead || attacking || player.Bounds.center.y >= Bounds.max.y) velocity = 0f;

        if (dying)
        {
            if (vcam.m_Lens.OrthographicSize > 3.5f) vcam.m_Lens.OrthographicSize -= 0.05f;
        }

        if (!entering && !dying && vcam.m_Lens.OrthographicSize < 5f) vcam.m_Lens.OrthographicSize += 0.05f;
        anim.SetFloat("velocityX", Mathf.Abs(velocity));
    }

    void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player" && !attacking && canAttack && !player.dead)
        StartCoroutine(AttackDelay(Random.Range(3.5f, 4.5f)));
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player" && !attacking && canAttack && !player.dead)
        StartCoroutine(Attack());
    }

    IEnumerator AttackDelay(float time)
    {
        yield return new WaitForSeconds(time);
        if (!attacking && canAttack) StartCoroutine(Attack());
    }

    IEnumerator Attack()
    {
        yield return new WaitForSeconds(0f);
        if (!attacking && canAttack)
        {
            attacking = true;
            canAttack = false;
            anim.SetTrigger(Random.value > 0.49f ? "attack1" : "attack2");
        }

    }

    IEnumerator UpdateAttackCondition()
    {
        yield return new WaitForSeconds(0.5f);
        attacking = false;
        yield return new WaitForSeconds(0.5f);
        canAttack = true;
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

        if (collider.gameObject.tag == "Player")
        {
            player.Hurt();
            hurtPlayer = true;
            StartCoroutine(Cooldown());
        }
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

        if (collider.gameObject.tag == "Player")
        {
            player.Hurt();
            hurtPlayer = true;
            StartCoroutine(Cooldown());
        }
    }

    IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(0.75f);
        hurtPlayer = false;
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
        dying = true;
        velocity = 0f;
        colli.enabled = false;
        player.controlEnabled = player.canCrouch = player.attacking = player.crouching = false;
        anim.SetTrigger("death");
        vcam.m_Follow = transform;
        vcam.m_LookAt = transform;

        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene(0);
    }
}
