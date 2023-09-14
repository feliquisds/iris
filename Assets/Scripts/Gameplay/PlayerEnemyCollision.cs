using Platformer.Core;
using System.Collections;
using Platformer.Mechanics;
using Platformer.Model;
using UnityEngine;
using static Platformer.Core.Simulation;

namespace Platformer.Gameplay
{

    /// <summary>
    /// Fired when a Player collides with an Enemy.
    /// </summary>
    /// <typeparam name="EnemyCollision"></typeparam>
    public class PlayerEnemyCollision : Simulation.Event<PlayerEnemyCollision>
    {
        public EnemyController enemy;
        public PlayerController player;
        internal Rigidbody2D colli;

        PlatformerModel model = Simulation.GetModel<PlatformerModel>();

        public override void Execute()
        {
            colli = enemy.GetComponent<Rigidbody2D>();
            var willHurtEnemy = player.Bounds.center.y >= enemy.Bounds.max.y;

            if (willHurtEnemy)
            {
                player.Bounce(7);
                enemy.Freeze();
                // var enemyHealth = enemy.GetComponent<Health>();
                // if (enemyHealth != null)
                // {
                //     enemyHealth.Decrement();
                //     if (!enemyHealth.IsAlive)
                //     {
                //         Schedule<EnemyDeath>().enemy = enemy;
                //         player.Bounce(2);
                //     }
                //     else
                //     {
                //         player.Bounce(7);
                //     }
                // }
                // else
                // {
                //     Schedule<EnemyDeath>().enemy = enemy;
                //     player.Bounce(2);
                // }
            }
            else
            {
                Schedule<PlayerHurt>();
                enemy.Freeze();
            }
        }

        IEnumerator coroutine()
        {
            yield return new WaitForSeconds(3f);
            enemy.Unfreeze();
        }
    }
}