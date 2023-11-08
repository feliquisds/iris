using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRespawner : MonoBehaviour
{
    public void Respawn()
    {
        foreach (GameObject enemy in Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[])
        {
            if (enemy.tag == "Enemy")
            {
                enemy.SetActive(true);

                if (enemy.GetComponent<EnemyHealth>() != null)
                enemy.GetComponent<EnemyHealth>().health = enemy.GetComponent<EnemyHealth>().initialHealth;

                if (enemy.GetComponent<EnemyWalk>() != null)
                {
                    enemy.GetComponent<EnemyWalk>().freeze = false;
                    enemy.transform.position = enemy.GetComponent<EnemyWalk>().initialTransform.position;
                }
            }
        }
    }
}
