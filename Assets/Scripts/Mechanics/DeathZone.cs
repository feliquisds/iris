using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZone : MonoBehaviour
{
    public bool active = true;
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player" && active)
        StartCoroutine(GameObject.FindWithTag("Player").GetComponent<PlayerControl>().Die());
    }
}