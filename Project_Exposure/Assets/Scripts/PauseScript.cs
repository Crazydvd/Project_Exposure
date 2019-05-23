using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PauseScript : MonoBehaviour
{
    [SerializeField] GameObject _pauseScreen;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Toggle();
        }
    }

    public static bool Paused { get; set; }

    public void Toggle()
    {
        if (_pauseScreen.activeSelf)
        {
            Time.timeScale = 1;
            _pauseScreen.SetActive(false);
            Paused = false;
        }
        else
        {
            Time.timeScale = 0;
            _pauseScreen.SetActive(true);
            Paused = true;
        }
    }

    public void Pause()
    {
        Time.timeScale = 0;
        _pauseScreen.SetActive(true);
        Paused = true;
    }

    public void Resume()
    {
        Time.timeScale = 1;
        _pauseScreen.SetActive(false);
        Paused = false;
    }
}
