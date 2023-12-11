using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

namespace Platformer.UI
{
    public class SubUIController : MonoBehaviour
    {
        public GameObject fallbackButton, cutsceneText, UIControllerObject;
        public MainUIController UIController => UIControllerObject.GetComponent<MainUIController>();
        internal EventSystem events => GetComponent<EventSystem>();
        internal GameObject playerObject => GameObject.FindWithTag("Player");
        internal PlayerControl player => playerObject.GetComponent<PlayerControl>();
        public int nextScene;
        public bool mainMenu => playerObject == null;
        public bool cutscene => TryGetComponent(out VideoPlayer video);
        internal float mouseExpireTime = 3f;
        internal VideoPlayer video => GetComponent<VideoPlayer>();
        internal GradientHide fade => GameObject.FindWithTag("Fade").GetComponent<GradientHide>();
        internal AudioSource audioSource => GameObject.FindWithTag("GameCore").GetComponent<AudioSource>();
        internal GameObject activePanel;
        internal bool locked = true, musicFadeOut = false, transitioning = false;
        internal bool mouseClick => Input.GetKey(KeyCode.Mouse0) || Input.GetKey(KeyCode.Mouse1) || Input.GetKey(KeyCode.Mouse2);
        internal bool mouseMove => Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0;
        internal MetaGameController gameController => GameObject.FindWithTag("GameCore").GetComponent<MetaGameController>();

        void Start() { if (cutscene) video.loopPointReached += EndReached; }
        void Awake() => StartCoroutine(FadeStart());
        IEnumerator FadeStart()
        {
            Time.timeScale = 1;
            if (!cutscene)
            {
                fade.opaque = true;
                yield return new WaitForSeconds(0.5f);

                fade.opaque = false;
                yield return new WaitForSeconds(0.3f);

                locked = false;
                if (!mainMenu) player.controlEnabled = player.canCrouch = gameController.canPause = true;
                else Time.timeScale = 0;
            }
            else
            {
                Cursor.visible = false;
                StartCoroutine(CutsceneButtonDisappear());
            }
        }

        void Update()
        {
            if (!cutscene)
            {
                if (events.currentSelectedGameObject == null) UpdateSelection();
                if (!mainMenu && locked) player.controlEnabled = player.canCrouch = gameController.canPause = false;

                if (Input.GetButtonDown("Cancel") && !transitioning)
                {
                    if (!mainMenu && !UIController.panels[0].activeSelf) UIController.Unpause();
                    else UIController.SetActivePanel(0);
                }
            }
            else
            {
                if (Input.anyKey)
                {
                    if (!cutsceneText.activeSelf)
                    {
                        cutsceneText.SetActive(true);
                        StartCoroutine(CutsceneButtonDisappear());
                    }
                    else if (Input.GetButtonDown("Jump")) StartCoroutine(Transition(true, nextScene, 0.3f));
                }
            }

            if (musicFadeOut && audioSource.volume > 0) audioSource.volume -= (2 * Time.unscaledDeltaTime);
            Cursor.visible = Cursor.visible && Input.anyKey && !mouseClick ? false : mouseMove || mouseClick ? true : Cursor.visible;

            if (Cursor.visible)
            {
                if (mouseMove) mouseExpireTime = 3f;
                else mouseExpireTime = mouseExpireTime <= 0 ? 0 : mouseExpireTime - Time.unscaledDeltaTime;
                if (mouseExpireTime <= 0 && Time.timeScale == 1) Cursor.visible = false;
            }
        }

        IEnumerator CutsceneButtonDisappear()
        {
            yield return new WaitForSecondsRealtime(2);
            cutsceneText.SetActive(false);
        }
        void EndReached(VideoPlayer vp) => SceneManager.LoadScene(nextScene);

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
            transitioning = true;
            fade.opaque = musicFadeOut = true;
            if (gameController != null) gameController.canPause = false;
            yield return new WaitForSecondsRealtime(scene == 5 ? 1 : time);
            if (sceneTransition) SceneManager.LoadScene(scene);
            else Application.Quit();
        }
    }
}