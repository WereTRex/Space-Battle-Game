using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class SceneManagerScript
{
    public static void StartGame()
    {
        SceneManager.LoadSceneAsync((int)SceneIndexes.MAINSCENE, LoadSceneMode.Additive);
    }

    public static void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
