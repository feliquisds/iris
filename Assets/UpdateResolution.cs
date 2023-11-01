using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateResolution : MonoBehaviour
{
    private float lastWidth, lastHeight;
    void Update()
    {
        if (lastWidth != Screen.width && !Screen.fullScreen)
        {
            var newWidth = Screen.width * (9f / 16f);
            Screen.SetResolution(Screen.width, (int)newWidth, false);
        }
        else if (lastHeight != Screen.height && !Screen.fullScreen)
        {
            var newHeight = Screen.height * (16f / 9f);
            Screen.SetResolution((int)newHeight, Screen.height, false);
        }

        lastWidth = Screen.width;
        lastHeight = Screen.height;
    }
}
