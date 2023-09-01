using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mute : MonoBehaviour
{
    public bool mute;
    Animator animator;

    // Start is called before the first frame update
    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
    }
    // Update is called once per frame
    void Update()
    {
            if (GetComponent<Renderer>().isVisible) {
            mute = false;
            }
            else mute = true;
            
            animator.SetBool("mute", mute);
    }
}
