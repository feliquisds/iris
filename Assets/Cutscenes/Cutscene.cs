using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class Cutscene : MonoBehaviour
{
    internal VideoPlayer video => GetComponent<VideoPlayer>();
    internal float videoTime;
    void Start()
    {
        videoTime = (float)video.length - 2f;
        Invoke("VideoEnded", videoTime);
    }
    void VideoEnded() => SceneManager.LoadScene(1);   
}
