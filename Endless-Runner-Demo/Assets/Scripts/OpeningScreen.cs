using UnityEngine;
using UnityEngine.SceneManagement;

public class OpeningScreen : MonoBehaviour
{
    // Connect this to the START button's OnClick event
    public void StartGame()
    {
        SceneManager.LoadScene("SampleScene");
    }

    // Connect this to the TUTORIAL button's OnClick event
    public void LoadTutorial()
    {
        SceneManager.LoadScene("Tutorial");
    }

    // Connect this to the QUIT button's OnClick event
    public void QuitGame()
    {
        Debug.Log("Quitting Game...");

        // Stops play mode if you are running the game inside the Unity Editor
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // Quits the application if it's a compiled build
        Application.Quit();
#endif
    }
}