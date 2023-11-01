using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuFade : MonoBehaviour
{
    internal bool canFade = false;
    internal Image image => GetComponent<Image>();

    void Awake() { Time.timeScale = 1; StartCoroutine(FadeAllow()); }

    void Update() { if (canFade) FadeOut(); }

    void FadeOut()
    {
        var imgColor = image.color;
        float fadeAmount = imgColor.a - (3f * Time.deltaTime);

        imgColor = new Color(imgColor.r, imgColor.g, imgColor.b, fadeAmount);

        if (imgColor.a <= 0) imgColor.a = 0;

        image.color = imgColor;
    }
    IEnumerator FadeAllow()
    {
        yield return new WaitForSeconds(0.5f);
        canFade = true;
    }
}
