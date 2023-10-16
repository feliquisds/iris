using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZone : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        StartCoroutine(GameObject.FindWithTag("Player").GetComponent<PlayerControl>().Die());
    }
}