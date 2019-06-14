using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartLevel : MonoBehaviour
{
    int _levelToLoad = 1;

    void Start()
    {
        _levelToLoad = gameObject.scene.buildIndex + 1;
        Button _button = GetComponent<Button>();
        _button.onClick.AddListener(loadLevel);
    }

    void loadLevel()
    {
        SceneManager.LoadScene(_levelToLoad);
    }

    public void loadLevel(int pLevel){
        SceneManager.LoadScene(pLevel);
    }
}
