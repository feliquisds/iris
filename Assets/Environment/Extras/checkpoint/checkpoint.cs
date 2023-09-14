using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Platformer.Core;
using Platformer.Mechanics;
using Platformer.Model;
using Platformer.Gameplay;

namespace Platformer.Gameplay
{

public class checkpoint : MonoBehaviour
{
    PlatformerModel model = Simulation.GetModel<PlatformerModel>();
    Animator animator;
    Collider selfCollider;

    public void OnTriggerEnter2D(Collider2D collider)
    {
        var selfCollider = GetComponent<Collider2D>();
        var player = collider.gameObject.GetComponent<PlayerController>(); 
        if (player != null)
        {
            animator = GetComponent<Animator>();
            model.spawnPoint.transform.position = this.transform.position;
            animator.SetBool("touched", true);
            selfCollider.enabled = false;
        }
    }
}
}
