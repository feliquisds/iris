using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Platformer.Core;
using Platformer.Model;

public class MoveCamera : MonoBehaviour
{
    internal GameObject player => GameObject.FindWithTag("Player");
    internal bool playerFacingLeft => player.GetComponent<SpriteRenderer>().flipX;
    internal bool playerGrounded => player.GetComponent<PlayerControl>().grounded;
    PlatformerModel model = Simulation.GetModel<PlatformerModel>();
    public bool fullPlayerFollow = true;

    void Update()
    {
        if (fullPlayerFollow)
        {
            if ((playerFacingLeft) && (playerGrounded))
                transform.position = player.transform.position + new Vector3(-1f, 0.5f, 0f);
            else if ((!playerFacingLeft) && (playerGrounded))
                transform.position = player.transform.position + new Vector3(1f, 0.5f, 0f);
        }
        else
        {
            transform.position = new Vector3(player.transform.position.x, transform.position.y, transform.position.z);
        }

        model.playercamerapoint = transform;
    }
}
