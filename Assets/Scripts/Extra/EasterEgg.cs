using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using Platformer.UI;

public class EasterEgg : MonoBehaviour
{
    public GameObject background, roomPositionObject, outsidePositionObject, UI, members;
    public AudioClip song;
    public bool isOutsidePortal;
    internal GameObject player => GameObject.FindWithTag("Player");
    internal PlayerControl playerScript => player.GetComponent<PlayerControl>();
    internal Collider2D colli => GetComponent<Collider2D>();
    public CinemachineVirtualCamera vcam => GameObject.FindWithTag("CameraHandler").GetComponent<CinemachineVirtualCamera>();
    internal GameObject fadeObj => GameObject.FindWithTag("Fade");
    internal Image fadeImg => fadeObj.GetComponent<Image>();
    internal GradientHide fade => fadeObj.GetComponent<GradientHide>();
    internal CinemachineTransposer camDamping => vcam.GetCinemachineComponent<CinemachineTransposer>();
    internal AudioSource music => GameObject.FindWithTag("GameCore").GetComponent<AudioSource>();
    internal MetaGameController gameController => GameObject.FindWithTag("GameCore").GetComponent<MetaGameController>();
    internal bool fadeOutMusic, lockPlayer;

    void Awake()
    {
        if (members != null)
        {
            for (int i = 1; i < members.transform.childCount + 1; i++)
                members.transform.GetChild(i - 1).gameObject.GetComponent<Animator>().SetTrigger((string)("member" + i));
        }
    }

    void OnTriggerEnter2D(Collider2D _collider)
    {
        if (_collider.gameObject.tag == "Player") StartCoroutine(TeleportPlayer());
    }

    void Update()
    {
        if (fadeOutMusic)
        {
            if (music.volume > 0) music.volume -= 2 * Time.unscaledDeltaTime;
            else
            {
                music.Stop();
                fadeOutMusic = false;
            }
        }
        if (lockPlayer)
        {
            playerScript.controlEnabled = playerScript.canCrouch = false;
            playerScript.sprite.flipX = isOutsidePortal ? false : true;
            playerScript.rb.velocity = Vector2.zero;
        }
    }

    IEnumerator TeleportPlayer()
    {
        var oldMusicVolume = music.volume;

        gameController.canPause = false;
        fade.opaque = fadeOutMusic = lockPlayer = true;
        StartCoroutine(ChangeCameraDamping(0, 0, 0));
        StartCoroutine(UnlockPlayer());
        yield return new WaitForSecondsRealtime(0.4f);

        music.clip = song;
        player.transform.position = isOutsidePortal ? roomPositionObject.transform.position : outsidePositionObject.transform.position;
        background.SetActive(isOutsidePortal ? false : true);
        UI.SetActive(isOutsidePortal ? false : true);
        yield return new WaitForSecondsRealtime(0.1f);

        music.volume = oldMusicVolume;
        music.Play();
        StartCoroutine(ChangeCameraDamping(2, 1, 1));
        fade.opaque = false;
    }

    IEnumerator UnlockPlayer()
    {
        yield return new WaitForSecondsRealtime(0.9f);
        lockPlayer = false;
        playerScript.controlEnabled = playerScript.canCrouch = gameController.canPause = true;
    }

    IEnumerator ChangeCameraDamping(float valueX, float valueY, float valueZ)
    {
        yield return new WaitForSecondsRealtime(0.1f);
        camDamping.m_XDamping = valueX;
        camDamping.m_YDamping = valueY;
        camDamping.m_ZDamping = valueZ;
    }
}
