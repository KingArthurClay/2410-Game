﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/**
 * Author: Arthur Clay Odom
 */

public class LevelManager : Singleton<LevelManager>
{
    public string lastScene { get; private set; }

    public Animator anim;

    public void switchScene(string switchTo)
    {
        lastScene = SceneManager.GetActiveScene().name;

        AssetBundle.LoadFromFile(System.IO.Path.Combine(Application.streamingAssetsPath, "switchTo"));

        SceneManager.LoadScene(switchTo);

        SpawnRespawnHandler.Instance.findRespawn();
    }

}
