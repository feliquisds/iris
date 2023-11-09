using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Platformer.Core;
using Platformer.Model;

public class MoveCamera : MonoBehaviour
{
    internal GameObject player => GameObject.FindWithTag("Player");
    internal GameObject boss => GameObject.FindWithTag("Enemy");
    internal bool playerFacingLeft => player.GetComponent<SpriteRenderer>().flipX;
    internal bool playerGrounded => player.GetComponent<PlayerControl>().grounded;
    PlatformerModel model = Simulation.GetModel<PlatformerModel>();
    public bool finalBoss = false;

    void Update()
    {
        if (!finalBoss)
        {
            if ((playerFacingLeft) && (playerGrounded))
                transform.position = player.transform.position + new Vector3(-1f, 0.5f, 0f);
            else if ((!playerFacingLeft) && (playerGrounded))
                transform.position = player.transform.position + new Vector3(1f, 0.5f, 0f);
        }
        else
        {
            var cameraPoint = (player.transform.position.x + boss.transform.position.x) / 2;
            transform.position = new Vector3(cameraPoint, transform.position.y, transform.position.z);
        }

        model.playercamerapoint = transform;
    }
}
