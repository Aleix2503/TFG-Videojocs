using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CommonMenuActions : MonoBehaviour
{
    public void LoadSceneByName(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void QuitGame()
    {
        print("This is a debug message to show that the game should be closing once this function is called. It will work when the game is an actual app.");
        Application.Quit();
    }
}
