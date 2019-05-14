using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoalScript : MonoBehaviour
{
    [SerializeField] private GameObject _endMenu;
    [SerializeField] private Text _obstacleScore;
    [SerializeField] private ObstacleCountScript _obstacleCountScript;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.ToUpper() == "MAINCAMERA")
        {
            other.gameObject.GetComponent<Animator>().speed = 0;
            _obstacleScore.text = "Destroyed Obstacles: " + (_obstacleCountScript.TotalObstacleCount - _obstacleCountScript.GetCurrentObstacleCount()) + "/" + _obstacleCountScript.TotalObstacleCount;
            _endMenu.SetActive(true);
        }
    }
}
