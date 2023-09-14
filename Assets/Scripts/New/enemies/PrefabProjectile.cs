using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Platformer.Core;
using Platformer.Mechanics;
using Platformer.Model;
using Platformer.Gameplay;

namespace Platformer.Gameplay
{

public class PrefabProjectile : MonoBehaviour
{
    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }
    
    void OnTriggerEnter2D(Collider2D collision)
    {
        var player = GetComponent<Collider>().gameObject.GetComponent<PlayerController>();

        if ((player != null))
        {
            Simulation.Schedule<PlayerHurt>();
            Destroy(gameObject);            
        }
    }
}
}