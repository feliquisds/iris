using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class VictoryZone : MonoBehaviour
{

    public bool walk;
    internal bool hasSprite => TryGetComponent<SpriteRenderer>(out SpriteRenderer _sprite);
    internal PlayerControl player => GameObject.FindWithTag("Player").GetComponent<PlayerControl>();

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            player.controlEnabled = false;
            StartCoroutine(WaitForSceneLoad());
            if (hasSprite) GetComponent<SpriteRenderer>().enabled = false;

            if (!walk)
            {
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