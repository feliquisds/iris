using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalBoss : MonoBehaviour
{
    public float velocityX;
    public bool dies, attack1, attack2;
    internal bool started = false;
    internal Animator anim => GetComponent<Animator>();

    void Awake()
    {
        if (dies || attack1 || attack2) anim.SetTrigger(dies ? "death" : attack1 ? "attack1" : "attack2");
    }

    void Update()
    {
        anim.SetFloat("velocityX", velocityX);

        if (!started) TestStart();
    }

    IEnumerator Test(string animation)
    {
        yield return new WaitForSeconds(dies ? 4f : 2f);
        anim.SetTrigger(animation);
        TestStart();
    }

    void TestStart()
    {
        started = true;
        if (dies || attack1 || attack2) StartCoroutine(Test(dies ? "death" : attack1 ? "attack1" : "attack2"));
    }
}
