using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class GradientHide : MonoBehaviour
{
    public bool opaque;
    internal float fadeAmount;
    public bool startsVisible = true, unscaledTime = false;
    public float fadeSpeed = 5, fadeInGoal = 1, fadeOutGoal = 0;
    internal bool usingSprite => TryGetComponent(out SpriteRenderer sprite);
    internal bool usingImage => TryGetComponent(out Image img);
    internal SpriteRenderer sprite => GetComponent<SpriteRenderer>();
    internal Tilemap tiles => GetComponent<Tilemap>();
    internal Image img => GetComponent<Image>();
    internal Color objColor => usingSprite ? GetComponent<SpriteRenderer>().color : usingImage ? GetComponent<Image>().color : GetComponent<Tilemap>().color;
    internal Color newColor;

    private void Awake()
    {
        opaque = startsVisible;
        if (usingSprite) sprite.color = new Color(objColor.r, objColor.g, objColor.b, Convert.ToInt32(opaque));
        else if (usingImage) img.color = new Color(objColor.r, objColor.g, objColor.b, Convert.ToInt32(opaque));
        else tiles.color = new Color(objColor.r, objColor.g, objColor.b, Convert.ToInt32(opaque));
    }

    private void OnTriggerEnter2D(Collider2D collider) => Triggered(collider, true);
    private void OnTriggerExit2D(Collider2D collider) => Triggered(collider, false);
    void Triggered(Collider2D collider, bool entered)
    {
        if (collider.gameObject.tag == "Player") opaque = startsVisible ? !entered : entered;
    }

    void Update() => Fade(opaque);
    void Fade(bool fadeIn)
    {
        if ((fadeIn && objColor.a < fadeInGoal) || (!fadeIn && objColor.a > fadeOutGoal))
        {
            fadeAmount = objColor.a + ((fadeSpeed * (unscaledTime ? Time.unscaledDeltaTime : Time.deltaTime)) * (fadeIn ? 1 : -1));
            newColor = new Color(objColor.r, objColor.g, objColor.b, fadeAmount);

            if (usingSprite) sprite.color = newColor;
            else if (usingImage) img.color = newColor;
            else tiles.color = newColor;
        }
    }
}