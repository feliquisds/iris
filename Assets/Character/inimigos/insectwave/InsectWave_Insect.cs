using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InsectWave_Insect : MonoBehaviour
{
    public string color;
    public bool isCanvas;

    void Awake()
    {
        if (!isCanvas)
        {
            if (TryGetComponent<Animator>(out Animator anim) && color != "") anim.SetTrigger(color);
            GetComponent<Rigidbody2D>().velocity = new Vector2(3, 5);
            transform.localPosition = new Vector3(transform.localPosition.x, Random.Range(-200, 450), transform.localPosition.z);
        }
        else GetComponent<Canvas>().worldCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        StartCoroutine(AutoDestruct());
    }

    IEnumerator AutoDestruct()
    {
        yield return new WaitForSeconds(2.5f);
        Destroy(gameObject);
    }
}
