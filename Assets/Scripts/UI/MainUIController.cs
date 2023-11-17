using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

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
        internal bool musicFadeOut = false;

        void OnEnable()
        {
            if (!isMainMenu) SetActivePanel(0);
            else StartCoroutine(StartMusic());
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
        public void LoadScene(int scene) => StartCoroutine(Transition(true, scene));
        public void ToggleFullScreen() => Screen.fullScreen = !Screen.fullScreen;
        public void Github() => Application.OpenURL("https://github.com/feliquisds/iris");
        public void Quit() => StartCoroutine(Transition(false, 0));

        IEnumerator Transition(bool sceneTransition, int scene)
        {
            fade.hidden = musicFadeOut = true;
            yield return new WaitForSecondsRealtime(0.3f);
            if (sceneTransition) SceneManager.LoadScene(scene);
            else Application.Quit();
        }
        void Update()
        {
            if (musicFadeOut && audioSource.volume > 0) audioSource.volume -= (2f * Time.unscaledDeltaTime);
        }
    }
}