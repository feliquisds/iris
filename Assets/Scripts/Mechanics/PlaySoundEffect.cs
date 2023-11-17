using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySoundEffect : MonoBehaviour
{
    public AudioClip audioClip;
    internal AudioSource audioSource => GetComponent<AudioSource>();
    void Play() => audioSource.PlayOneShot(audioClip, 1f);
}
