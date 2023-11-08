using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeUI : MonoBehaviour
{
    internal Animator animator => GetComponent<Animator>();

    void Update() => animator.SetInteger("vida", (int)GameObject.FindWithTag("Player").GetComponent<PlayerControl>().health);
}