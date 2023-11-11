using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuFade : MonoBehaviour
{
    internal bool canFade = false;
    internal Image img => GetComponent<Image>();
    internal Color imgColor => img.color;
    internal Color newColor;
    internal float fadeAmount;

    void Awake() { Time.timeScale = 1; StartCoroutine(FadeAllow()); }
    void Update() { if (canFade) FadeOut(); }

    IEnumerator FadeAllow()
    {
        yield return new WaitForSeconds(0.25f);
        canFade = true;
    }
    void FadeOut()
    {
        if (imgColor.a > 0)
        {
            fadeAmount = imgColor.a - (3 * Time.deltaTime);
            newColor = new Color(imgColor.r, imgColor.g, imgColor.b, fadeAmount);
            img.color = newColor;
        }
    }
}
