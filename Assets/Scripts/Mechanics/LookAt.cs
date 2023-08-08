using System;
using Platformer.Gameplay;
using UnityEngine;
using static Platformer.Core.Simulation;

namespace Platformer.Mechanics
{
    /// <summary>
    /// If you want something to look at another thing.
    /// </summary>
    [RequireComponent(typeof(SpriteRenderer), typeof(Animator))]
    public class LookAr : KinematicObject
    {

        /// <summary>
        /// Look at object.
        /// </summary>
        public GameObject obj;

        SpriteRenderer spriteRenderer;
        Animator animator;

        protected void LookAt()
        {
            if (obj.transform.position.x > transform.position.x)
                spriteRenderer.flipX = false;
            else if (obj.transform.position.x < transform.position.x)
                spriteRenderer.flipX = true;
        }
    }
}