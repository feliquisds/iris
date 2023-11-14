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
    internal AudioSource audioSource => GetComponent<AudioSource>();
    internal PlayerControl player => GameObject.FindWithTag("Player").GetComponent<PlayerControl>();
    internal Transform startPos => GameObject.FindWithTag("Path").transform;
    internal Transform limitL => GameObject.FindWithTag("LimitL").transform;
    internal Transform limitR => GameObject.FindWithTag("LimitR").transform;
    internal Transform playerPosition => GameObject.FindWithTag("Player").transform;
    internal float distance => Vector3.Distance(playerPosition.position, transform.position);
    public CinemachineVirtualCamera vcam => GameObject.FindWithTag("CameraHandler").GetComponent<CinemachineVirtualCamera>();
    public Bounds Bounds => colli.bounds;
    public GameObject playerHealth, wave, meteor;
    public float spawnXOffset, spawnYOffset, waveSpeed;
    public AudioClip step, death, attack1, attack2;

    void Awake() => StartCoroutine(StartFight());
    IEnumerator StartFight()
    {
        playerHealth.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0f);
        vcam.m_Follow = vcam.m_LookAt = startPos;

        yield return new WaitForSeconds(5);
        var fightCamera = GameObject.FindWithTag("CustomCamera").transform;
        vcam.m_Follow = vcam.m_LookAt = fightCamera;
        entering = false;
        playerHealth.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
    }


    void FixedUpdate() => rb.velocity = new Vector2(velocity, 0f);
    void Update()
    {
        if (entering)
        {
            velocity = transform.position.x > startPos.position.x ? -3f : 0f;
            if (vcam.m_Lens.OrthographicSize > 3.5f) vcam.m_Lens.OrthographicSize -= 0.05f;
        }

        if (!attacking && !entering && !dying && !hurtPlayer && !player.dead)
        {
            transform.localScale = new Vector3(playerPosition.position.x < transform.position.x ? -2.25f : 2.25f, 2.25f, 1f);
            velocity = playerPosition.position.x < transform.position.x ? -3f : 3f;
        }
        if (hurtPlayer || player.dead || attacking || player.Bounds.center.y >= Bounds.max.y) velocity = 0f;

        if (dying)
        {
            if (vcam.m_Lens.OrthographicSize > 3.5f) vcam.m_Lens.OrthographicSize -= 0.05f;
        }

        if (!entering && !dying)
        {
            if (distance < 15)
            {
                if (vcam.m_Lens.OrthographicSize < 5) vcam.m_Lens.OrthographicSize += 0.01f;
                if (vcam.m_Lens.OrthographicSize > 5) vcam.m_Lens.OrthographicSize -= 0.01f;
            }
            else
            {
                if (vcam.m_Lens.OrthographicSize < 6.5f) vcam.m_Lens.OrthographicSize += 0.01f;
                if (vcam.m_Lens.OrthographicSize > 6.5f) vcam.m_Lens.OrthographicSize -= 0.01f;
            }
        }
        
        player.controlEnabled = player.canCrouch = entering || dying ? false : true;

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
        Attack();
    }

    IEnumerator AttackDelay(float time)
    {
        yield return new WaitForSeconds(time);
        if (!attacking && canAttack) Attack();
    }

    void Attack()
    {
        if (!attacking && canAttack)
        {
            attacking = true;
            canAttack = false;
            anim.SetTrigger(Random.value > 0.49f ? "attack1" : "attack2");
        }
    }

    void WaveAttack()
    {
        var facingLeft = transform.localScale.x == -2.25f;
        GameObject attack = Instantiate(wave, new Vector3(
            (transform.position.x + (facingLeft ? spawnXOffset : (spawnXOffset * -1))),
            (transform.position.y + spawnYOffset), transform.position.z), transform.rotation);
        var rigid = attack.GetComponent<Rigidbody2D>();
        rigid.velocity = new Vector2((facingLeft ? waveSpeed : (waveSpeed * -1)), 0f);
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

    IEnumerator PlaySoundEffect(string sfx)
    {
        yield return new WaitForSeconds(0);
        var sound = sfx == "step" ? step : sfx == "death" ? death : sfx == "attack1" ? attack1 : attack2;
        audioSource.PlayOneShot(sound, 1);
    }

    IEnumerator Die()
    {
        dying = true;
        velocity = 0;
        colli.enabled = false;
        anim.SetTrigger("death");
        player.attacking = player.crouching = false;
        vcam.m_Follow = vcam.m_LookAt = transform;

        yield return new WaitForSeconds(5);
        SceneManager.LoadScene(0);
    }
}
