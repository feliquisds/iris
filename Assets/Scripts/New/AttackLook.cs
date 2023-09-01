using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackLook : MonoBehaviour
{

    public GameObject player;
    SpriteRenderer spriteRenderer;
    public bool attack;
    Animator animator;

    // Start is called before the first frame update
    protected virtual void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player.transform.position.x > transform.position.x) {
                if (spriteRenderer.flipX == false) {
                    attack = true;
                }
                else if (spriteRenderer.flipX == true) {
                    attack = false;
                }
            }
            else if (player.transform.position.x < transform.position.x) {
                if (spriteRenderer.flipX == false) {
                    attack = false;
                }
                else if (spriteRenderer.flipX == true) {
                    attack = true;
                }
                
            }

            animator.SetBool("attack", attack);
    }
}
