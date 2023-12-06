using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class EasterEgg : MonoBehaviour
{
    public GameObject background, roomPositionObject, outsidePositionObject, UI;
    public bool isOutsidePortal;
    internal GameObject player => GameObject.FindWithTag("Player");
    internal PlayerControl playerScript => player.GetComponent<PlayerControl>();
    internal Collider2D colli => GetComponent<Collider2D>();
    public CinemachineVirtualCamera vcam => GameObject.FindWithTag("CameraHandler").GetComponent<CinemachineVirtualCamera>();
    internal GradientHide fade => GameObject.FindWithTag("Fade").GetComponent<GradientHide>();
    internal CinemachineTransposer camDamping => vcam.GetCinemachineComponent<CinemachineTransposer>();

    void OnTriggerEnter2D(Collider2D _collider)
    {
        if (_collider.gameObject.tag == "Player") StartCoroutine(TeleportPlayer());
    }

    IEnumerator TeleportPlayer()
    {
        playerScript.controlEnabled = false;
        player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        StartCoroutine(ChangeCameraDamping(0, 0, 0));
        fade.hidden = true;
        yield return new WaitForSeconds(0.4f);

        player.transform.position = isOutsidePortal ? roomPositionObject.transform.position : outsidePositionObject.transform.position;
        background.SetActive(isOutsidePortal ? false : true);
        UI.SetActive(isOutsidePortal ? false : true);
        yield return new WaitForSeconds(0.1f);

        StartCoroutine(ChangeCameraDamping(2, 1, 1));
        fade.hidden = false;
        yield return new WaitForSeconds(0.4f);

        playerScript.controlEnabled = true;
    }

    IEnumerator ChangeCameraDamping(float valueX, float valueY, float valueZ)
    {
        yield return new WaitForSeconds(0.1f);
        camDamping.m_XDamping = valueX;
        camDamping.m_YDamping = valueY;
        camDamping.m_ZDamping = valueZ;
    }
}
