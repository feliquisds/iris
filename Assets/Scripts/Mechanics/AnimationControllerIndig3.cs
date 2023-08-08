﻿using System.Collections;
using System.Collections.Generic;
using Platformer.Core;
using Platformer.Model;
using UnityEngine;

namespace Platformer.Mechanics
{
    /// <summary>
    /// AnimationControllerIndig3 integrates physics and animation. It is generally used for simple enemy animation.
    /// </summary>
    [RequireComponent(typeof(SpriteRenderer), typeof(Animator))]
    public class AnimationControllerIndig3 : KinematicObject
    {
        /// <summary>
        /// Max horizontal speed.
        /// </summary>
        public float maxSpeed = 7;
        /// <summary>
        /// Max jump velocity
        /// </summary>
        public float jumpTakeOffSpeed = 7;

        /// <summary>
        /// Used to indicated desired direction of travel.
        /// </summary>
        public Vector2 move;

        /// <summary>
        /// Set to true to initiate a jump.
        /// </summary>
        public bool jump;

        /// <summary>
        /// Set to true to set the current jump velocity to zero.
        /// </summary>
        public bool stopJump;

        /// <summary>
        /// Look at player.
        /// </summary>
        public GameObject player;

        public bool hide;
        public bool attack;

        SpriteRenderer spriteRenderer;
        Animator animator;
        PlatformerModel model = Simulation.GetModel<PlatformerModel>();

        protected virtual void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            animator = GetComponent<Animator>();
        }

        protected override void ComputeVelocity()
        {
            velocity.y = 0f;

            if (player.transform.position.x > transform.position.x) {
                if (spriteRenderer.flipX == false) {
                    hide = true;
                    attack = false;
                }
                else if (spriteRenderer.flipX == true) {
                    hide = false;
                    attack = true;
                }
            }
            else if (player.transform.position.x < transform.position.x) {
                if (spriteRenderer.flipX == false) {
                    hide = false;
                    attack = true;
                }
                else if (spriteRenderer.flipX == true) {
                    hide = true;
                    attack = false;
                }
                
            }

            animator.SetBool("hide", hide);
            animator.SetBool("attack", attack);
        }
    }
}