using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class VictoryZone : MonoBehaviour
{

    public bool walk;
    internal PlayerControl player => GameObject.FindWithTag("Player").GetComponent<PlayerControl>();

    void OnTriggerEnter2D(Collider2D collider)
    {
        var sprite = this.GetComponent<SpriteRenderer>();

        if (collider.gameObject.tag == "Player")
        {
            player.controlEnabled = false;
            StartCoroutine(WaitForSceneLoad());

            if (!walk)
            {
                sprite.enabled = false;
                player.rb.velocity = Vector2.zero;
                player.anim.SetTrigger("victory");
            }
            else player.winning = true;
        }
    }

    private IEnumerator WaitForSceneLoad()
    {
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene((SceneManager.GetActiveScene().buildIndex) + 1);
    }
}