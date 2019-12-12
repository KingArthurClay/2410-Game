using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fadeScript : MonoBehaviour
{
    public string currentTarget = "Playground";

    public void switchLevel()
    {
        LevelManager.Instance.switchScene(currentTarget);
    }

    public void respawnPlayer()
    {
        SpawnRespawnHandler.Instance.respawnPlayer();
    }
}
