using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeCollisor : MonoBehaviour
{
    public bool RangeAlert = false;
    //Isso aqui vai verificar se o player está ou não tocando a área de disparo.

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            RangeAlert = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            RangeAlert = false;
        }
    }
}
