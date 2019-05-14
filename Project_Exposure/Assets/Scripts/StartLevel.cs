using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartLevel : MonoBehaviour
{
    void Start()
    {
        Button _button = GetComponent<Button>();
        _button.onClick.AddListener(LoadLevel);
    }

    void LoadLevel()
    {
        SceneManager.LoadScene(1);
    }
}
