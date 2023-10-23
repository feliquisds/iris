using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmadilloUI : MonoBehaviour
{
    internal int armadilloCount = 0;
    internal Vector3 newTransform;

    void Awake() => newTransform = transform.position;

    public void AddCount()
    {
        armadilloCount += 1;
        newTransform = newTransform - new Vector3(200, 0, 0);
    }

    void Update() => transform.position = Vector3.MoveTowards(transform.position, newTransform, 700f * Time.deltaTime);
}
