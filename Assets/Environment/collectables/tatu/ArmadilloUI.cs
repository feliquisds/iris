using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmadilloUI : MonoBehaviour
{
    internal int armadilloCount = 0;
    internal bool move = false;
    internal Vector3 newTransform;
    internal float movement => Screen.width / 1920f;

    public void AddCount()
    {
        armadilloCount += 1;
        newTransform = transform.TransformPoint(GameObject.FindWithTag("GameCore").transform.position) - new Vector3(200f * movement, 0, 0);
        move = true;
    }

    void Update()
    {
        if (move) 
        {
            transform.position = Vector3.MoveTowards(transform.position, newTransform, 700f * Time.deltaTime);
            if (transform.position == newTransform) move = false;
        }
    }
}
