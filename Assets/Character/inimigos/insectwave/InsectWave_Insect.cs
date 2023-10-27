using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InsectWave_Insect : MonoBehaviour
{
    public string color;
    public bool isCanvas;

    void Awake()
    {
        if (!isCanvas)
        {
            GetComponent<Animator>().SetTrigger(color);
            GetComponent<Rigidbody2D>().velocity = new Vector2(325f, 300f);
            transform.position = new Vector3(0f, Random.Range(200f, 650f), 0f);
        }
        StartCoroutine(AutoDestruct());
    }

    IEnumerator AutoDestruct()
    {
        yield return new WaitForSeconds(2.5f);
        Destroy(gameObject);
    }
}
