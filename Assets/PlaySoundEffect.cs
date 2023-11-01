using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySoundEffect : MonoBehaviour
{
    public AudioClip audioClip;
    void Play() => GetComponent<AudioSource>().PlayOneShot(audioClip, 1f);
}
