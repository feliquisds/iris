using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Collectable : MonoBehaviour
{
    public bool isArmadillo;
    public EdgeCollider2D playerEdgeCollider => GameObject.FindWithTag("Player").GetComponent<EdgeCollider2D>();
    public int level => SceneManager.GetActiveScene().buildIndex;
    internal CollectableUI collectableUI => GameObject.FindWithTag(isArmadillo ? "ArmadilloUI" : "CollectableUI").GetComponent<CollectableUI>();

    void Awake()
    {
        if (!isArmadillo) GetComponent<Animator>().SetTrigger(level == 1 ? "level1" : level == 2 ? "level2" : "level3");
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), playerEdgeCollider, true);
    }

    void OnTriggerEnter2D(Collider2D _collider)
    {
        if (_collider.gameObject.tag == "Player")
        {
            Destroy(gameObject);
            collectableUI.AddCount();
        }
    }
}