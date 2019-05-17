using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoalScript : MonoBehaviour
{
    [SerializeField] GameObject _endMenu;
    [SerializeField] Text _obstacleScore;
    [SerializeField] ObstacleCountScript _obstacleCountScript;
    [SerializeField] HighscoreScript _highscoreScript;
    [SerializeField] int _level = 1;

    void OnTriggerEnter(Collider other)
    {
        if (other.tag.ToUpper() == "MAINCAMERA")
        {
            other.gameObject.GetComponent<Animator>().speed = 0;
            _obstacleScore.text = "Destroyed Obstacles: " + (_obstacleCountScript.TotalObstacleCount - _obstacleCountScript.GetCurrentObstacleCount()) + "/" + _obstacleCountScript.TotalObstacleCount;
            _endMenu.SetActive(true);

            _highscoreScript.AddEntry("Klox Mastermann", _obstacleCountScript.TotalObstacleCount - _obstacleCountScript.GetCurrentObstacleCount(), _level);
        }
    }
}
