using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using Platformer.UI;

public class VictoryZone : MonoBehaviour
{
    public bool walk;
    internal bool hasSprite => TryGetComponent<SpriteRenderer>(out SpriteRenderer _sprite);
    internal PlayerControl player => GameObject.FindWithTag("Player").GetComponent<PlayerControl>();
    internal SubUIController subUIController => GameObject.FindWithTag("GameEvents").GetComponent<SubUIController>();
    internal MetaGameController gameController => GameObject.FindWithTag("GameCore").GetComponent<MetaGameController>();
    public GameObject insectWave;

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            player.controlEnabled = player.canCrouch = player.canBeHurt = gameController.canPause = false;
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
        yield return new WaitForSeconds(1.7f);
        if (insectWave != null) insectWave.GetComponent<InsectWave>().sceneEnding = true;
        StartCoroutine(subUIController.Transition(true, (SceneManager.GetActiveScene().buildIndex) + 1, 0.3f));
    }
}