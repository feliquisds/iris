using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomCameraChanger : MonoBehaviour
{
    internal MoveCamera cam => GameObject.FindWithTag("PlayerCameraPoint").GetComponent<MoveCamera>();
    void OnTriggerEnter2D(Collider2D collider) => cam.usingCustomPoint = collider.gameObject.tag == "Player";
    void OnTriggerStay2D(Collider2D collider) => cam.usingCustomPoint = collider.gameObject.tag == "Player";
    void OnTriggerExit2D(Collider2D collider) => cam.usingCustomPoint = !(collider.gameObject.tag == "Player");
}
