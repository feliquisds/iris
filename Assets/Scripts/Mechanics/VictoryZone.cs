using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class VictoryZone : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collider)
    {
        var sprite = this.GetComponent<SpriteRenderer>();
        if (collider.gameObject.tag == "Player")
        {
            sprite.enabled = false;
            StartCoroutine(WaitForSceneLoad());
        }
    }

    private IEnumerator WaitForSceneLoad()
    {
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene((SceneManager.GetActiveScene().buildIndex) + 1);
    }
}