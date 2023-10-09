using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Platformer.Core.Simulation;
using Platformer.Model;
using Platformer.Core;
using UnityEngine.Tilemaps;

namespace Platformer.Gameplay
{

public class GradientHide : MonoBehaviour
{
    internal Collider2D _collider;
    internal bool hidden;
    public bool startsVisible = true;
    public float fadeSpeed = 5;
    internal bool usingSprite;

    private void Awake()
    {
        if (startsVisible) hidden = true;
        else
        {
            hidden = false;
            var obj = GetComponent<SpriteRenderer>();
            var objColor = obj.color;
            obj.color = new Color(objColor.r, objColor.g, objColor.b, 0);
        }

        usingSprite = TryGetComponent(out SpriteRenderer sprite);
    }

    private void OnTriggerEnter2D(Collider2D _collider)
    {
        if (_collider.gameObject.tag == "Player")
        {
            if (startsVisible) hidden = false;
            else hidden = true;
        }
    }

    private void OnTriggerExit2D(Collider2D _collider)
    {
        if (_collider.gameObject.tag == "Player")
        {
            if (startsVisible) hidden = true;
            else hidden = false;
        }
    }

    void Update()
    {
        if (hidden)
        {
            if (usingSprite) FadeInSprite();
            FadeInTile();
        }
        else
        {
            if (usingSprite) FadeOutSprite();
            FadeOutTile();
        }
    }

    void FadeOutSprite()
    {
        var obj = GetComponent<SpriteRenderer>();
        var objColor = obj.color;
        float fadeAmount = objColor.a - (fadeSpeed * Time.deltaTime);

        objColor = new Color(objColor.r, objColor.g, objColor.b, fadeAmount);

        if (objColor.a <= 0) objColor.a = 0;
        //objColor = new Color(objColor.r, objColor.g, objColor.b, 0);

        obj.color = objColor;
    }

    void FadeInSprite()
    {
        var obj = GetComponent<SpriteRenderer>();
        var objColor = obj.color;
        float fadeAmount = objColor.a + (fadeSpeed * Time.deltaTime);

        objColor = new Color(objColor.r, objColor.g, objColor.b, fadeAmount);
        
        if (objColor.a >= 1) objColor.a = 1;
        //objColor = new Color(objColor.r, objColor.g, objColor.b, 1);
            
        obj.color = objColor;
    }

    void FadeOutTile()
    {
        var obj = GetComponent<Tilemap>();
        var objColor = obj.color;
        float fadeAmount = objColor.a - (fadeSpeed * Time.deltaTime);

        objColor = new Color(objColor.r, objColor.g, objColor.b, fadeAmount);

        if (objColor.a <= 0) objColor.a = 0;
        //objColor = new Color(objColor.r, objColor.g, objColor.b, 0);

        obj.color = objColor;
    }

    void FadeInTile()
    {
        var obj = GetComponent<Tilemap>();
        var objColor = obj.color;
        float fadeAmount = objColor.a + (fadeSpeed * Time.deltaTime);

        objColor = new Color(objColor.r, objColor.g, objColor.b, fadeAmount);
        
        if (objColor.a >= 1) objColor.a = 1;
        //objColor = new Color(objColor.r, objColor.g, objColor.b, 1);
            
        obj.color = objColor;
    }
}
}
