using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

namespace Platformer.UI
{
    /// <summary>
    /// A simple controller for switching between UI panels.
    /// </summary>
    public class MainUIController : MonoBehaviour
    {
        public bool isMainMenu = false;
        public GameObject[] panels;
        internal EventSystem events => GameObject.FindWithTag("GameEvents").GetComponent<EventSystem>();

        public void SetActivePanel(int index)
        {
            for (var i = 0; i < panels.Length; i++)
            {
                var active = i == index;
                var g = panels[i];
                if (g.activeSelf != active) g.SetActive(active);
            }
            events.SetSelectedGameObject(null);
        }

        public void Unpause()
        {
            var menu = GameObject.FindWithTag("GameCore").GetComponent<MetaGameController>();
            menu.ToggleMainMenu(false);
        }

        public void LoadScene(int scene) => SceneManager.LoadScene(scene);

        public void ToggleFullScreen() => Screen.fullScreen = !Screen.fullScreen;

        public void Quit() => Application.Quit();

        public void Github() => Application.OpenURL("https://github.com/feliquisds/iris");

        void OnEnable()
        {
            if (!isMainMenu) SetActivePanel(0);
        }
    }
}