using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZone : MonoBehaviour
{
    public bool active = true;
    internal PlayerControl player => GameObject.FindWithTag("Player").GetComponent<PlayerControl>();
    void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player" && active) StartCoroutine(player.Die());
    }
}