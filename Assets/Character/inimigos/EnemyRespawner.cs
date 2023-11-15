using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRespawner : MonoBehaviour
{
    public void Respawn()
    {
        foreach (GameObject enemy in Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[])
        {
            if (enemy.tag == "Enemy") enemy.SetActive(true);
        }
    }
}
