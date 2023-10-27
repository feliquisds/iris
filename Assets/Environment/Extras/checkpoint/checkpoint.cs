using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Platformer.Core;
using Platformer.Model;

public class Checkpoint : MonoBehaviour
{
    internal Animator animator => GetComponent<Animator>();
    internal Collider2D colli => GetComponent<Collider2D>();
    internal PlayerControl player => GameObject.FindWithTag("Player").GetComponent<PlayerControl>();

    public void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            colli.enabled = false;
            animator.SetBool("touched", true);

            var model = Simulation.GetModel<PlatformerModel>();
            model.spawnPoint.transform.position = transform.position;

            player.IncreaseLife();
        }
    }
}