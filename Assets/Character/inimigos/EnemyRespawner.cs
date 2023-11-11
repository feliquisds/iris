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

                if (enemy.TryGetComponent<EnemyHealth>(out EnemyHealth enemyHealth))
                enemyHealth.health = enemyHealth.initialHealth;

                if (enemy.TryGetComponent<EnemyWalk>(out EnemyWalk enemyWalk))
                {
                    enemyWalk.freeze = false;
                    enemy.transform.position = enemyWalk.initialTransform;
                }
            }
        }
    }
}
