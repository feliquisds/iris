using System.Collections;
using System.Collections.Generic;
using Platformer.Core;
using Platformer.Model;
using UnityEngine;

namespace Platformer.Gameplay
{
    /// <summary>
    /// Fired when the player has died.
    /// </summary>
    /// <typeparam name="PlayerHurt"></typeparam>
    public class PlayerHurt : Simulation.Event<PlayerHurt>
    {
        PlatformerModel model = Simulation.GetModel<PlatformerModel>();

        public override void Execute()
        {
            var player = model.player;
            var colli = player.GetComponent<Collider2D>();
            var rigid = player.GetComponent<Rigidbody2D>();
            if (player.health.currentHP == 0)
            {
                player.health.Die();
                model.virtualCamera.m_Follow = null;
                model.virtualCamera.m_LookAt = null;
                colli.enabled = false;
                player.controlEnabled = false;
                rigid.simulated = false;
                if (player.audioSource && player.ouchAudio)
                    player.audioSource.PlayOneShot(player.ouchAudio);
                player.animator.SetTrigger("dying");
                player.animator.SetBool("dead", true);
                Simulation.Schedule<PlayerSpawn>(2);
            }
            else {
                player.hurting = true;
                player.controlEnabled = false;
                player.animator.SetBool("hurt", true);
                player.animator.SetTrigger("hurting");
                if (player.audioSource && player.hurtAudio)
                    player.audioSource.PlayOneShot(player.hurtAudio);
                player.GotHurt();
            }
        }
    }
    /// <summary>
    /// Fired when the player has died.
    /// </summary>
    /// <typeparam name="PlayerDeath"></typeparam>
    public class PlayerDeath : Simulation.Event<PlayerDeath>
    {
        PlatformerModel model = Simulation.GetModel<PlatformerModel>();

        public override void Execute()
        {
            var player = model.player;
            if (player.health.IsAlive)
            {
                player.health.Die();
                model.virtualCamera.m_Follow = null;
                model.virtualCamera.m_LookAt = null;
                // player.collider.enabled = false;
                player.controlEnabled = false;

                if (player.audioSource && player.ouchAudio)
                    player.audioSource.PlayOneShot(player.ouchAudio);
                player.animator.SetTrigger("dying");
                player.animator.SetBool("dead", true);
                Simulation.Schedule<PlayerSpawn>(2);
            }
        }
    }
}