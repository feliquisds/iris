using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;
using UnityEngine.UI;
using Platformer.UI;

public class FinalBoss : MonoBehaviour
{
    public int health = 20;
    public Material defaultMaterial, whiteMaterial;
    internal float velocity = 0;
    internal bool dying = false, entering = true, hurtPlayer, locked = true;
    internal bool facingLeft => transform.localScale.x == -2.25f;
    public bool attacking = false, canAttack = true;
    internal Collider2D colli => GetComponent<Collider2D>();
    internal Rigidbody2D rb => GetComponent<Rigidbody2D>();
    internal Animator anim => GetComponent<Animator>();
    internal SpriteRenderer sprite => GetComponent<SpriteRenderer>();
    internal AudioSource audioSource => GetComponent<AudioSource>();
    internal PlayerControl player => GameObject.FindWithTag("Player").GetComponent<PlayerControl>();
    internal Transform startPos => GameObject.FindWithTag("Path").transform;
    internal Transform playerPosition => GameObject.FindWithTag("Player").transform;
    internal float distance => Vector3.Distance(playerPosition.position, transform.position);
    public CinemachineVirtualCamera vcam => GameObject.FindWithTag("CameraHandler").GetComponent<CinemachineVirtualCamera>();
    internal GradientHide fade => GameObject.FindWithTag("Fade").GetComponent<GradientHide>();
    internal Tracker tracker => GameObject.FindWithTag("Tracker").GetComponent<Tracker>();
    public Bounds Bounds => colli.bounds;
    public GameObject playerHealth, wave, meteor;
    public Collider2D attackCollider;
    public float spawnXOffset, spawnYOffset, waveSpeed;
    public AudioClip step, death, attack1, attack2;
    internal SubUIController subUIController => GameObject.FindWithTag("GameEvents").GetComponent<SubUIController>();
    internal MetaGameController gameController => GameObject.FindWithTag("GameCore").GetComponent<MetaGameController>();

    void Awake() => StartCoroutine(StartFight());
    IEnumerator StartFight()
    {
        vcam.m_Follow = vcam.m_LookAt = startPos;

        yield return new WaitForSeconds(5);
        var fightCamera = GameObject.FindWithTag("CustomCamera").transform;
        vcam.m_Follow = vcam.m_LookAt = fightCamera;
        entering = false;
        playerHealth.GetComponent<GradientHide>().opaque = true;
        locked = false;

        yield return new WaitForSeconds(0.05f);
        player.controlEnabled = player.canCrouch = gameController.canPause = true;
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

        if (!entering && !dying && Time.timeScale != 0)
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

        if (locked) gameController.canPause = player.controlEnabled = player.canCrouch = player.attacking = player.crouching = entering || dying ? false : true;

        anim.SetFloat("velocityX", Mathf.Abs(velocity));
    }

    void OnTriggerStay2D(Collider2D collider) => Triggered(collider, true);
    void OnTriggerEnter2D(Collider2D collider) => Triggered(collider, false);
    void Triggered(Collider2D collider, bool stay)
    {
        if (collider.gameObject.tag == "PlayerTrigger" && !attacking && canAttack && !player.dead)
        {
            if (stay) StartCoroutine(AttackDelay(Random.Range(3.5f, 4.5f)));
            else Attack();
        }
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
            anim.SetTrigger(player.Bounds.center.y >= Bounds.max.y ? "attack2" : Random.value > 0.49f ? "attack1" : "attack2");
        }
    }

    void WaveAttack()
    {
        GameObject attack = Instantiate(wave, new Vector3(
            (transform.position.x + (facingLeft ? spawnXOffset : (spawnXOffset * -1))),
            (transform.position.y + spawnYOffset), transform.position.z), transform.rotation);
        attack.GetComponent<Rigidbody2D>().velocity = new Vector2((facingLeft ? waveSpeed : (waveSpeed * -1)), 0f);
    }
    IEnumerator MeteorAttack()
    {
        var local_facingLeft = facingLeft;
        for (int i = 3; i < 13; i = i + 3)
        {
            GameObject attack = Instantiate(meteor, new Vector3(
                (transform.position.x + (local_facingLeft ? (i * -1) : i)),
                (transform.position.y + 15), transform.position.z), transform.rotation);
            yield return new WaitForSeconds(0.5f);
        }
    }

    IEnumerator UpdateAttackCondition()
    {
        yield return new WaitForSeconds(0.5f);
        attacking = false;
        yield return new WaitForSeconds(0.5f);
        canAttack = true;
    }

    void OnCollisionEnter2D(Collision2D collider) => Collided(collider);
    void OnCollisionStay2D(Collision2D collider) => Collided(collider);
    void Collided(Collision2D collider)
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

    public void AttackCollider(int activated) => attackCollider.enabled = activated == 1;

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

    void ShowItem() => transform.GetChild(0).gameObject.SetActive(true);

    IEnumerator Die()
    {
        playerHealth.GetComponent<GradientHide>().opaque = false;
        audioSource.Stop();
        dying = locked = true;
        velocity = 0;
        colli.enabled = player.colli.enabled = player.canBeHurt = false;
        player.rb.velocity = Vector2.zero;
        anim.SetTrigger("death");
        vcam.m_Follow = vcam.m_LookAt = transform;

        yield return new WaitForSeconds(4.7f);
        StartCoroutine(subUIController.Transition(true, tracker.shouldPlayCutscene ? 6 : 0, 0.3f));
    }
}
