using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Pause : MonoBehaviour
{
    [SerializeField] GameObject _pauseScreen;
    [SerializeField] GameObject _button;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
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
                EventSystem.current.SetSelectedGameObject(_button);
            }
        }
    }

    public static bool Paused { get; set; }

    public void Resume()
    {
        Time.timeScale = 1;
        _pauseScreen.SetActive(false);
        Paused = false;
    }
}
