using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Collectable : MonoBehaviour
{
    public bool isArmadillo;
    internal bool collected;
    public int level => SceneManager.GetActiveScene().buildIndex;
    internal AudioSource audioSource => GetComponent<AudioSource>();
    public EdgeCollider2D playerEdgeCollider => GameObject.FindWithTag("Player").GetComponent<EdgeCollider2D>();
    internal CollectableUI collectableUI => GameObject.FindWithTag(isArmadillo ? "ArmadilloUI" : "CollectableUI").GetComponent<CollectableUI>();

    void Awake()
    {
        if (!isArmadillo) GetComponent<Animator>().SetTrigger(level == 1 ? "level1" : level == 2 ? "level2" : "level3");
        if (TryGetComponent(out Collider2D colli)) Physics2D.IgnoreCollision(colli, playerEdgeCollider, true);
    }

    void OnTriggerEnter2D(Collider2D _collider)
    {
        if (_collider.gameObject.tag == "Player")
        {
            if (isArmadillo)
            {
                audioSource.Play();
                GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
                GetComponent<Collider2D>().enabled = false;
                collected = true;
            }
            else Destroy(gameObject);
            collectableUI.AddCount();
        }
    }

    void Update() { if (collected && !audioSource.isPlaying) Destroy(gameObject); }
}