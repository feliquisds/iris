using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Platformer.Gameplay;
using static Platformer.Core.Simulation;
using Platformer.Model;
using Platformer.Core;

namespace Platformer.Mechanics
{

public class Vida : MonoBehaviour
{

    PlatformerModel model = Simulation.GetModel<PlatformerModel>();

    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        var player = model.player;
        animator.SetInteger("vida", player.health.newHP);
        animator.SetBool("morto", player.health.death);
    }
}
}
