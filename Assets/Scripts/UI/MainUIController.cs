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
        public GameObject trackerObj;
        public GameObject[] panels;
        internal GameObject eventsObject => GameObject.FindWithTag("GameEvents");
        internal EventSystem events => eventsObject.GetComponent<EventSystem>();
        internal SubUIController subUIController => eventsObject.GetComponent<SubUIController>();
        internal GameObject gameCoreObject => GameObject.FindWithTag("GameCore");
        internal Tracker tracker;
        internal MetaGameController gameCore => gameCoreObject.GetComponent<MetaGameController>();
        internal bool lastFullscreenValue;

        void Start()
        {
            if (isMainMenu && GameObject.FindWithTag("Tracker") == null)
            {
                GameObject newTracker = Instantiate(trackerObj, transform.position, transform.rotation);
                tracker = newTracker.GetComponent<Tracker>();
            }
            else tracker = GameObject.FindWithTag("Tracker").GetComponent<Tracker>();
        }
        void OnEnable()
        {
            lastFullscreenValue = !Screen.fullScreen;
            if (!isMainMenu) SetActivePanel(0);
            else
            {
                StartCoroutine(subUIController.StartMusic());
                gameCoreObject.transform.GetChild(2).gameObject.SetActive(Debug.isDebugBuild);
            }
        }

        public void SetActivePanel(int index)
        {
            for (var i = 0; i < panels.Length; i++)
                if (panels[i].activeSelf != (i == index)) panels[i].SetActive(i == index);
            events.SetSelectedGameObject(null);
        }
        public void Unpause() => gameCore.ToggleMenu(false);
        public void LoadScene(int scene) => StartCoroutine(subUIController.Transition(true, scene, 0.3f));
        public void EnableCutscene(int enabled) => tracker.shouldPlayCutscene = enabled == 1;
        public void ToggleFullScreen(Toggle toggle) => Screen.fullScreen = toggle.isOn;
        public void Github() => Application.OpenURL("https://github.com/feliquisds/iris");
        public void Quit() => StartCoroutine(subUIController.Transition(false, 0, 0.6f));

        void Update()
        {
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