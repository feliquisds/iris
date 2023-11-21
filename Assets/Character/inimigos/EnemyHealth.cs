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
    public Vector3 explosionOffset;
    public bool isBlue, isPurple;
    internal Collider2D _collider => GetComponent<Collider2D>();
    public Bounds Bounds => _collider.bounds;
    internal PlayerControl player => GameObject.FindWithTag("Player").GetComponent<PlayerControl>();
    internal bool hasEnemyWalk => TryGetComponent<EnemyWalk>(out EnemyWalk enemy);

    void Awake() => initialHealth = health;
    void Respawn() => health = initialHealth;
    void Update() { if (player.respawning) Respawn(); }
    
    void OnCollisionEnter2D(Collision2D collider) => Collided(collider);
    void OnCollisionStay2D(Collision2D collider) => Collided(collider);
    void Collided(Collision2D collider)
    {
        if (collider.gameObject.tag == "PlayerAttack")
        {
            if (health > 0)
            {
                health -= 1;
                StartCoroutine(Flicker());
            }
            else Explode();
        }
        if (collider.gameObject.tag == "Player" && !hasEnemyWalk) player.Hurt();
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
        GameObject smoke = Instantiate(explosion, transform.position + explosionOffset, transform.rotation);
        if (isBlue) smoke.GetComponent<Animator>().SetTrigger("blue");
        if (isPurple) smoke.GetComponent<Animator>().SetTrigger("purple");

        gameObject.SetActive(false);
    }
}