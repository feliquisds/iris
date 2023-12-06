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
    public bool finalBoss = false, usingCustomPoint = false, changeOnlyWhenGrounded = true, fixedCoordinateX = false, fixedCoordinateY = false;
    public Vector3 customPoint, customFixedPointX, customFixedPointY;
    internal Vector3 invertedCustomPoint => new Vector3(customPoint.x * -1, customPoint.y, customPoint.z);
    internal float bossCamera => (player.transform.position.x + boss.transform.position.x) / 2;

    void Update()
    {
        if (!finalBoss)
        {
            if (!fixedCoordinateX && !fixedCoordinateY)
            {
                if (playerFacingLeft && ((playerGrounded && changeOnlyWhenGrounded) || (!changeOnlyWhenGrounded)))
                    transform.position = player.transform.position + (usingCustomPoint ? invertedCustomPoint : new Vector3(-1f, 0.5f, 0f));
                else if (!playerFacingLeft && ((playerGrounded && changeOnlyWhenGrounded) || (!changeOnlyWhenGrounded)))
                    transform.position = player.transform.position + (usingCustomPoint ? customPoint : new Vector3(1f, 0.5f, 0f));
            }
            if (fixedCoordinateX) transform.position = new Vector3(customFixedPointX.x, player.transform.position.y + customFixedPointX.y, customFixedPointX.z);
            if (fixedCoordinateY) transform.position = new Vector3(player.transform.position.x + customFixedPointY.x, customFixedPointY.y, customFixedPointY.z);
        }
        else transform.position = new Vector3(bossCamera, transform.position.y, transform.position.z);

        model.playercamerapoint = transform;
    }
}
