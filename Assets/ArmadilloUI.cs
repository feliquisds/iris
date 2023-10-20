using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmadilloUI : MonoBehaviour
{
    internal int armadilloCount = 0;
    internal Vector3 newTransform;

    void Awake() => newTransform = this.transform.position;

    public void AddCount()
    {
        armadilloCount += 1;
        newTransform = newTransform - new Vector3(200f, 0, 0);
    }

    void Update() => this.transform.position = Vector3.MoveTowards(this.transform.position, newTransform, 700f * Time.deltaTime);
}
