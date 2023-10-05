using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Platformer.Core.Simulation;
using Platformer.Model;
using Platformer.Core;
using UnityEngine.Tilemaps;

namespace Platformer.Gameplay
{

public class HiddenWall : MonoBehaviour
{
    internal Collider2D _collider;
    internal bool hidden = true;
    public float fadeSpeed;

    private void OnTriggerEnter2D(Collider2D _collider)
    {
        if (_collider.gameObject.tag == "Player")
        {
            hidden = false;
        }
    }

    private void OnTriggerExit2D(Collider2D _collider)
    {
        if (_collider.gameObject.tag == "Player")
        {
            hidden = true;
        }
    }

    void Update()
    {
        if (hidden)
        {
            FadeIn();
        }
        else
        {
            FadeOut();
        }
    }

    void FadeOut()
    {
        var objColor = GetComponent<Tilemap>().color;
        float fadeAmount = objColor.a - (fadeSpeed * Time.deltaTime);

        objColor = new Color(objColor.r, objColor.g, objColor.b, fadeAmount);

        if (objColor.a <= 0)
        {
            objColor = new Color(objColor.r, objColor.g, objColor.b, 0);
            GetComponent<Tilemap>().color = objColor;
        }
        else
        {
            GetComponent<Tilemap>().color = objColor;
        }
    }

    void FadeIn()
    {
        var objColor = GetComponent<Tilemap>().color;
        float fadeAmount = objColor.a + (fadeSpeed * Time.deltaTime);

        objColor = new Color(objColor.r, objColor.g, objColor.b, fadeAmount);
        
        if (objColor.a >= 1)
        {
            objColor = new Color(objColor.r, objColor.g, objColor.b, 1);
            GetComponent<Tilemap>().color = objColor;
        }
        else
        {
            GetComponent<Tilemap>().color = objColor;
        }
    }
}
}
