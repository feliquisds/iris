using System.Collections;
using System.Collections.Generic;
using Platformer.Mechanics;
using Platformer.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;

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
        internal Volume volume => GameObject.FindWithTag("MainCamera").GetComponent<Volume>();
        internal AudioLowPassFilter filter => GetComponent<AudioLowPassFilter>();
        internal AudioSource audioSource => GetComponent<AudioSource>();
        public bool canPause = true;

        void OnEnable() => StartCoroutine(StartMusic());
        IEnumerator StartMusic()
        {
            yield return new WaitForSecondsRealtime(0.5f);
            audioSource.Play();
        }

        public void ToggleMenu(bool show)
        {
            if (showMainCanvas != show)
            {
                volume.enabled = show;
                filter.enabled = show;
                Time.timeScale = show ? 0 : 1;
                mainMenu.gameObject.SetActive(show);
                foreach (var i in gamePlayCanvasii) i.gameObject.SetActive(!show);
                if (show) player.controlEnabled = false;
                else StartCoroutine(UnlockPlayer());
                showMainCanvas = show;
            }
        }
        IEnumerator UnlockPlayer()
        {
            yield return new WaitForSeconds(0.01f);
            player.controlEnabled = true;
        }

        void Update() { if (Input.GetButtonDown("Menu") && canPause) ToggleMenu(!showMainCanvas); }
    }
}
