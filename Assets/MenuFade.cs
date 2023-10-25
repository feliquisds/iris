using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuFade : MonoBehaviour
{
    internal bool canFade = false;
    internal Image image => GetComponent<Image>();

    void Update()
    {
        if (canFade)
        {
            var imgColor = image.color;
            float fadeAmount = imgColor.a - (3f * Time.deltaTime);

            imgColor = new Color(imgColor.r, imgColor.g, imgColor.b, fadeAmount);

            if (imgColor.a <= 0) imgColor.a = 0;

            image.color = imgColor;
        }
        else StartCoroutine(FadeOut());
    }

    IEnumerator FadeOut()
    {
        yield return new WaitForSeconds(0.5f);
        canFade = true;
    }
}
