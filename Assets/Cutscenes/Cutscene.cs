using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class Cutscene : MonoBehaviour
{
    internal VideoPlayer video => GetComponent<VideoPlayer>();
    void Start() => video.loopPointReached += EndReached;
    void EndReached(VideoPlayer vp) => SceneManager.LoadScene(1);
}
