using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Author: Arthur Clay Odom
 */

public class SpawnRespawnHandler : Singleton<SpawnRespawnHandler>
{

    public Vector3 respawnPosition = new Vector3(0, 0, 0);

    public ArrayList enemies = new ArrayList();

    public void findRespawn()
    {
        respawnPosition = new Vector3();
        respawnPosition.x = -Mathf.PI;

        //Iterate over level transitions to automactially select a spawnpoint
        foreach (GameObject spawnPoint in GameObject.FindGameObjectsWithTag("LevelTransition"))
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

        //If we don't find anything, just use the player's position in scene
        if (respawnPosition.x == -Mathf.PI)
        {
            respawnPosition = GameObject.FindGameObjectWithTag("Player").transform.position;
        }

        //Now save the enemy spawnpoints
        enemies = new ArrayList();

        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            enemies.Add(enemy.transform.position);
        }

    }

    public void respawnPlayer()
    {
        GameObject.FindGameObjectWithTag("Player").transform.position = respawnPosition;

        int i = 0;
        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            enemy.transform.position = (Vector3) enemies.ToArray()[i];
            i++;
        }
    }

}
