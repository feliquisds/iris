using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    internal Collider2D colli;
    internal Rigidbody2D rb;
    internal SpriteRenderer sprite;
    internal Animator anim;

    public bool controlEnabled = true;
    public float maxSpeed, acceleration = 5f;
    public float jumpStrength = 8.5f;
    internal float move;
    internal Vector2 newVelocity;

    public bool canAttack;
    public GameObject projectile;
    private bool attacking, crouching, canJump;

    public ContactFilter2D contactFilter;

    void Awake()
    {
        colli = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
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
    }

    void Update()
    {
        if (controlEnabled)
        {
            if (attacking || crouching) move = 0f;
            else move = Input.GetAxis("Horizontal");

            if (Input.GetButtonDown("Jump") && canJump)
            {
                canJump = false;
                rb.velocity += new Vector2(0, jumpStrength);
            }
            if (!grounded && canJump) StartCoroutine(JumpMercy());
        }

        if (move > 0f) sprite.flipX = false;
        else if (move < 0f) sprite.flipX = true;

        if (grounded)
        {
            if (!attacking) canJump = true;

            if (Input.GetButton("Crouch"))
                crouching = true;
            else crouching = false;

            if (canAttack && Input.GetButton("Attack"))
                attacking = true;
            else attacking = false;
        }
        if (rb.velocity.y < -10f)
            rb.velocity = new Vector2(rb.velocity.x, -10f);

        UpdateAnimator();
    }

    public bool grounded => rb.IsTouching(contactFilter);

    void OnCollisionStay2D(Collision2D _collider) { if (_collider.gameObject.tag == "Slope") EnterSlope(); }
    void EnterSlope() => rb.gravityScale = (move == 0f) ? 0 : 2;
    void OnCollisionExit2D(Collision2D _collider) => rb.gravityScale = (_collider.gameObject.tag == "Slope") ? 2 : 2;

    IEnumerator JumpMercy()
    {
        yield return new WaitForSeconds(0.1f);
        canJump = false;
    }

    void AttackToggle() => canJump = controlEnabled = (canJump && controlEnabled) ? false : true;

    void Shoot()
    {
        GameObject p = Instantiate(projectile, transform.position, transform.rotation);
        var rigid = p.GetComponent<Rigidbody2D>();
        rigid.velocity = new Vector2(5f, 0f);
    }

    private void UpdateAnimator()
    {
        anim.SetBool("attack", attacking);
        anim.SetBool("crouch", crouching);
        anim.SetBool("grounded", grounded);
        anim.SetFloat("velocityX", Mathf.Abs(newVelocity.x));
        anim.SetFloat("velocityY", rb.velocity.y);
    }
}