using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class InsectWave : MonoBehaviour
{
    internal Vector3 newTransform, initialTransform, newCameraTransform, initialCameraTransform, newDeathTransform, initialDeathTransform;
    internal float goalCam = 3.5f;
    internal GameObject wave => GameObject.FindWithTag("InsectWave");
    internal PlayerControl playerScript => GameObject.FindWithTag("Player").GetComponent<PlayerControl>();
    internal bool playerRendering => GameObject.FindWithTag("Player").GetComponent<Renderer>().isVisible;
    public bool isDeathZone, sceneEnding;
    internal bool chasing, canSpawnInsect, canSpawnDebris, stopSpawnInsect = false, stopSpawnDebris = false;
    public CinemachineVirtualCamera vcam => GameObject.FindWithTag("CameraHandler").GetComponent<CinemachineVirtualCamera>();
    internal GameObject cameraPoint => GameObject.FindWithTag("CustomCamera");
    internal AudioSource insectSound => cameraPoint.GetComponent<AudioSource>();
    public GameObject insect1, insect2;
    public GameObject[] debris;

    void Awake()
    {
        if (isDeathZone) initialDeathTransform = newDeathTransform = transform.position;
        else
        {
            initialTransform = newTransform = wave.transform.localPosition;
            initialCameraTransform = newCameraTransform = cameraPoint.transform.position;
        }
    }

    void OnTriggerEnter2D(Collider2D _collider)
    {
        if (_collider.gameObject.tag == "Player" && !chasing && !isDeathZone) StartChase();
        if (_collider.gameObject.tag == "Player" && isDeathZone) StartCoroutine(StartDeathZone());
    }

    IEnumerator StartDeathZone()
    {
        yield return new WaitForSeconds(2f);
        GetComponent<DeathZone>().active = true;
        newDeathTransform = transform.position + new Vector3(74, 0, 0);
    }

    void StartChase()
    {
        insectSound.mute = false;
        newTransform = wave.transform.localPosition + new Vector3(815, 0, 0);
        newCameraTransform = cameraPoint.transform.position + new Vector3(85, 0, 0);
        vcam.m_Follow = vcam.m_LookAt = cameraPoint.transform;
        goalCam = 5f;
        chasing = true;
        StartCoroutine(CanSpawnObjects(true));
    }

    IEnumerator CanSpawnObjects(bool start)
    {
        yield return new WaitForSeconds(start ? 1 : 0);
        stopSpawnInsect = stopSpawnDebris = !start;
        canSpawnInsect = canSpawnDebris = start;
        if (!start)
        {
            GameObject[] insects = GameObject.FindGameObjectsWithTag("InsectWave_Insect");
            foreach (GameObject insect in insects) Destroy(insect);
        }
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
            vcam.m_Follow = vcam.m_LookAt = GameObject.FindWithTag("PlayerCameraPoint").transform;
            wave.transform.localPosition = newTransform = initialTransform;
            cameraPoint.transform.position = newCameraTransform = initialCameraTransform;

            vcam.m_Lens.OrthographicSize = goalCam = 3.5f;
            insectSound.mute = true;
            chasing = false;
            StartCoroutine(CanSpawnObjects(false));
        }
    }

    void Update()
    {
        if (isDeathZone) transform.position = Vector3.MoveTowards(transform.position, newDeathTransform, 4.5f * Time.deltaTime);
        else
        {
            wave.transform.localPosition = Vector3.MoveTowards(wave.transform.localPosition, newTransform, 350 * Time.deltaTime);
            cameraPoint.transform.position = Vector3.MoveTowards(cameraPoint.transform.position, newCameraTransform, 4.5f * Time.deltaTime);

            if (vcam.m_Lens.OrthographicSize < goalCam && Time.timeScale != 0) vcam.m_Lens.OrthographicSize += 0.01f;

            if (!playerRendering && chasing) StartCoroutine(playerScript.Die());

            if (canSpawnInsect && !stopSpawnInsect) StartCoroutine(InsectSpawn());
            if (canSpawnDebris && !stopSpawnDebris) StartCoroutine(DebrisSpawn());
        }
        if (playerScript.respawning) Reset();
        if (sceneEnding && insectSound.volume > 0) insectSound.volume -= (2f * Time.unscaledDeltaTime);
    }
}
