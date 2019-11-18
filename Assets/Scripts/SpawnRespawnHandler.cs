using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Author: Arthur Clay Odom
 */

public class SpawnRespawnHandler : Singleton<SpawnRespawnHandler>
{

    public Vector3 respawnPosition = new Vector3(0, 0, 0);

    public void findRespawn()
    {
        GameObject[] spawnPoints = GameObject.FindGameObjectsWithTag("LevelTransition");

        foreach (GameObject spawnPoint in spawnPoints)
        {
            LevelTransition LT;

            if (spawnPoint.TryGetComponent<LevelTransition>(out LT) && LT != null)
            {
                if (LT.target == LevelManager.Instance.lastScene)
                {
                    this.respawnPosition = LT.transform.position;
                }
            }
        }

        respawnPosition = GameObject.FindGameObjectWithTag("Player").transform.position;

    }

    public void respawnPlayer()
    {
        GameObject.FindGameObjectWithTag("Player").transform.position = respawnPosition;
    }

}
