using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class VictoryZone : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collider)
    {
        var sprite = this.GetComponent<SpriteRenderer>();
        var player = GameObject.FindWithTag("Player").GetComponent<PlayerControl>();

        if (collider.gameObject.tag == "Player")
        {
            sprite.enabled = false;
            player.controlEnabled = false;
            player.rb.velocity = Vector2.zero;
            player.anim.SetTrigger("victory");
            StartCoroutine(WaitForSceneLoad());
        }
    }

    private IEnumerator WaitForSceneLoad()
    {
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene((SceneManager.GetActiveScene().buildIndex) + 1);
    }
}