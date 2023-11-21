using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Platformer.UI
{
    public class MainUIController : MonoBehaviour
    {
        public bool isMainMenu => eventsObject.GetComponent<SubUIController>().mainMenu;
        public GameObject[] panels;
        internal GameObject eventsObject => GameObject.FindWithTag("GameEvents");
        internal EventSystem events => eventsObject.GetComponent<EventSystem>();
        internal GameObject gameCoreObject => GameObject.FindWithTag("GameCore");
        internal MetaGameController gameCore => gameCoreObject.GetComponent<MetaGameController>();
        internal AudioSource audioSource => gameCoreObject.GetComponent<AudioSource>();
        internal GradientHide fade => GameObject.FindWithTag("Fade").GetComponent<GradientHide>();
        internal bool musicFadeOut = false, lastFullscreenValue;

        void OnEnable()
        {
            lastFullscreenValue = !Screen.fullScreen;
            if (!isMainMenu) SetActivePanel(0);
            else
            {
                StartCoroutine(StartMusic());
                gameCoreObject.transform.GetChild(2).gameObject.SetActive(Debug.isDebugBuild);
            }
        }
        IEnumerator StartMusic()
        {
            yield return new WaitForSecondsRealtime(0.5f);
            audioSource.Play();
        }

        public void SetActivePanel(int index)
        {
            for (var i = 0; i < panels.Length; i++)
                if (panels[i].activeSelf != (i == index)) panels[i].SetActive(i == index);
            events.SetSelectedGameObject(null);
        }
        public void Unpause() => gameCore.ToggleMenu(false);
        public void LoadScene(int scene) => StartCoroutine(Transition(true, scene, 0.3f));
        public void ToggleFullScreen(Toggle toggle) => Screen.fullScreen = toggle.isOn;
        public void Github() => Application.OpenURL("https://github.com/feliquisds/iris");
        public void Quit() => StartCoroutine(Transition(false, 0, 0.6f));

        IEnumerator Transition(bool sceneTransition, int scene, float time)
        {
            fade.hidden = musicFadeOut = true;
            yield return new WaitForSecondsRealtime(time);
            if (sceneTransition) SceneManager.LoadScene(scene);
            else Application.Quit();
        }
        void Update()
        {
            if (musicFadeOut && audioSource.volume > 0) audioSource.volume -= (2f * Time.unscaledDeltaTime);
            if (!isMainMenu) this.gameObject.transform.GetChild(1).gameObject.SetActive(panels[0].activeSelf);
            if (panels[1].activeSelf && lastFullscreenValue != Screen.fullScreen)
            {
                lastFullscreenValue = Screen.fullScreen;
                foreach (Transform transform in panels[1].transform)
                    if (transform.gameObject.TryGetComponent(out Toggle toggle)) toggle.isOn = lastFullscreenValue;
            }
        }
    }
}