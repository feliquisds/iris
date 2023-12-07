using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tracker : MonoBehaviour
{
    public bool shouldPlayCutscene;
    void Start() => DontDestroyOnLoad(this.gameObject);
}
