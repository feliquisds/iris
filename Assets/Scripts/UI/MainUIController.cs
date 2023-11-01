using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Platformer.UI
{
    /// <summary>
    /// A simple controller for switching between UI panels.
    /// </summary>
    public class MainUIController : MonoBehaviour
    {
        public GameObject[] panels;

        public void SetActivePanel(int index)
        {
            for (var i = 0; i < panels.Length; i++)
            {
                var active = i == index;
                var g = panels[i];
                if (g.activeSelf != active) g.SetActive(active);
            }
        }

        public void Unpause()
        {
            var menu = GameObject.FindWithTag("GameCore").GetComponent<MetaGameController>();
            menu.ToggleMainMenu(false);
        }

        public void MainMenu() => SceneManager.LoadScene(0);

        public void ToggleFullScreen() => Screen.fullScreen = !Screen.fullScreen;

        public void StartGame() => SceneManager.LoadScene(1);

        void OnEnable()
        {
            SetActivePanel(1);
        }
    }
}