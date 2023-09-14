using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabShoot : MonoBehaviour
{

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Ground")
        {
            Destroy(gameObject);
        }
    }
    
    void Destroy()
    {
        Destroy(gameObject);
    }
}
