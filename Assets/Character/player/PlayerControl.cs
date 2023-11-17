using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Platformer.Core;
using Platformer.Model;

public class PlayerControl : MonoBehaviour
{
    internal Collider2D colli => GetComponent<Collider2D>();
    internal Rigidbody2D rb => GetComponent<Rigidbody2D>();
    internal SpriteRenderer sprite => GetComponent<SpriteRenderer>();
    internal Animator anim => GetComponent<Animator>();
    internal Animator lifeAnim => GameObject.FindWithTag("LifeUI").GetComponent<Animator>();

    public bool controlEnabled = true, canAttack, canCrouch = true, winning, respawning;

    public bool grounded => rb.IsTouching(groundFilter) || onSlope;
    public bool onSlope => rb.IsTouching(slopeFilter) || rb.IsTouching(invertedSlopeFilter);

    public int maxHealth = 2, health = 2;

    internal float move, targetVelocity;
    public float maxSpeed = 5f, acceleration = 5f, jumpStrength = 8.5f, shootXOffset, shootYOffset;

    public GameObject projectile;
    private bool canJump;
    public bool attacking, crouching, dead = false, canBeHurt = true;

    internal AudioSource audioSource => GetComponent<AudioSource>();
    public AudioClip jumpSound, hurtSound, deathSound;
    public float audioVolume = 1f;

    public ContactFilter2D groundFilter, slopeFilter, invertedSlopeFilter;
    public Bounds Bounds => colli.bounds;

    void FixedUpdate()
    {
        if (controlEnabled)
        {
            targetVelocity = Mathf.MoveTowards(move, move * maxSpeed, (acceleration * 100) * Time.deltaTime);
            rb.velocity = new Vector2(targetVelocity, rb.velocity.y);
        }
        if (winning) rb.velocity = new Vector2(5f, 0f);
        if (rb.velocity.y < -10f) rb.velocity = new Vector2(rb.velocity.x, -10f);
    }

    void Update()
    {
        if (controlEnabled)
        {
            move = (attacking || crouching) ? 0f : Input.GetAxis("Horizontal");
            if (Input.GetButtonDown("Jump") && canJump)
            {
                audioSource.PlayOneShot(jumpSound, audioVolume);
                rb.velocity = new Vector2(rb.velocity.x, jumpStrength);
            }
        }

        if (grounded && canCrouch && Time.timeScale == 1)
        {
            crouching = (Input.GetButton("Crouch")) ? true : false;
            attacking = (canAttack && Input.GetButton("Attack")) ? true : false;
        }

        canJump = grounded ? (attacking ? false : true) : false;

        sprite.flipX = move > 0 ? false : move < 0 ? true : sprite.flipX;

        if (dead && grounded) rb.simulated = false;

        rb.gravityScale = (onSlope && move == 0) ? 0 : 2;

        if (attacking && onSlope) rb.velocity = Vector2.zero;

        UpdateAnimator();
    }

    public void Bounce() => rb.velocity = new Vector2(rb.velocity.x, 9f);

    void AttackToggle() { canJump = controlEnabled = attacking ? false : true; rb.velocity = Vector2.zero; }
    void AttackFallDisable() { controlEnabled = true; attacking = false; }
    void Shoot()
    {
        GameObject p = Instantiate(projectile, transform.position + new Vector3(
            (sprite.flipX ? (shootXOffset * -1) : shootXOffset), shootYOffset, transform.position.z), transform.rotation);
        p.GetComponent<Rigidbody2D>().velocity = new Vector2((sprite.flipX ? -5f : 5f), 0f);
    }

    public void Hurt()
    {
        if (canBeHurt)
        {
            if (health > 0)
            {
                controlEnabled = canBeHurt = false;
                audioSource.PlayOneShot(hurtSound, audioVolume);
                health -= 1;
                anim.SetTrigger("hurting");
                rb.velocity = new Vector2((sprite.flipX ? 3f : -3f), 3f);
                StartCoroutine(HurtRecover());
            }
            else StartCoroutine(Die());
        }
    }

    IEnumerator HurtRecover()
    {
        yield return new WaitForSeconds(0.5f);
        controlEnabled = true;

        yield return new WaitForSeconds(0.5f);
        canBeHurt = true;
    }

    public IEnumerator Die()
    {
        if (canBeHurt)
        {
            var model = Simulation.GetModel<PlatformerModel>();

            audioSource.PlayOneShot(deathSound, audioVolume);
            anim.SetTrigger(grounded ? "dying" : "airdying");
            health = -1;
            rb.velocity = Vector2.zero;
            dead = true;
            controlEnabled = canBeHurt = false;
            model.virtualCamera.m_Follow = model.virtualCamera.m_LookAt = null;

            yield return new WaitForSeconds(2);
            StartCoroutine(Respawn());
        }
    }

    IEnumerator Respawn()
    {
        var model = Simulation.GetModel<PlatformerModel>();
        respawning = true;
        EnemyRespawn();

        health = maxHealth;
        transform.position = model.spawnPoint.transform.position;
        rb.simulated = true;
        dead = sprite.flipX = attacking = false;
        rb.velocity = Vector2.zero;
        move = 0f;
        model.virtualCamera.m_Follow = model.virtualCamera.m_LookAt = model.playercamerapoint;

        yield return new WaitForSeconds(0.75f);
        controlEnabled = true;
        respawning = false;

        yield return new WaitForSeconds(0.75f);
        canBeHurt = true;
    }

    public void IncreaseLife() => health = health >= 2 ? health : health += 1;

    void EnemyRespawn()
    {
        GameObject enemy = GameObject.FindWithTag("EnemyRespawner");
        if (enemy != null) enemy.GetComponent<EnemyRespawner>().Respawn();
    }

    private void UpdateAnimator()
    {
        anim.SetBool("dead", dead);
        anim.SetBool("canjump", canJump);
        anim.SetBool("attack", attacking);
        anim.SetBool("crouch", crouching);
        anim.SetBool("grounded", grounded);
        anim.SetFloat("velocityX", Mathf.Abs(rb.velocity.x));
        anim.SetFloat("velocityY", rb.velocity.y);
        lifeAnim.SetInteger("vida", health);
    }
}