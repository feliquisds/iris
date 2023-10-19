using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Platformer.Core;
using Platformer.Model;

public class PlayerControl : MonoBehaviour
{
    internal Collider2D colli;          internal Rigidbody2D rb;
    internal SpriteRenderer sprite;     internal Animator anim;

    public bool controlEnabled = true, canAttack;

    public bool grounded => rb.IsTouching(groundFilter);
    public bool onSlope => rb.IsTouching(slopeFilter) || rb.IsTouching(invertedSlopeFilter);

    public int health, maxHealth = 2;

    internal float move;
    internal Vector2 newVelocity;
    public float maxSpeed = 5f, acceleration = 5f, jumpStrength = 8.5f, shootXOffset, shootYOffset;

    public GameObject projectile;
    private bool attacking, crouching, canJump, canBeHurt = true, dead = false;

    internal AudioSource audioSource;
    public AudioClip jumpSound, hurtSound, deathSound;
    public float audioVolume = 1f;

    public ContactFilter2D groundFilter, slopeFilter, invertedSlopeFilter;
    public Bounds Bounds => colli.bounds;

    void Awake()
    {
        colli = GetComponent<Collider2D>();         rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();    anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        health = maxHealth;
    }

    void FixedUpdate()
    {
        if (controlEnabled)
        {
            newVelocity = rb.velocity;

            var desiredVelocity = new Vector2(move, 0f) * Mathf.Max(maxSpeed, 0f);
            var velocity = Mathf.MoveTowards(move, desiredVelocity.x, (acceleration * 100) * Time.deltaTime);
            newVelocity = new Vector2(velocity, newVelocity.y);

            rb.velocity = newVelocity;
        }
        if (attacking) rb.velocity = Vector2.zero;
    }

    void Update()
    {
        if (controlEnabled)
        {
            move = (attacking || crouching) ? 0f : Input.GetAxis("Horizontal");

            if (Input.GetButtonDown("Jump") && canJump)
            {
                audioSource.PlayOneShot(jumpSound, audioVolume);
                canJump = false;
                rb.velocity = new Vector2(rb.velocity.x, jumpStrength);
            }
            if (!grounded && canJump) StartCoroutine(JumpMercy());
        }

        if (move > 0f) sprite.flipX = false;
        if (move < 0f) sprite.flipX = true;

        if (grounded)
        {
            canJump = (!attacking) ? true : false;
            crouching = (Input.GetButton("Crouch")) ? true : false;
            attacking = (canAttack && Input.GetButton("Attack")) ? true : false;
        }

        if (rb.velocity.y < -10f) rb.velocity = new Vector2(rb.velocity.x, -10f);

        if (dead && grounded) rb.simulated = false;

        rb.gravityScale = (onSlope && move == 0f) ? 0 : 2;

        UpdateAnimator();
    }

    IEnumerator JumpMercy() { yield return new WaitForSeconds(0.1f); canJump = false; }

    public void Bounce() => rb.velocity = new Vector2(rb.velocity.x, 9f);

    void AttackToggle() => canJump = controlEnabled = attacking ? false : true;
    void Shoot()
    {
        GameObject p = Instantiate(projectile, transform.position + new Vector3((sprite.flipX ? (shootXOffset * -1) : shootXOffset), shootYOffset, transform.position.z), transform.rotation);
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
        var model = Simulation.GetModel<PlatformerModel>();

        audioSource.PlayOneShot(deathSound, audioVolume);
        anim.SetTrigger(grounded ? "dying" : "airdying");
        health = -1;
        rb.velocity = Vector2.zero;
        dead = true;
        controlEnabled = canBeHurt = false;
        model.virtualCamera.m_Follow = model.virtualCamera.m_LookAt = null;

        yield return new WaitForSeconds(2f);
        StartCoroutine(Respawn());
    }

    IEnumerator Respawn()
    {
        var model = Simulation.GetModel<PlatformerModel>();

        health = maxHealth;
        transform.position = model.spawnPoint.transform.position;
        rb.simulated = true;
        dead = sprite.flipX = false;
        rb.velocity = newVelocity = Vector2.zero;
        move = 0f;
        model.virtualCamera.m_Follow = model.virtualCamera.m_LookAt = model.playercamerapoint;

        yield return new WaitForSeconds(1f);
        controlEnabled = canBeHurt = true;
    }

    private void UpdateAnimator()
    {
        anim.SetBool("dead", dead);
        anim.SetBool("attack", attacking);
        anim.SetBool("crouch", crouching);
        anim.SetBool("grounded", grounded);
        anim.SetFloat("velocityX", Mathf.Abs(newVelocity.x));
        anim.SetFloat("velocityY", rb.velocity.y);
    }
}