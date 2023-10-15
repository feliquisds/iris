using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Platformer.Model;
using Platformer.Core;

public class MoveCamera : MonoBehaviour
{
    public GameObject player;
    private bool playerFacing, playerGrounded;
    PlatformerModel model = Simulation.GetModel<PlatformerModel>();

    void Update()
    {
        playerFacing = player.GetComponent<SpriteRenderer>().flipX;
        playerGrounded = player.GetComponent<PlayerControl>().grounded;

        if ((playerFacing) && (playerGrounded))
            transform.position = player.transform.position + new Vector3(-1f, 0.5f, 0f);
        else if ((!playerFacing) && (playerGrounded))
            transform.position = player.transform.position + new Vector3(1f, 0.5f, 0f);
        
        model.playercamerapoint = transform;
    }
}
