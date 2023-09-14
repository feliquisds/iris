using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public GameObject PrefabRangerShoot;
    public Transform shootPoint;
    public float shootForce;
    public RangerAnim RangerAnim;
    public RangeCollisor RangeCollisor;
    public float shootInterval;
    private float lastShootTime;

    private void Start()
    {
        lastShootTime = Time.time;
    }
    private void Update()
    {
        if (RangeCollisor.RangeAlert)
        {
            if (RangerAnim.isAttacking)
            {
                if (Time.time - lastShootTime >= shootInterval)
                {
                    Shoot();
                    lastShootTime = Time.time;
                }
            }
        }
    }

    void Shoot()
    {
        GameObject arrow = Instantiate(PrefabRangerShoot, shootPoint.position, Quaternion.identity);
        Rigidbody2D rb = arrow.GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            rb.AddForce(transform.right * shootForce, ForceMode2D.Impulse);
        }
    }
}
