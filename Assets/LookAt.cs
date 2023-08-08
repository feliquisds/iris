using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace LookAt.Mechanics
{
    /// <summary>
    /// If you want something to look at another thing.
    /// </summary>
public class LookAt : MonoBehaviour
{
    /// <summary>
        /// Look at object.
        /// </summary>
        public GameObject obj;

        SpriteRenderer spriteRenderer;
        Animator animator;


    // Update is called once per frame
    void Update()
    {
         if (obj.transform.position.x > transform.position.x)
                spriteRenderer.flipX = false;
            else if (obj.transform.position.x < transform.position.x)
                spriteRenderer.flipX = true;
    }
}
}
