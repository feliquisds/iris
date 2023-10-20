using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    public bool isArmadillo;
    internal ArmadilloUI armadilloUI => GameObject.FindWithTag("ArmadilloUI").GetComponent<ArmadilloUI>();

    void OnTriggerEnter2D(Collider2D _collider)
    {
        if (_collider.gameObject.tag == "Player")
        {
            if (isArmadillo) { armadilloUI.AddCount(); }
            Destroy(gameObject);
        }
    }
}