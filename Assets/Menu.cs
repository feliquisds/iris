using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    internal bool canFade = false;
    internal Image image => GameObject.FindWithTag("Fade").GetComponent<Image>();

    void Update()
    {
        if (Input.GetButton("Jump")) SceneManager.LoadScene(1);
        
        if (canFade) FadeOut();
        else StartCoroutine(FadeAllow());
    }

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
