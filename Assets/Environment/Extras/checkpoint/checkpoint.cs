using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Platformer.Core;
using Platformer.Model;

public class Checkpoint : MonoBehaviour
{
    Animator animator;
    Collider2D colli;

    void Awake()
    {
        animator = GetComponent<Animator>();
        colli = GetComponent<Collider2D>();
    }

    public void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            var model = Simulation.GetModel<PlatformerModel>();
            model.spawnPoint.transform.position = transform.position;
            animator.SetBool("touched", true);
            colli.enabled = false;
            IncreaseLife();
        }
    }

    void IncreaseLife()
    {
        var player = GameObject.FindWithTag("Player").GetComponent<PlayerControl>();
        player.health = (player.health < 2) ? player.health += 1 : player.health;
    }
}