using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TheVictoryScreen : MonoBehaviour
{

    public void gotoMainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }

}
