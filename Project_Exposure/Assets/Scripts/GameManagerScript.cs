using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerScript : MonoBehaviour
{
    [SerializeField] float _secondsBeforeGoingBackToMenu = 180f;
    float _time;
    bool _loading;

    void Update()
    {
        if (_loading)
            return;

        _time += Time.deltaTime;
        Debug.Log(_time);
        if (Input.anyKey)
        {
            _time = 0;
        }

        if (_time > _secondsBeforeGoingBackToMenu)
        {
            BackToMainMenu();
            _loading = true;
        }
    }


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
