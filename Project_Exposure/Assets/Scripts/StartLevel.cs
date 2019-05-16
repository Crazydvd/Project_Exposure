using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartLevel : MonoBehaviour
{
    void Start()
    {
        Button _button = GetComponent<Button>();
        _button.onClick.AddListener(loadLevel);
    }

    void loadLevel()
    {
        SceneManager.LoadScene(1);
    }
}
