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
        internal MetaGameController gameCore => GameObject.FindWithTag("GameCore").GetComponent<MetaGameController>();
        internal GradientHide fade => GameObject.FindWithTag("Fade").GetComponent<GradientHide>();

        public void SetActivePanel(int index)
        {
            for (var i = 0; i < panels.Length; i++)
                if (panels[i].activeSelf != (i == index)) panels[i].SetActive(i == index);
            events.SetSelectedGameObject(null);
        }
        void OnEnable() { if (!isMainMenu) SetActivePanel(0); }
        public void Unpause() => gameCore.ToggleMainMenu(false);

        public void LoadScene(int scene) => StartCoroutine(SceneTransition(scene));
        IEnumerator SceneTransition(int scene)
        {
            fade.hidden = true;
            yield return new WaitForSecondsRealtime(0.3f);
            SceneManager.LoadScene(scene);
        }

        public void ToggleFullScreen() => Screen.fullScreen = !Screen.fullScreen;
        public void Github() => Application.OpenURL("https://github.com/feliquisds/iris");
        public void Quit() => Application.Quit();
    }
}