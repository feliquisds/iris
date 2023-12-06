using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomCameraChanger : MonoBehaviour
{
    internal MoveCamera cam => GameObject.FindWithTag("PlayerCameraPoint").GetComponent<MoveCamera>();
    public bool changeOnlyWhenGrounded, useFixedCoordinateX, useFixedCoordinateY;
    void OnTriggerEnter2D(Collider2D collider) => Triggered(collider, true);
    void OnTriggerStay2D(Collider2D collider) => Triggered(collider, true);
    void OnTriggerExit2D(Collider2D collider) => Triggered(collider, false);
    void Triggered(Collider2D collider, bool enable)
    {
        if (collider.gameObject.tag == "Player")
        {
            cam.usingCustomPoint = enable;
            if (!changeOnlyWhenGrounded) cam.changeOnlyWhenGrounded = !enable ? true : false;
            if (useFixedCoordinateX) cam.fixedCoordinateX = enable ? true : false;
            if (useFixedCoordinateY) cam.fixedCoordinateY = enable ? true : false;
        }
    }
}
