using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Platformer.Core;
using Platformer.Mechanics;
using Platformer.Model;

public class Move : MonoBehaviour
{
    /// <summary>
    /// Look at player.
    /// </summary>
    public GameObject player;

    private bool playerC;
    private bool playerJump;

    float neg = -1f;
    float pos = 1f;
    float half = 0.5f;
    float none = 0f;

    PlatformerModel model = Simulation.GetModel<PlatformerModel>();

    void Update()
    {
        playerC = player.GetComponent<PlayerController>().lookRight;
        playerJump = player.GetComponent<PlayerController>().playerGrounded;
        if ((!playerC) && (playerJump))
            transform.position = player.transform.position + new Vector3(neg, half, none);
        else if ((playerC) && (playerJump))
            transform.position = player.transform.position + new Vector3(pos, half, none);
        
        model.playercamerapoint = transform;
    }
}
