using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

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
        internal GameObject activePanel;
        internal bool locked;

        void Awake() => StartCoroutine(FadeStart());
        IEnumerator FadeStart()
        {
            locked = true;
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
            if (events.currentSelectedGameObject == null)
            {
                if (mainMenu) MenuUpdate();
                else PauseUpdate();
            }
            if (!mainMenu && locked) player.controlEnabled = player.canCrouch = false;
        }

        void MenuUpdate()
        {
            if (Input.GetAxis("Horizontal") != 0f || Input.GetAxis("Vertical") != 0f)
            {
                if (UIController.panels[2].activeSelf) events.SetSelectedGameObject(fallbackButton);
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

        void PauseUpdate()
        {
            if (Time.timeScale != 1)
            {
                if (Input.GetButtonDown("Horizontal") != false || Input.GetButtonDown("Vertical") != false)
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
            {
                if (transform.CompareTag(tag)) return transform.gameObject;
            }
            return null;
        }
    }
}