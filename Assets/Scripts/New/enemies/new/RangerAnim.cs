using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangerAnim : MonoBehaviour
{
    private Animator anim;
    public bool isAttacking = false;
    public RangeCollisor RangeAlert;
    public float timeCooling;
    public float lasttimecooling;
    private bool cooling = false;
    public bool isInRange = false;

    private void Start()
    {
        anim = GetComponent<Animator>();
        lasttimecooling = timeCooling;
    }
    void Update()
    {
        if (cooling)
        {
            CoolDown();
            anim.SetBool("attack", false);
        }
        if (RangeAlert.RangeAlert && !cooling && isInRange)
        {
            Attack();
        }
        else
        {
            anim.SetBool("attack", false);
        }
    }

    void Attack()
    {
        timeCooling = lasttimecooling;

        anim.SetBool("attack", true);
    }
    void StopAttack()
    {
        cooling = true;
        anim.SetBool("attack", false);
    }
    void TriggerCooling()
    {
        cooling = true;
    }
    void CoolDown()
    {
        timeCooling -= Time.deltaTime;
        if (timeCooling <= 0 && cooling)
        {
            cooling = false;
            timeCooling = lasttimecooling;
        }
    }

    void onAttack()
    {
        isAttacking = true;
    }
    void notAttack()
    {
        isAttacking = false;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            isInRange = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            isInRange = false;
        }
    }
}
