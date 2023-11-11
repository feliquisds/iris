using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Platformer.UI
{
    public class SelectButton : MonoBehaviour
    {
        public GameObject fallbackButton, UIControllerObject;
        public MainUIController UIController => UIControllerObject.GetComponent<MainUIController>();
        public bool mainMenu;
        internal EventSystem events => GetComponent<EventSystem>();
        internal GameObject activePanel;

        void Update()
        {
            if (events.currentSelectedGameObject == null)
            {
                if (mainMenu) MenuUpdate();
                else PauseUpdate();
            }
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
                if (transform.CompareTag(tag))
                {
                    return transform.gameObject;
                }
            }
            return null;
        }
    }
}