using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerScript : MonoBehaviour
{
    public void BackToMainMenu(){
        Time.timeScale = 1f;
        LoadingScreenScript.Load(0);
    }
    public void ReloadScene()
    {
        Time.timeScale = 1f;
        LoadingScreenScript.Load(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadScene(int pScene)
    {
        Time.timeScale = 1f;
        LoadingScreenScript.Load(pScene);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
