using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestruct : MonoBehaviour
{
    public AudioClip sound;
    public float volume;
    void Destroy() => Destroy(gameObject);
    void Awake() { if (TryGetComponent<AudioSource>(out AudioSource audioSource)) audioSource.PlayOneShot(sound, volume); }
}
