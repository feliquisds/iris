using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmadilloUI : MonoBehaviour
{
    internal int armadilloCount = 0;
    internal Vector3 newTransform;
    internal float movement => Screen.width / 1920f;

    void Start() => newTransform = transform.position;

    public void AddCount()
    {
        armadilloCount += 1;
        newTransform = transform.position - new Vector3(200f * movement, 0, 0);
    }

    void Update() => transform.position = Vector3.MoveTowards(transform.position, newTransform, 700f * Time.deltaTime);
}
