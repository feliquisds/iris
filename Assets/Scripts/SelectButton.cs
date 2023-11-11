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

        void Update() { if (mainMenu) MenuUpdate(); else PauseUpdate(); }

        void MenuUpdate()
        {
            if (events.currentSelectedGameObject == null)
            {
                if (UIController.panels[2].activeSelf)
                {
                    if (Input.GetAxis("Horizontal") != 0f || Input.GetAxis("Vertical") != 0f)
                        events.SetSelectedGameObject(fallbackButton);
                }

                else
                {
                    var activePanel = 0;
                    for (var i = 0; i < UIController.panels.Length; i++)
                    {
                        var g = UIController.panels[i];
                        if (g.activeSelf) activePanel = i;
                    }

                    if (Input.GetAxis("Horizontal") != 0f || Input.GetAxis("Vertical") != 0f)
                        events.SetSelectedGameObject(FindChildWithTag(UIController.panels[activePanel], "FallbackButton"));
                }
            }
        }

        void PauseUpdate()
        {
            if (events.currentSelectedGameObject == null && Time.timeScale != 1)
            {
                var activePanel = 0;
                for (var i = 0; i < UIController.panels.Length; i++)
                {
                    var g = UIController.panels[i];
                    if (g.activeSelf) activePanel = i;
                }

                if (Input.GetButtonDown("Horizontal") != false || Input.GetButtonDown("Vertical") != false)
                    events.SetSelectedGameObject(FindChildWithTag(UIController.panels[activePanel], "FallbackButton"));
            }
        }

        GameObject FindChildWithTag(GameObject parent, string tag)
        {
            GameObject child = null;

            foreach (Transform transform in parent.transform)
            {
                if (transform.CompareTag(tag))
                {
                    child = transform.gameObject;
                    break;
                }
            }

            return child;
        }
    }
}