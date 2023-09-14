using System.Collections;
using System.Collections.Generic;
using Platformer.Gameplay;
using UnityEngine;
using static Platformer.Core.Simulation;

namespace Platformer.Mechanics
{
    /// <summary>
    /// A simple controller for enemies. Provides movement control over a patrol path.
    /// </summary>
    [RequireComponent(typeof(AnimationController), typeof(Collider2D))]
    public class EnemyController : MonoBehaviour
    {
        public PatrolPath path;
        public AudioClip ouch;

        internal PatrolPath.Mover mover;
        internal AnimationController control;
        internal Collider2D _collider;
        internal AudioSource _audio;
        SpriteRenderer spriteRenderer;
        internal Rigidbody2D rigid;

        public Bounds Bounds => _collider.bounds;
        internal bool freeze = false;

        void Awake()
        {
            control = GetComponent<AnimationController>();
            _collider = GetComponent<Collider2D>();
            rigid = GetComponent<Rigidbody2D>();
            _audio = GetComponent<AudioSource>();
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        void OnCollisionEnter2D(Collision2D collision)
        {
            var player = collision.gameObject.GetComponent<PlayerController>();
            if (player != null)
            {
                var ev = Schedule<PlayerEnemyCollision>();
                ev.player = player;
                ev.enemy = this;
            }
        }

        void Update()
        {
            if (path != null && !freeze)
            {
                if (mover == null) mover = path.CreateMover(control.maxSpeed * 0.5f);
                control.move.x = Mathf.Clamp(mover.Position.x - transform.position.x, -1, 1);
            }
            if (freeze)
            {
                control.move.x = 0;
            }
        }

        public void Freeze()
        {
            freeze = true;
            //_collider.enabled = false;
            //rigid.constraints = Rigidbody2DConstraints.FreezeAll;
            StartCoroutine(Unfreeze());
        }

        public IEnumerator Unfreeze()
        {
            yield return new WaitForSeconds(1.5f);
            freeze = false;
            //_collider.enabled = true;
            //rigid.enabled = true;
        }

    }
}