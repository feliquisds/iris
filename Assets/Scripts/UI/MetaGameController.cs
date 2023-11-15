using System.Collections;
using System.Collections.Generic;
using Platformer.Mechanics;
using Platformer.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Platformer.UI
{
    /// <summary>
    /// The MetaGameController is responsible for switching control between the high level
    /// contexts of the application, eg the Main Menu and Gameplay systems.
    /// </summary>
    public class MetaGameController : MonoBehaviour
    {
        /// <summary>
        /// The main UI object which used for the menu.
        /// </summary>
        public MainUIController mainMenu;

        /// <summary>
        /// A list of canvas objects which are used during gameplay (when the main ui is turned off)
        /// </summary>
        public Canvas[] gamePlayCanvasii;

        /// <summary>
        /// The game controller.
        /// </summary>
        public GameController gameController;

        bool showMainCanvas = false;
        internal PlayerControl player => GameObject.FindWithTag("Player").GetComponent<PlayerControl>();
        internal GameObject gameCore => GameObject.FindWithTag("GameCore");
        internal AudioLowPassFilter filter => gameCore.GetComponent<AudioLowPassFilter>();
        
        void OnEnable() => _ToggleMainMenu(showMainCanvas);

        /// <summary>
        /// Turn the main menu on or off.
        /// </summary>
        /// <param name="show"></param>
        public void ToggleMainMenu(bool show) { if (this.showMainCanvas != show) _ToggleMainMenu(show); }

        void _ToggleMainMenu(bool show)
        {
            if (show)
            {
                filter.enabled = true;
                Time.timeScale = 0;
                mainMenu.gameObject.SetActive(true);
                foreach (var i in gamePlayCanvasii) i.gameObject.SetActive(false);
                player.controlEnabled = false;
            }
            else
            {
                filter.enabled = false;
                Time.timeScale = 1;
                mainMenu.gameObject.SetActive(false);
                foreach (var i in gamePlayCanvasii) i.gameObject.SetActive(true);
                StartCoroutine(UnlockPlayer());
            }
            this.showMainCanvas = show;
        }

        void Update() { if (Input.GetButtonDown("Menu")) ToggleMainMenu(show: !showMainCanvas); }

        IEnumerator UnlockPlayer()
        {
            yield return new WaitForSeconds(0.01f);
            player.controlEnabled = true;
        }

    }
}
