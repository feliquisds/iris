using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class CollectableUI : MonoBehaviour
{
    public GameObject collectables => GameObject.FindWithTag("Collectables");
    internal int collectableCount => collectables.transform.childCount;
    internal Transform gameCore => GameObject.FindWithTag("GameCore").transform;
    internal Image img => GetComponent<Image>();
    internal float movement => Screen.width / 1920f;
    internal int collectableIndicator = 0, armadilloCount = 0;
    public TMP_Text textCount;
    public bool isArmadillo;
    internal bool move = false;
    internal Vector3 newTransform;

    public void AddCount()
    {
        if (isArmadillo)
        {
            armadilloCount += 1;
            newTransform = transform.TransformPoint(gameCore.position) - new Vector3(200f * movement, 0, 0);
            move = true;
        }
        else
        {
            collectableIndicator += 1;
            textCount.text = "x" + collectableIndicator;
            if (collectableCount == 1) img.color = textCount.color = new Color(1, 1, 0, 1);
        }
    }

    void Update()
    {
        if (move && isArmadillo)
        {
            transform.position = Vector3.MoveTowards(transform.position, newTransform, 700f * Time.deltaTime);
            if (transform.position == newTransform) move = false;
        }
    }
}
