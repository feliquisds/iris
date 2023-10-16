using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeUI : MonoBehaviour
{
    internal Animator animator;

    void Awake() => animator = GetComponent<Animator>();
    void Update() => animator.SetInteger("vida", GameObject.FindWithTag("Player").GetComponent<PlayerControl>().health);
}