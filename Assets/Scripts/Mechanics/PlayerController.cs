using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Platformer.Gameplay;
using static Platformer.Core.Simulation;
using Platformer.Model;
using Platformer.Core;

namespace Platformer.Mechanics
{
    /// <summary>
    /// This is the main class used to implement control of the player.
    /// It is a superset of the AnimationController class, but is inlined to allow for any kind of customisation.
    /// </summary>
    public class PlayerController : KinematicObject
    {
        public AudioClip jumpAudio;
        public AudioClip respawnAudio;
        public AudioClip ouchAudio;

        /// <summary>
        /// Max horizontal speed of the player.
        /// </summary>
        public float maxSpeed = 7;
        /// <summary>
        /// Initial jump velocity at the start of a jump.
        /// </summary>
        public float jumpTakeOffSpeed = 7;

        public JumpState jumpState = JumpState.Grounded;
        public bool playerGrounded;
        private bool stopJump;
        /*internal new*/ public Collider2D collider2d;
        /*internal new*/ public AudioSource audioSource;
        public Health health;
        public bool controlEnabled = true;

        bool jump;
        public bool lookRight;
        bool hurting = false;
        Vector2 move;
        SpriteRenderer spriteRenderer;
        internal Animator animator;
        readonly PlatformerModel model = Simulation.GetModel<PlatformerModel>();

        public Bounds Bounds => collider2d.bounds;

        void Awake()
        {
            health = GetComponent<Health>();
            audioSource = GetComponent<AudioSource>();
            collider2d = GetComponent<Collider2D>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            animator = GetComponent<Animator>();
        }

        public void GotHurt()
        {
            controlEnabled = false;
            collider2d.enabled = false;
            hurting = true;
            if (lookRight == true) {
                Teleport((transform.position - (new Vector3(3f,0f,0f))));
            }
                
            else {
                Teleport((transform.position - (new Vector3(-3f,0f,0f))));
            }

            health.currentHP = health.currentHP - 1;
            Simulation.Schedule<EnablePlayerInput>(0.5f);
            collider2d.enabled = true;
            hurting = false;
        }

        protected override void Update()
        {
            if (!controlEnabled)
            {
                move.x = 0;
            }
            else
            {
                move.x = Input.GetAxis("Horizontal");
                if (jumpState == JumpState.Grounded && Input.GetButtonDown("Jump"))
                    jumpState = JumpState.PrepareToJump;
                /*else if (Input.GetButtonUp("Jump"))
                {
                    stopJump = true;
                   Schedule<PlayerStopJump>().player = this;
                }*/
            }
            UpdateJumpState();
            base.Update();
        }


        void UpdateJumpState()
        {
            jump = false;
            switch (jumpState)
            {
                case JumpState.PrepareToJump:
                    jumpState = JumpState.Jumping;
                    jump = true;
                    stopJump = false;
                    break;
                case JumpState.Jumping:
                    if (!IsGrounded)
                    {
                        Schedule<PlayerJumped>().player = this;
                        jumpState = JumpState.InFlight;
                    }
                    break;
                case JumpState.InFlight:
                    if (IsGrounded)
                    {
                        Schedule<PlayerLanded>().player = this;
                        jumpState = JumpState.Landed;
                    }
                    break;
                case JumpState.Landed:
                    jumpState = JumpState.Grounded;
                    break;
            }
        }

        protected override void ComputeVelocity()
        {
            if (hurting == false) {
            if (jump && IsGrounded)
            {
                    velocity.y = jumpTakeOffSpeed;
                    jump = false;
            }
            else if (stopJump)
            {
                stopJump = false;
                if (velocity.y < -10)
                {
                    velocity.y = -10;
                }
            }

            if (velocity.y < -10)
                {
                    velocity.y = -10;
                }

            if ((move.x > 0.01f))
                spriteRenderer.flipX = false;
            else if ((move.x < -0.01f))
                spriteRenderer.flipX = true;

            animator.SetBool("grounded", IsGrounded);
            animator.SetFloat("velocityX", Mathf.Abs(velocity.x) / maxSpeed);

            targetVelocity = move * maxSpeed;

            if (spriteRenderer.flipX == false)
                lookRight = true;
            else lookRight = false;

            if (IsGrounded) playerGrounded = true;
            else playerGrounded = false;

            }
            else {
                Vector3 p = transform.position;
                p.x = p.x - 1000f;
                transform.position = p;
            }
        }

        public enum JumpState
        {
            Grounded,
            PrepareToJump,
            Jumping,
            InFlight,
            Landed
        }
    }
}