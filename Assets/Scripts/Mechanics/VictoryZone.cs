using Platformer.Gameplay;
using UnityEngine;
using System.Collections;
using static Platformer.Core.Simulation;
using UnityEngine.SceneManagement;

namespace Platformer.Mechanics
{
    /// <summary>
    /// Marks a trigger as a VictoryZone, usually used to end the current game level.
    /// </summary>
    public class VictoryZone : MonoBehaviour
    {
        void OnTriggerEnter2D(Collider2D collider)
        {
            var sprite = this.GetComponent<SpriteRenderer>();
            var p = collider.gameObject.GetComponent<PlayerController>();
            if (p != null)
            {
                var ev = Schedule<PlayerEnteredVictoryZone>();
                ev.victoryZone = this;
                sprite.enabled = false;
                StartCoroutine(WaitForSceneLoad());
            }
        }

        private IEnumerator WaitForSceneLoad()
        {
            yield return new WaitForSeconds(2);
            SceneManager.LoadScene((SceneManager.GetActiveScene().buildIndex) + 1);
        }
    }
}