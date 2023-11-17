using System;
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
    public float fadeSpeed = 5, fadeInGoal = 1, fadeOutGoal = 0;
    internal bool usingSprite => TryGetComponent(out SpriteRenderer sprite);
    internal bool usingImage => TryGetComponent(out Image img);
    internal Color objColor => usingSprite ? GetComponent<SpriteRenderer>().color : usingImage ? GetComponent<Image>().color : GetComponent<Tilemap>().color;
    internal Color newColor;

    private void Awake()
    {
        hidden = startsVisible;
        if (usingSprite) GetComponent<SpriteRenderer>().color = new Color(objColor.r, objColor.g, objColor.b, Convert.ToInt32(hidden));
        else if (usingImage) GetComponent<Image>().color = new Color(objColor.r, objColor.g, objColor.b, Convert.ToInt32(hidden));
        else GetComponent<Tilemap>().color = new Color(objColor.r, objColor.g, objColor.b, Convert.ToInt32(hidden));
    }

    private void OnTriggerEnter2D(Collider2D collider) => Triggered(collider, true);
    private void OnTriggerExit2D(Collider2D collider) => Triggered(collider, false);
    void Triggered(Collider2D collider, bool entered)
    {
        if (collider.gameObject.tag == "Player") hidden = startsVisible ? !entered : entered;
    }

    void Update() => Fade(hidden);
    void Fade(bool fadeIn)
    {
        if ((fadeIn && objColor.a < fadeInGoal) || (!fadeIn && objColor.a > fadeOutGoal))
        {
            fadeAmount = objColor.a + ((fadeSpeed * Time.unscaledDeltaTime) * (fadeIn ? 1 : -1));
            newColor = new Color(objColor.r, objColor.g, objColor.b, fadeAmount);

            if (usingSprite) GetComponent<SpriteRenderer>().color = newColor;
            else if (usingImage) GetComponent<Image>().color = newColor;
            else GetComponent<Tilemap>().color = newColor;
        }
    }
}