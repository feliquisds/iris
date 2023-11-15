using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class VictoryZone : MonoBehaviour
{

    public bool walk;
    internal bool hasSprite => TryGetComponent<SpriteRenderer>(out SpriteRenderer _sprite);
    internal PlayerControl player => GameObject.FindWithTag("Player").GetComponent<PlayerControl>();
    internal GradientHide fade => GameObject.FindWithTag("Fade").GetComponent<GradientHide>();

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            player.controlEnabled = player.canBeHurt = false;
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
        fade.hidden = true;

        yield return new WaitForSeconds(0.3f);
        SceneManager.LoadScene((SceneManager.GetActiveScene().buildIndex) + 1);
    }
}