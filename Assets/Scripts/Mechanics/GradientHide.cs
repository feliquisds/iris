using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class GradientHide : MonoBehaviour
{
    public bool hidden;
    internal float fadeAmount;
    public bool startsVisible = true;
    public float fadeSpeed = 5;
    internal bool usingSprite => TryGetComponent(out SpriteRenderer sprite);
    internal bool usingImage => TryGetComponent(out Image img);
    internal Color objColor => usingSprite ? GetComponent<SpriteRenderer>().color : usingImage ? GetComponent<Image>().color : GetComponent<Tilemap>().color;
    internal Color newColor;

    private void Awake()
    {
        if (startsVisible) hidden = true;
        else
        {
            hidden = false;
            if (usingSprite) GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
            else if (usingImage) GetComponent<Image>().color = new Color(1, 1, 1, 0);
            else GetComponent<Tilemap>().color = new Color(1, 1, 1, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D _collider)
    {
        if (_collider.gameObject.tag == "Player") hidden = startsVisible ? false : true;
    }
    private void OnTriggerExit2D(Collider2D _collider)
    {
        if (_collider.gameObject.tag == "Player") hidden = startsVisible ? true : false;
    }

    void Update()
    {
        if (hidden) FadeIn();
        else FadeOut();
    }
    void FadeIn()
    {
        if (objColor.a < 1)
        {
            fadeAmount = objColor.a + (fadeSpeed * Time.unscaledDeltaTime);
            newColor = new Color(objColor.r, objColor.g, objColor.b, fadeAmount);

            if (usingSprite) GetComponent<SpriteRenderer>().color = newColor;
            else if (usingImage) GetComponent<Image>().color = newColor;
            else GetComponent<Tilemap>().color = newColor;
        }
    }
    void FadeOut()
    {
        if (objColor.a > 0)
        {
            fadeAmount = objColor.a - (fadeSpeed * Time.unscaledDeltaTime);
            newColor = new Color(objColor.r, objColor.g, objColor.b, fadeAmount);

            if (usingSprite) GetComponent<SpriteRenderer>().color = newColor;
            else if (usingImage) GetComponent<Image>().color = newColor;
            else GetComponent<Tilemap>().color = newColor;
        }
    }
}