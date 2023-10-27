using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class InsectWave : MonoBehaviour
{
    internal Vector3 newTransform, initialTransform, newCameraTransform, initialCameraTransform, newDeathTransform, initialDeathTransform;
    internal float movement => Screen.width / 1366f;
    internal float goalCam = 3.5f;
    internal GameObject wave => GameObject.FindWithTag("InsectWave");
    internal PlayerControl playerScript => GameObject.FindWithTag("Player").GetComponent<PlayerControl>();
    internal GameObject player => GameObject.FindWithTag("Player");
    public bool activated => wave.transform.position != initialTransform;
    public bool isDeathZone;
    internal bool chasing, canSpawnInsect, canSpawnDebris, stopSpawnInsect = false, stopSpawnDebris = false;
    public CinemachineVirtualCamera vcam => GameObject.FindWithTag("CameraHandler").GetComponent<CinemachineVirtualCamera>();
    internal GameObject cameraPoint => GameObject.FindWithTag("CustomCamera");
    public GameObject insect1, insect2, insect3;
    public GameObject[] debris;

    void Start()
    {
        if (isDeathZone) initialDeathTransform = newDeathTransform = transform.position;
        else
        {
            initialTransform = newTransform = wave.transform.position;
            initialCameraTransform = newCameraTransform = cameraPoint.transform.position;
        }
    }

    void OnTriggerEnter2D(Collider2D _collider)
    {
        if (_collider.gameObject.tag == "Player" && !activated && !isDeathZone) StartChase();
    }

    void OnTriggerExit2D(Collider2D _collider)
    {
        if (_collider.gameObject.tag == "Player" && isDeathZone) StartCoroutine(StartDeathZone());
    }

    IEnumerator StartDeathZone()
    {
        yield return new WaitForSeconds(0.93f);
        GetComponent<DeathZone>().active = true;
        newDeathTransform = transform.position + new Vector3(74f, 0, 0);
    }

    void StartChase()
    {
        newTransform = wave.transform.position + new Vector3(800f * movement, 0, 0);
        newCameraTransform = cameraPoint.transform.position + new Vector3(85f, 0, 0);
        vcam.m_Follow = cameraPoint.transform;
        vcam.m_LookAt = cameraPoint.transform;
        goalCam = 5f;
        chasing = true;
        StartCoroutine(StartInsectSpawn());
    }

    IEnumerator StartInsectSpawn()
    {
        yield return new WaitForSeconds(1f);
        stopSpawnInsect = false;
        canSpawnInsect = true;
        stopSpawnDebris = false;
        canSpawnDebris = true;
    }

    void StopInsectSpawn()
    {
        stopSpawnInsect = true;
        canSpawnInsect = false;
        stopSpawnDebris = true;
        canSpawnDebris = false;
        GameObject[] insects = GameObject.FindGameObjectsWithTag("InsectWave_Insect");
        foreach (GameObject insect in insects) Destroy(insect);
    }

    IEnumerator InsectSpawn()
    {
        canSpawnInsect = false;
        yield return new WaitForSeconds(Random.value);
        GameObject insect = Instantiate(Random.value > 0.49f ? insect1 : insect2, transform.position, transform.rotation);
        canSpawnInsect = true;
    }

    IEnumerator DebrisSpawn()
    {
        canSpawnDebris = false;
        yield return new WaitForSeconds(Random.Range(0f, 0.5f));

        var debris1 = debris[Random.Range(0, debris.Length)];
        GameObject deb1 = Instantiate(debris1, transform.position, transform.rotation);

        canSpawnDebris = true;
    }

    public void Reset()
    {
        if (isDeathZone)
        {
            transform.position = newDeathTransform = initialDeathTransform;
            GetComponent<DeathZone>().active = false;
        }
        else
        {
            vcam.m_Follow = GameObject.FindWithTag("PlayerCameraPoint").transform;
            vcam.m_LookAt = GameObject.FindWithTag("PlayerCameraPoint").transform;
            wave.transform.position = newTransform = initialTransform;
            cameraPoint.transform.position = newCameraTransform = initialCameraTransform;

            vcam.m_Lens.OrthographicSize = goalCam = 3.5f;
            chasing = false;
            StopInsectSpawn();
        }
    }

    void Update()
    {
        if (isDeathZone) transform.position = Vector3.MoveTowards(transform.position, newDeathTransform, 4.5f * Time.deltaTime);
        else
        {
            wave.transform.position = Vector3.MoveTowards(wave.transform.position, newTransform, 350f * Time.deltaTime);
            cameraPoint.transform.position = Vector3.MoveTowards(cameraPoint.transform.position, newCameraTransform, 4.5f * Time.deltaTime);

            if (vcam.m_Lens.OrthographicSize < goalCam) vcam.m_Lens.OrthographicSize += 0.1f;

            if (!player.GetComponent<Renderer>().isVisible && chasing) StartCoroutine(playerScript.Die());

            if (canSpawnInsect && !stopSpawnInsect) StartCoroutine(InsectSpawn());
            if (canSpawnDebris && !stopSpawnDebris) StartCoroutine(DebrisSpawn());
        }
        if (playerScript.respawning) Reset();
    }
}
