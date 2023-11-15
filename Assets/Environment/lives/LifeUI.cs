using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeUI : MonoBehaviour
{
    internal Animator animator => GetComponent<Animator>();
    internal int health => GameObject.FindWithTag("Player").GetComponent<PlayerControl>().health;
    void Update() => animator.SetInteger("vida", health);
}