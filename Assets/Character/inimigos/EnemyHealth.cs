using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    internal SpriteRenderer sprite => GetComponent<SpriteRenderer>();
    public int health;
    internal int initialHealth;
    public Material defaultMaterial, whiteMaterial;
    public GameObject explosion;
    public bool isBlue, isPurple;
    internal Collider2D _collider => GetComponent<Collider2D>();
    public Bounds Bounds => _collider.bounds;
    internal PlayerControl player => GameObject.FindWithTag("Player").GetComponent<PlayerControl>();

    void Awake() => initialHealth = health;
    
    void OnCollisionEnter2D(Collision2D _collider)
    {
        if (_collider.gameObject.tag == "PlayerAttack")
        {
            if (health > 0)
            {
                health -= 1;
                StartCoroutine(Flicker());
            }
            else Explode();
        }

        if (_collider.gameObject.tag == "Player")
        {
            if (this.GetComponent<EnemyWalk>() == null) player.Hurt();
        }
    }

    void OnCollisionStay2D(Collision2D _collider)
    {
        if (_collider.gameObject.tag == "PlayerAttack")
        {
            if (health > 0)
            {
                health -= 1;
                StartCoroutine(Flicker());
            }
            else Explode();
        }

        if (_collider.gameObject.tag == "Player")
        {
            if (this.GetComponent<EnemyWalk>() == null) player.Hurt();
        }
    }

    IEnumerator Flicker()
    {
        sprite.material = whiteMaterial;
        yield return new WaitForSeconds(0.1f);
        sprite.material = defaultMaterial;
        yield return new WaitForSeconds(0.1f);
    }

    void Explode()
    {
        GameObject smoke = Instantiate(explosion, transform.position, transform.rotation);
        if (isBlue) smoke.GetComponent<Animator>().SetTrigger("blue");
        if (isPurple) smoke.GetComponent<Animator>().SetTrigger("purple");

        gameObject.SetActive(false);
    }
}