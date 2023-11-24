using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace Platformer.UI
{
    public class SubUIController : MonoBehaviour
    {
        public GameObject fallbackButton, UIControllerObject;
        public MainUIController UIController => UIControllerObject.GetComponent<MainUIController>();
        internal EventSystem events => GetComponent<EventSystem>();
        internal GameObject playerObject => GameObject.FindWithTag("Player");
        internal PlayerControl player => playerObject.GetComponent<PlayerControl>();
        public bool mainMenu => playerObject == null;
        internal GradientHide fade => GameObject.FindWithTag("Fade").GetComponent<GradientHide>();
        internal AudioSource audioSource => GameObject.FindWithTag("GameCore").GetComponent<AudioSource>();
        internal GameObject activePanel;
        internal bool locked = true, musicFadeOut = false;
        internal bool mouseClick => Input.GetKey(KeyCode.Mouse0) || Input.GetKey(KeyCode.Mouse1) || Input.GetKey(KeyCode.Mouse2);
        internal bool mouseMove => Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0;

        void Awake() => StartCoroutine(FadeStart());
        IEnumerator FadeStart()
        {
            Time.timeScale = 1;
            fade.hidden = true;
            yield return new WaitForSeconds(0.5f);

            fade.hidden = false;
            yield return new WaitForSeconds(0.3f);

            locked = false;
            if (!mainMenu) player.controlEnabled = player.canCrouch = true;
        }

        void Update()
        {
            if (events.currentSelectedGameObject == null) UpdateSelection();
            if (!mainMenu && locked) player.controlEnabled = player.canCrouch = false;

            Cursor.visible = Cursor.visible && Input.anyKey && !mouseClick ? false : mouseMove || mouseClick ? true : Cursor.visible;
            if (musicFadeOut && audioSource.volume > 0) audioSource.volume -= (2 * Time.unscaledDeltaTime);

            if (Input.GetButtonDown("Cancel"))
            {
                if (!mainMenu && !UIController.panels[0].activeSelf) UIController.Unpause();
                else UIController.SetActivePanel(0);
            }
        }

        void UpdateSelection()
        {
            if (((Time.timeScale != 1 && !mainMenu) || (mainMenu)) &&
                (Input.GetButtonDown("Horizontal") || Input.GetButtonDown("Vertical")))
            {
                if (UIController.panels[0].activeSelf) events.SetSelectedGameObject(fallbackButton);
                else
                {
                    foreach (GameObject panel in UIController.panels)
                    {
                        activePanel = panel.activeSelf ? panel : activePanel;
                    }
                    events.SetSelectedGameObject(FindChildWithTag(activePanel, "FallbackButton"));
                }
            }
        }
        GameObject FindChildWithTag(GameObject parent, string tag)
        {
            foreach (Transform transform in parent.transform)
                if (transform.CompareTag(tag)) return transform.gameObject;
            return null;
        }

        public IEnumerator StartMusic()
        {
            yield return new WaitForSecondsRealtime(0.5f);
            audioSource.Play();
        }
        public IEnumerator Transition(bool sceneTransition, int scene, float time)
        {
            fade.hidden = musicFadeOut = true;
            yield return new WaitForSecondsRealtime(time);
            if (sceneTransition) SceneManager.LoadScene(scene);
            else Application.Quit();
        }
    }
}