using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    internal SpriteRenderer sprite => GetComponent<SpriteRenderer>();
    public int health;
    public Material defaultMaterial, whiteMaterial;


    void OnCollisionEnter2D(Collision2D _collider)
    {
        if (_collider.gameObject.tag == "PlayerAttack")
        {
            if (health > 0)
            {
                health -= 1;
                StartCoroutine(Flicker());
            }
            else Destroy(gameObject);
        }
    }

    IEnumerator Flicker()
    {
        sprite.material = whiteMaterial;
        yield return new WaitForSeconds(0.1f);
        sprite.material = defaultMaterial;
        yield return new WaitForSeconds(0.1f);
    }
}