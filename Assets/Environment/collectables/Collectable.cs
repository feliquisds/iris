using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    public bool isArmadillo;
    public int level;
    internal ArmadilloUI armadilloUI => GameObject.FindWithTag("ArmadilloUI").GetComponent<ArmadilloUI>();

    void Awake() { if (!isArmadillo) GetComponent<Animator>().SetTrigger(level == 1 ? "level1" : level == 2 ? "level2" : "level3"); }

    void OnTriggerEnter2D(Collider2D _collider)
    {
        if (_collider.gameObject.tag == "Player")
        {
            if (isArmadillo) { armadilloUI.AddCount(); }
            Destroy(gameObject);
        }
    }
}