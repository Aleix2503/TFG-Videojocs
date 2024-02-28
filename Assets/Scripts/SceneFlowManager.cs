using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneFlowManager : MonoBehaviour
{
    // Method to go to a scene
    public void GoToScene(string name)
    {
        SceneManager.LoadScene(name);
    }


}